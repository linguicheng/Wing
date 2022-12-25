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
    public class UpdateStatusConsumer : ISubscribe<UpdateTranUnitStatusEvent>
    {
        public Task<bool> Consume(UpdateTranUnitStatusEvent eventMessage)
        {
            return Scoped.Create(async scoped =>
            {
                var logger = scoped.ServiceProvider.GetRequiredService<ILogger<UpdateStatusConsumer>>();
                var json = scoped.ServiceProvider.GetRequiredService<IJson>();
                var service = scoped.ServiceProvider.GetRequiredService<ISagaTranUnitService>();
                try
                {
                    var result = await service.UpdateStatus(eventMessage);
                    if (result <= 0)
                    {
                        logger.LogError($"更新Saga事务单元状态失败，内容为：{json.Serialize(eventMessage)}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "更新Saga事务单元状态失败，内容为：{0}", json.Serialize(eventMessage));
                    return false;
                }

                return true;
            });
        }
    }
}
