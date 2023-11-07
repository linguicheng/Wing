using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.EventBus;
using Wing.Persistence.Gateway;

namespace Wing.Gateway.EventBus
{
    [Subscribe(QueueMode.DLX)]
    public class LogConsumerFail : ISubscribe<Log>
    {
        public async Task<bool> Consume(Log log)
        {
            var logger = App.GetRequiredService<ILogger<LogConsumerFail>>();
            var json = App.GetRequiredService<IJson>();
            try
            {
                var logService = App.GetRequiredService<ILogService>();
                if (await logService.Any(log.Id))
                {
                    return true;
                }

                var result = await logService.Add(log);
                if (result <= 0)
                {
                    logger.LogError($"数据库保存失败，请求日志：{json.Serialize(log)}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "发生异常，请求日志：{0}", json.Serialize(log));
                return false;
            }

            return true;
        }
    }
}
