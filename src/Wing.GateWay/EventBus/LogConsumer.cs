using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.EventBus;
using Wing.Persistence.Gateway;

namespace Wing.Gateway.EventBus
{
    public class LogConsumer : ISubscribe<Log>
    {
        public async Task<bool> Consume(Log log)
        {
            var logger = App.GetRequiredService<ILogger<LogConsumer>>();
            var json = App.GetRequiredService<IJson>();
            try
            {
                var result = await App.GetRequiredService<ILogService>().Add(log);
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
