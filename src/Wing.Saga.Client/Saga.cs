using System;
using Wing.EventBus;
using Wing.ServiceProvider;
using Wing.Converter;
using Wing.Persistence.Saga;

namespace Wing.Saga.Client
{
    public class Saga
    {
        /// <summary>
        /// 开始执行Saga事务，默认向前恢复策略
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static SagaProvider Start(string name, SagaOptions sagaOptions = null)
        {
            name.IsNotNull();
            SagaTran sagaTran = new SagaTran
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Status=TranStatus.Executing,
                CreatedTime = DateTime.Now,
            };
            if (sagaOptions == null)
            {
                sagaTran.Policy = TranPolicy.Forward;
            }
            else
            {
                if (sagaOptions.TranPolicy == TranPolicy.Custom && sagaOptions.CustomCount <= 0)
                {
                    throw new Exception($"选择自定义策略时，向前恢复次数必须大于0");
                }

                sagaTran.Policy = sagaOptions.TranPolicy;
                sagaTran.CustomCount = sagaOptions.CustomCount;
                sagaTran.BreakerCount = sagaOptions.BreakerCount;
                sagaTran.Description = sagaOptions.Description;
            }
            ServiceLocator.GetRequiredService<IEventBus>().Publish(sagaTran);
            return new SagaProvider(tranId);
        }
    }
}
