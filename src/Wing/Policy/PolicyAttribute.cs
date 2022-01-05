using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace Wing.Policy
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PolicyAttribute : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 是否启用熔断
        /// </summary>
        public bool IsEnableBreaker { get; set; } = false;
        /// <summary>
        /// 熔断前出现允许错误几次
        /// </summary>
        public int ExceptionsAllowedBeforeBreaking { get; set; } = 3;
        /// <summary>
        /// 执行超过多少毫秒则认为超时（0表示不检测超时）
        /// </summary>
        public int TimeOutMilliseconds { get; set; } = 0;
        /// <summary>
        /// 熔断多长时间（毫秒）
        /// </summary>
        public int MillisecondsOfBreak { get; set; } = 1000;
        /// <summary>
        /// 最多重试几次，如果为0则不重试
        /// </summary>
        public int MaxRetryTimes { get; set; } = 0;
        /// <summary>
        /// 重试间隔的毫秒数
        /// </summary>
        public int RetryIntervalMilliseconds { get; set; } = 100;
        /// <summary>
        /// 缓存多少毫秒（0表示不缓存），用“类名+方法名+所有参数ToString拼接”做缓存Key
        /// </summary>
        public int CacheMilliseconds { get; set; } = 0;
        /// <summary>
        /// 默认是WingCommandCache_Key_{命名空间+类名+方法声明+实参}
        /// </summary>
        public string CacheKey { get; set; }

        public PolicyAttribute(string fallBackMethod)
        {
            FallBackMethod = fallBackMethod;
        }

        public string FallBackMethod { get; set; }

        private static readonly ConcurrentDictionary<MethodInfo, AsyncPolicy> Policies = new ConcurrentDictionary<MethodInfo, AsyncPolicy>();

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            Policies.TryGetValue(context.ServiceMethod, out AsyncPolicy policy);
            if (policy == null)
            {
                policy = Polly.Policy.NoOpAsync();
                if (IsEnableBreaker)
                {
                    policy = policy.WrapAsync(Polly.Policy.Handle<Exception>().CircuitBreakerAsync(ExceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(MillisecondsOfBreak)));
                }

                if (TimeOutMilliseconds > 0)
                {
                    policy = policy.WrapAsync(Polly.Policy.TimeoutAsync(() => TimeSpan.FromMilliseconds(TimeOutMilliseconds), Polly.Timeout.TimeoutStrategy.Pessimistic));
                }

                if (MaxRetryTimes > 0)
                {
                    policy = policy.WrapAsync(Polly.Policy.Handle<Exception>().WaitAndRetryAsync(MaxRetryTimes, i => TimeSpan.FromMilliseconds(RetryIntervalMilliseconds)));
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
                Policies.TryAdd(context.ServiceMethod, policy);
            }

            Context pollyCtx = new Context
            {
                ["aspectContext"] = context
            };
            if (CacheMilliseconds > 0)
            {
                string cacheKey = CacheKey ?? $"WingPolicyCache_Key_{context.ServiceMethod.DeclaringType}.{context.ServiceMethod}.{string.Join("_", context.Parameters)}";
                var cacheProvider = context.ServiceProvider.GetService<IMemoryCache>();
                var cacheValue = cacheProvider.Get(cacheKey);
                if (cacheValue != null)
                {
                    context.ReturnValue = cacheValue;
                    return;
                }

                await policy.ExecuteAsync(ctx => next(context), pollyCtx);
                cacheProvider.Set(cacheKey, context.ReturnValue, TimeSpan.FromMilliseconds(CacheMilliseconds));
                return;
            }

            await policy.ExecuteAsync(ctx => next(context), pollyCtx);
        }
    }
}
