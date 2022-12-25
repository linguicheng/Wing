using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.EventBus;
using Wing.Injection;
using Wing.Persistence.Saga;

namespace Wing.Saga.Server.TranUnit
{
    public class RetryCancelConsumer : ISubscribe<RetryCancelTranUnitEvent>
    {
        public Task<bool> Consume(RetryCancelTranUnitEvent eventMessage)
        {
            return Scoped.Create(async scoped =>
            {
                var logger = scoped.ServiceProvider.GetRequiredService<ILogger<RetryCancelConsumer>>();
                var json = scoped.ServiceProvider.GetRequiredService<IJson>();
                var service = scoped.ServiceProvider.GetRequiredService<ISagaTranUnitService>();
                try
                {
                    var result = await service.RetryCancel(eventMessage);
                    if (result <= 0)
                    {
                        logger.LogError($"Saga事务单元向后恢复失败，内容为：{json.Serialize(eventMessage)}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Saga事务单元向后恢复失败，内容为：{0}", json.Serialize(eventMessage));
                    return false;
                }

                return true;
            });
        }
    }
}
