using Wing.Converter;
using Wing.Persistence.Saga;
using Wing.Saga.Client.Persistence;

namespace Wing.Saga.Client
{
    public class Saga
    {
        /// <summary>
        /// 开始执行Saga事务，默认向前恢复策略
        /// </summary>
        /// <param name="name">事务名称</param>
        /// <param name="sagaOptions">其他选项</param>
        /// <returns></returns>
        public static SagaProvider Start(string name, SagaOptions sagaOptions = null)
        {
            name.IsNotNull();
            var service = App.CurrentService;
            SagaTran sagaTran = new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                ServiceName = service.Name,
                ServiceType = service.Option,
                Status = TranStatus.Executing,
                CreatedTime = DateTime.Now,
                BeginTime = DateTime.Now,
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

            App.GetRequiredService<ISagaTranAppService>().Add(sagaTran, "开始Saga事务").GetAwaiter().GetResult();
            return new SagaProvider(sagaTran, 0, null);
        }
    }
}
