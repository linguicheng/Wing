using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace Wing.Policy
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PolicyAttribute : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 回退方法，默认是：当前方法+Fallback
        /// </summary>
        public string FallBackMethod { get; set; }

        private string ConfigKey { get; set; }

        public PolicyAttribute(string configKey = "Policy:Global")
        {
            ConfigKey = configKey;
        }

        private static readonly ConcurrentDictionary<MethodInfo, WingAsyncPolicy> Policies = new ConcurrentDictionary<MethodInfo, WingAsyncPolicy>();

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var config = context.ServiceProvider.GetRequiredService<IConfiguration>().GetSection(ConfigKey).Get<PolicyConfig>();
            FallBackMethod ??= $"{context.ServiceMethod.Name}FallBack";
            Policies.TryGetValue(context.ServiceMethod, out WingAsyncPolicy wingPolicy);
            AsyncPolicy policy;
            if (wingPolicy == null
                || wingPolicy.Policy == null
                || wingPolicy.Config != config)
            {
                policy = Polly.Policy.NoOpAsync();
                if (config.IsEnableBreaker)
                {
                    policy = policy.WrapAsync(Polly.Policy.Handle<Exception>().CircuitBreakerAsync(config.ExceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(config.MillisecondsOfBreak)));
                }

                if (config.TimeOutMilliseconds > 0)
                {
                    policy = policy.WrapAsync(Polly.Policy.TimeoutAsync(() => TimeSpan.FromMilliseconds(config.TimeOutMilliseconds), Polly.Timeout.TimeoutStrategy.Pessimistic));
                }

                if (config.MaxRetryTimes > 0)
                {
                    policy = policy.WrapAsync(Polly.Policy.Handle<Exception>().WaitAndRetryAsync(config.MaxRetryTimes, i => TimeSpan.FromMilliseconds(config.RetryIntervalMilliseconds)));
                }

                var policyFallBack = Polly.Policy
                    .Handle<Exception>()
                    .FallbackAsync(
                        async (ctx, t) =>
                    {
                        await Task.Run(() =>
                        {
                            AspectContext aspectContext = (AspectContext)ctx["aspectContext"];
                            var fallBackMethod = context.ServiceMethod.DeclaringType.GetMethod(FallBackMethod);
                            var fallBackResult = fallBackMethod.Invoke(context.Implementation, context.Parameters);
                            aspectContext.ReturnValue = fallBackResult;
                        });
                    }, async (ex, t) =>
                    {
                        await Task.Run(() =>
                        {
                            var logger = context.ServiceProvider.GetService<ILogger<PolicyAttribute>>();
                            logger.LogError(ex, "触发异常降级，方法为：{0}", $"{context.ServiceMethod.DeclaringType}.{context.ServiceMethod}.{string.Join("_", context.Parameters)}");
                        });
                    });

                policy = policyFallBack.WrapAsync(policy);
                Policies.TryAdd(context.ServiceMethod, new WingAsyncPolicy { Policy = policy, Config = config });
            }
            else
            {
                policy = wingPolicy.Policy;
            }

            Context pollyCtx = new Context
            {
                ["aspectContext"] = context
            };
            if (config.CacheMilliseconds > 0)
            {
                string cacheKey = config.CacheKey ?? $"{config.CacheKeyPrefix}WingPolicyCache_{context.ServiceMethod.DeclaringType}.{context.ServiceMethod}.{string.Join("_", context.Parameters)}";
                var cacheProvider = context.ServiceProvider.GetService<IMemoryCache>();
                var cacheValue = cacheProvider.Get(cacheKey);
                if (cacheValue != null)
                {
                    context.ReturnValue = cacheValue;
                    return;
                }

                await policy.ExecuteAsync(ctx => next(context), pollyCtx);
                cacheProvider.Set(cacheKey, context.ReturnValue, TimeSpan.FromMilliseconds(config.CacheMilliseconds));
                return;
            }

            await policy.ExecuteAsync(ctx => next(context), pollyCtx);
        }
    }
}
