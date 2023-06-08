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

        public PolicyAttribute(string configKey = "Policy:Global", string fallBackMethod = null)
        {
            ConfigKey = configKey;
            FallBackMethod = fallBackMethod;
        }

        private static readonly ConcurrentDictionary<MethodInfo, WingPolicy> Policies = new ConcurrentDictionary<MethodInfo, WingPolicy>();

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var config = context.ServiceProvider.GetRequiredService<IConfiguration>().GetSection(ConfigKey).Get<PolicyConfig>();
            FallBackMethod = string.IsNullOrWhiteSpace(FallBackMethod) ? $"{context.ServiceMethod.Name}Fallback" : FallBackMethod;
            Policies.TryGetValue(context.ServiceMethod, out WingPolicy wingPolicy);
            Policy<Task> policy;
            if (wingPolicy == null || wingPolicy.Config != config)
            {
                policy = Polly.Policy.NoOp<Task>();
                if (config.IsEnableBreaker)
                {
                    policy = policy.Wrap(Polly.Policy.Handle<Exception>().CircuitBreaker(config.ExceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(config.MillisecondsOfBreak)));
                }

                if (config.TimeOutMilliseconds > 0)
                {
                    policy = policy.Wrap(Polly.Policy.Timeout(() => TimeSpan.FromMilliseconds(config.TimeOutMilliseconds), Polly.Timeout.TimeoutStrategy.Pessimistic));
                }

                if (config.MaxRetryTimes > 0)
                {
                    policy = policy.Wrap(Polly.Policy.Handle<Exception>().WaitAndRetry(config.MaxRetryTimes, i => TimeSpan.FromMilliseconds(config.RetryIntervalMilliseconds)));
                }

                var logger = context.ServiceProvider.GetService<ILogger<PolicyAttribute>>();
                var policyFallBack = Policy<Task>
                    .Handle<Exception>()
                    .Fallback(
                    ctx =>
                    {
                        AspectContext aspectContext = (AspectContext)ctx["aspectContext"];
                        var fallBackMethod = context.ServiceMethod.DeclaringType.GetMethod(FallBackMethod);
                        if (fallBackMethod == null)
                        {
                            throw new Exception($"找不到异常降级方法【{FallBackMethod}】");
                        }

                        var fallBackResult = fallBackMethod.Invoke(context.Implementation, context.Parameters);
                        aspectContext.ReturnValue = fallBackResult;
                        return Task.CompletedTask;
                    }, (dr, ctx) =>
                    {
                        logger.LogError(dr.Exception, "触发异常降级，方法为：{0}", $"{context.ServiceMethod.DeclaringType}.{context.ServiceMethod}.{string.Join("_", context.Parameters)}");
                    });

                policy = policyFallBack.Wrap(policy);
                Policies.AddOrUpdate(context.ServiceMethod,
                    new WingPolicy { Policy = policy, Config = config },
                    (key, value) => new WingPolicy { Policy = policy, Config = config });
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
                string cacheKey = string.IsNullOrWhiteSpace(config.CacheKey) ? $"{config.CacheKeyPrefix}WingPolicyCache_{context.ServiceMethod.DeclaringType}.{context.ServiceMethod}.{string.Join("_", context.Parameters)}" : config.CacheKey;
                var cacheProvider = context.ServiceProvider.GetService<IMemoryCache>();
                var cacheValue = cacheProvider.Get(cacheKey);
                if (cacheValue != null)
                {
                    context.ReturnValue = cacheValue;
                    return;
                }

                await policy.Execute(ctx => next(context), pollyCtx);
                cacheProvider.Set(cacheKey, context.ReturnValue, TimeSpan.FromMilliseconds(config.CacheMilliseconds));
                return;
            }

            await policy.Execute(ctx => next(context), pollyCtx);
        }
    }
}
