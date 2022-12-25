using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Timeout;
using Wing.Exceptions;
using Wing.ServiceProvider;

namespace Wing.GateWay.Middleware
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
            if (string.IsNullOrWhiteSpace(serviceContext.ServiceName))
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
                policy = Polly.Policy.NoOpAsync<HttpResponseMessage>();
                if (config.TimeOutMilliseconds > 0)
                {
                    policy = Policy<HttpResponseMessage>
                   .Handle<TimeoutRejectedException>()
                   .Or<TimeoutException>()
                   .FallbackAsync(t =>
                   {
                       return Task.FromResult(new HttpResponseMessage(HttpStatusCode.GatewayTimeout));
                   }).WrapAsync<HttpResponseMessage>(Polly.Policy.TimeoutAsync<HttpResponseMessage>(() => TimeSpan.FromMilliseconds(config.TimeOutMilliseconds), TimeoutStrategy.Pessimistic));
                }

                if (config.IsEnableBreaker)
                {
                    policy = policy.WrapAsync<HttpResponseMessage>(Polly.Policy.Handle<ServiceNotFoundException>().Or<Exception>().CircuitBreakerAsync(config.ExceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(config.MillisecondsOfBreak)));
                }

                if (config.MaxRetryTimes > 0)
                {
                    policy = policy.WrapAsync<HttpResponseMessage>(Polly.Policy.Handle<ServiceNotFoundException>().Or<Exception>().WaitAndRetryAsync(config.MaxRetryTimes, i => TimeSpan.FromMilliseconds(config.RetryIntervalMilliseconds)));
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

                policy = unknownErrorFallBack.WrapAsync(serviceNotFoundFallBack).WrapAsync(policy);
                Policies.AddOrUpdate(serviceContext.ServiceName, KeyValuePair.Create(config, policy), (key, value) =>
                {
                    return KeyValuePair.Create(config, policy);
                });
            }

            var resMsg = await policy.ExecuteAsync(async () =>
            {
                return await _serviceFactory.HttpServiceInvokeAsync(serviceContext.ServiceName, async serviceAddr =>
                {
                    serviceContext.ServiceAddress = serviceAddr.ToString();
                    var reqMsg = serviceContext.HttpContext.Request.ToHttpRequestMessage(serviceAddr, serviceContext.DownstreamPath);
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
