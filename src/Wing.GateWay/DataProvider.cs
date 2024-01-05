using System.Collections.Concurrent;
using System.Net;
using Polly;
using Polly.Bulkhead;
using Polly.RateLimit;
using Polly.Timeout;
using Wing.Exceptions;
using Wing.Gateway.Config;
using Wing.Persistence.GateWay;
using Wing.ServiceProvider;

namespace Wing.Gateway
{
    public class DataProvider
    {
        public static readonly ConcurrentQueue<LogAddDto> Data = new();

        public static LogConfig LogConfig => App.GetConfig<LogConfig>("Gateway:Log");

        private static readonly IServiceFactory _serviceFactory = App.GetRequiredService<IServiceFactory>();

        private static readonly ConcurrentDictionary<string, KeyValuePair<Config.Policy, AsyncPolicy<ServiceContext>>> Policies = new();

        public static async Task<ServiceContext> InvokeWithPolicy(ServiceContext serviceContext)
        {
            var config = serviceContext.Policy;
            Policies.TryGetValue(serviceContext.ServiceName, out KeyValuePair<Config.Policy, AsyncPolicy<ServiceContext>> policyPair);
            var policy = policyPair.Value;
            if (policy == null || policyPair.Key != config)
            {
                policy = Polly.Policy.NoOpAsync<ServiceContext>();

                if (config.Breaker != null && config.Breaker.IsEnabled)
                {
                    policy = policy.WrapAsync<ServiceContext>(Polly.Policy.Handle<ServiceNotFoundException>()
                        .Or<Exception>()
                        .CircuitBreakerAsync(config.Breaker.ExceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(config.Breaker.MillisecondsOfBreak)));
                }

                if (config.Retry != null && config.Retry.IsEnabled)
                {
                    policy = policy.WrapAsync<ServiceContext>(Polly.Policy.Handle<ServiceNotFoundException>()
                        .Or<Exception>()
                        .WaitAndRetryAsync(config.Retry.MaxTimes, i => TimeSpan.FromMilliseconds(config.Retry.IntervalMilliseconds)));
                }

                if (config.BulkHead != null && config.BulkHead.IsEnabled)
                {
                    policy = policy.WrapAsync<ServiceContext>(Polly.Policy.BulkheadAsync<ServiceContext>(config.BulkHead.MaxParallelization, config.BulkHead.MaxQueuingActions));
                }

                if (config.RateLimit != null && config.RateLimit.IsEnabled)
                {
                    policy = policy.WrapAsync<ServiceContext>(Polly.Policy.RateLimitAsync<ServiceContext>(config.RateLimit.NumberOfExecutions, TimeSpan.FromSeconds(config.RateLimit.PerSeconds), config.RateLimit.MaxBurst));
                }

                if (config.TimeOut != null && config.TimeOut.IsEnabled)
                {
                    policy = policy.WrapAsync<ServiceContext>(Polly.Policy.TimeoutAsync<ServiceContext>(() => TimeSpan.FromMilliseconds(config.TimeOut.TimeOutMilliseconds), TimeoutStrategy.Pessimistic));
                }

                var unknownErrorFallBack = Policy<ServiceContext>
                .Handle<Exception>()
                .FallbackAsync((d, cxt, t) =>
                {
                    if (d.Exception.InnerException is ServiceNotFoundException)
                    {
                        serviceContext.Exception = Tag.SERVICE_NOT_FOUND;
                        serviceContext.StatusCode = (int)HttpStatusCode.NotFound;
                        return Task.FromResult(serviceContext);
                    }

                    serviceContext.Exception = Tag.ExceptionFormat(Tag.UNKNOWN_FALLBACK, d.Exception);
                    serviceContext.StatusCode = (int)HttpStatusCode.BadGateway;
                    return Task.FromResult(serviceContext);
                }, (d, cxt) =>
                {
                    serviceContext.Exception = Tag.ExceptionFormat(Tag.UNKNOWN_FALLBACK, d.Exception);
                    return Task.CompletedTask;
                });

                var serviceNotFoundFallBack = Policy<ServiceContext>
                    .Handle<ServiceNotFoundException>()
                    .FallbackAsync(t =>
                    {
                        serviceContext.Exception = Tag.SERVICE_NOT_FOUND;
                        serviceContext.StatusCode = (int)HttpStatusCode.NotFound;
                        return Task.FromResult(serviceContext);
                    });

                var timeoutFallBack = Policy<ServiceContext>
                   .Handle<TimeoutRejectedException>()
                   .Or<TimeoutException>()
                   .FallbackAsync(t =>
                   {
                       serviceContext.Exception = Tag.TIME_OUT_FALLBACK;
                       serviceContext.StatusCode = (int)HttpStatusCode.GatewayTimeout;
                       return Task.FromResult(serviceContext);
                   });

                var rateLimitFallBack = Policy<ServiceContext>
                   .Handle<RateLimitRejectedException>()
                   .FallbackAsync(t =>
                   {
                       serviceContext.Exception = Tag.RATE_LIMIT_FALLBACK;
                       serviceContext.StatusCode = (int)HttpStatusCode.TooManyRequests;
                       return Task.FromResult(serviceContext);
                   });

                var bulkHeadFallBack = Policy<ServiceContext>
                   .Handle<BulkheadRejectedException>()
                   .FallbackAsync(t =>
                   {
                       serviceContext.Exception = Tag.BULK_HEAD_FALLBACK;
                       serviceContext.StatusCode = (int)HttpStatusCode.TooManyRequests;
                       return Task.FromResult(serviceContext);
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

            return await policy.ExecuteAsync(async () =>
            {
                return await _serviceFactory.InvokeAsync(serviceContext.ServiceName, async serviceAddr =>
                {
                    serviceContext.ServiceAddress = serviceAddr.ToString();
                    return await serviceContext.HttpContext.Request.Request(serviceContext);
                });
            });
        }

        public static async Task<ServiceContext> InvokeWithNoPolicy(ServiceContext serviceContext)
        {
            var context = serviceContext.HttpContext;
            return await _serviceFactory.InvokeAsync(serviceContext.ServiceName, async serviceAddr =>
            {
                serviceContext.ServiceAddress = serviceAddr.ToString();
                return await context.Request.Request(serviceContext);
            });
        }
    }
}
