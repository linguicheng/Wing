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
    public class RetryCommitConsumer : ISubscribe<RetryCommitTranEvent>
    {
        public Task<bool> Consume(RetryCommitTranEvent eventMessage)
        {
            return Scoped.Create(async scoped =>
            {
                var logger = scoped.ServiceProvider.GetRequiredService<ILogger<RetryCommitConsumer>>();
                var json = scoped.ServiceProvider.GetRequiredService<IJson>();
                var service = scoped.ServiceProvider.GetRequiredService<ISagaTranService>();
                try
                {
                    var result = await service.RetryCommit(eventMessage);
                    if (result <= 0)
                    {
                        logger.LogError($"Saga事务向前恢复失败，内容为：{json.Serialize(eventMessage)}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Saga事务向前恢复失败，内容为：{0}", json.Serialize(eventMessage));
                    return false;
                }

                return true;
            });
        }
    }
}
