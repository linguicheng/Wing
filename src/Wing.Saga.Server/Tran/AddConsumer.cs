using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.EventBus;
using Wing.Injection;
using Wing.Persistence.Saga;

namespace Wing.Saga.Server.Tran
{
    public class AddConsumer : ISubscribe<SagaTran>
    {
        public Task<bool> Consume(SagaTran eventMessage)
        {
            return Scoped.Create(async scoped =>
            {
                var logger = scoped.ServiceProvider.GetRequiredService<ILogger<AddConsumer>>();
                var json = scoped.ServiceProvider.GetRequiredService<IJson>();
                var service = scoped.ServiceProvider.GetRequiredService<ISagaTranService>();
                try
                {
                    var result = await service.Add(eventMessage);
                    if (result <= 0)
                    {
                        logger.LogError($"保存Saga事务失败，内容为：{json.Serialize(eventMessage)}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "保存Saga事务失败，内容为：{0}", json.Serialize(eventMessage));
                    return false;
                }

                return true;
            });
        }
    }
}
