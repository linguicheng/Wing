using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Wing.Injection;
using Wing.ServiceProvider;

namespace Wing.Saga
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SagaMainAttribute : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 事务超时时间，默认不超时（单位：毫秒）
        /// </summary>
        public uint TimeOut { get; set; }

        /// <summary>
        /// 回滚方法，为空时表示不回滚，定时重试该事务，直到成功为止
        /// </summary>
        public string CancelMethod { get; set; }

        public SagaMainAttribute()
        {
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            context.AdditionalData.Add("SagaMainId", Guid.NewGuid().ToString());
            await next(context);
            var value = context.ReturnValue;
            if (value is Task<bool> task)
            {
                var result = await task;
            }
            else if (value is Task<SagaResult>)
            {
            }
            else if (value is bool data)
            {
                var bb = data;
            }
            else if (value is SagaResult)
            {
            }
            else
            {
                throw new InvalidOperationException("不支持该返回值类型，请使用bool、Task<bool>、SagaResult、Task<SagaResult>作为返回值类型！");
            }

            //var declaringType = context.ServiceMethod.DeclaringType.FullName;
            //var type = GlobalInjection.GetType(declaringType);
            //var mi = type.GetMethod(context.ImplementationMethod.Name);
            //var cc = ServiceLocator.GetService(type);
            //var dd = mi.Invoke(cc, context.Parameters);
        }
    }
}
