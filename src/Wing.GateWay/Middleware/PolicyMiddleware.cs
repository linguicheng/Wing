using System.Collections.Concurrent;
using System.Net;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Bulkhead;
using Polly.RateLimit;
using Polly.Timeout;
using Wing.Exceptions;
using Wing.ServiceProvider;

namespace Wing.Gateway.Middleware
{
    public class PolicyMiddleware
    {
        private static readonly ConcurrentDictionary<string, KeyValuePair<Config.Policy, AsyncPolicy<HttpResponseMessage>>> Policies = new ConcurrentDictionary<string, KeyValuePair<Config.Policy, AsyncPolicy<HttpResponseMessage>>>();
        private readonly ServiceRequestDelegate _next;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IServiceFactory _serviceFactory;
        private readonly ILogger<PolicyMiddleware> _logger;
        private readonly ILogProvider _logProvider;

        public PolicyMiddleware(ServiceRequestDelegate next,
            IHttpClientFactory clientFactory,
            IServiceFactory serviceFactory,
            ILogger<PolicyMiddleware> logger,
            ILogProvider logProvider)
        {
            _next = next;
            _clientFactory = clientFactory;
            _serviceFactory = serviceFactory;
            _logger = logger;
            _logProvider = logProvider;
        }

        public async Task InvokeAsync(ServiceContext serviceContext)
        {
            if (string.IsNullOrWhiteSpace(serviceContext.ServiceName) || serviceContext.IsWebSocket)
            {
                await _next(serviceContext);
                return;
            }

            await InvokeWithPolicy(serviceContext);
        }

        private async Task InvokeWithPolicy(ServiceContext serviceContext)
        {
            var config = serviceContext.Policy;
            Policies.TryGetValue(serviceContext.ServiceName, out KeyValuePair<Config.Policy, AsyncPolicy<HttpResponseMessage>> policyPair);
            var policy = policyPair.Value;
            if (policy == null || policyPair.Key != config)
            {
                policy = Policy.NoOpAsync<HttpResponseMessage>();

                if (config.Breaker != null && config.Breaker.IsEnabled)
                {
                    policy = policy.WrapAsync<HttpResponseMessage>(Policy.Handle<ServiceNotFoundException>()
                        .Or<Exception>()
                        .CircuitBreakerAsync(config.Breaker.ExceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(config.Breaker.MillisecondsOfBreak)));
                }

                if (config.Retry != null && config.Retry.IsEnabled)
                {
                    policy = policy.WrapAsync<HttpResponseMessage>(Policy.Handle<ServiceNotFoundException>()
                        .Or<Exception>()
                        .WaitAndRetryAsync(config.Retry.MaxTimes, i => TimeSpan.FromMilliseconds(config.Retry.IntervalMilliseconds)));
                }

                if (config.BulkHead != null && config.BulkHead.IsEnabled)
                {
                    policy = policy.WrapAsync<HttpResponseMessage>(Policy.BulkheadAsync<HttpResponseMessage>(config.BulkHead.MaxParallelization, config.BulkHead.MaxQueuingActions));
                }

                if (config.RateLimit != null && config.RateLimit.IsEnabled)
                {
                    policy = policy.WrapAsync<HttpResponseMessage>(Policy.RateLimitAsync<HttpResponseMessage>(config.RateLimit.NumberOfExecutions, TimeSpan.FromSeconds(config.RateLimit.PerSeconds), config.RateLimit.MaxBurst));
                }

                if (config.TimeOut != null && config.TimeOut.IsEnabled)
                {
                    policy = policy.WrapAsync<HttpResponseMessage>(Policy.TimeoutAsync<HttpResponseMessage>(() => TimeSpan.FromMilliseconds(config.TimeOut.TimeOutMilliseconds), TimeoutStrategy.Pessimistic));
                }

                var unknownErrorFallBack = Policy<HttpResponseMessage>
                .Handle<Exception>()
                .FallbackAsync((d, cxt, t) =>
                {
                    if (d.Exception.InnerException is ServiceNotFoundException)
                    {
                        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
                    }

                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadGateway));
                }, (d, cxt) =>
                {
                    _logger.LogError(d.Exception, "触发异常降级");
                    return Task.CompletedTask;
                });

                var serviceNotFoundFallBack = Policy<HttpResponseMessage>
                    .Handle<ServiceNotFoundException>()
                    .FallbackAsync(t =>
                    {
                        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
                    });

                var timeoutFallBack = Policy<HttpResponseMessage>
                   .Handle<TimeoutRejectedException>()
                   .Or<TimeoutException>()
                   .FallbackAsync(t =>
                   {
                       _logger.LogError("触发超时异常降级");
                       return Task.FromResult(new HttpResponseMessage(HttpStatusCode.GatewayTimeout));
                   });

                var rateLimitFallBack = Policy<HttpResponseMessage>
                   .Handle<RateLimitRejectedException>()
                   .FallbackAsync(t =>
                   {
                       _logger.LogError("触发限流异常降级");
                       return Task.FromResult(new HttpResponseMessage(HttpStatusCode.TooManyRequests));
                   });

                var bulkHeadFallBack = Policy<HttpResponseMessage>
                   .Handle<BulkheadRejectedException>()
                   .FallbackAsync(t =>
                   {
                       _logger.LogError("触发舱壁异常降级");
                       return Task.FromResult(new HttpResponseMessage(HttpStatusCode.TooManyRequests));
                   });

                policy = unknownErrorFallBack.WrapAsync(serviceNotFoundFallBack)
                    .WrapAsync(timeoutFallBack)
                    .WrapAsync(rateLimitFallBack)
                    .WrapAsync(bulkHeadFallBack)
                    .WrapAsync(policy);
                Policies.AddOrUpdate(serviceContext.ServiceName, KeyValuePair.Create(config, policy), (key, value) =>
                {
                    return KeyValuePair.Create(config, policy);
                });
            }

            var resMsg = await policy.ExecuteAsync(async () =>
            {
                return await _serviceFactory.InvokeAsync(serviceContext.ServiceName, async serviceAddr =>
                {
                    serviceContext.ServiceAddress = serviceAddr.ToString();
                    var reqMsg = await serviceContext.HttpContext.Request.ToHttpRequestMessage(serviceAddr, serviceContext.DownstreamPath);
                    var client = _clientFactory.CreateClient(serviceContext.ServiceName);
                    return await client.SendAsync(reqMsg);
                });
            });
            await serviceContext.HttpContext.Response.FromHttpResponseMessage(resMsg, (statusCode, content) =>
            {
                serviceContext.StatusCode = statusCode;
                serviceContext.ResponseValue = content;
                _logProvider.Add(serviceContext);
            });
        }
    }
}
