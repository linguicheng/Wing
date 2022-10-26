using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Wing.Converter;
using Wing.EventBus;
using Wing.Injection;
using Wing.Persistence.Saga;

namespace Wing.Saga.Server
{
    public class UpdateTranUnitStatusConsumer : ISubscribe<UpdateTranUnitStatusDto>
    {
        public Task<bool> Consume(UpdateTranUnitStatusDto eventMessage)
        {
            return Scoped.Create(async scoped =>
            {
                var logger = scoped.ServiceProvider.GetRequiredService<ILogger<UpdateTranUnitStatusConsumer>>();
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
