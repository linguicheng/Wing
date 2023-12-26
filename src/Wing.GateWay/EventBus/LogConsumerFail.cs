using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.EventBus;
using Wing.Persistence.Gateway;
using Wing.Persistence.GateWay;

namespace Wing.Gateway.EventBus
{
    [Subscribe(QueueMode.DLX)]
    public class LogConsumerFail : ISubscribe<LogAddDto>
    {
        public async Task<bool> Consume(LogAddDto logDto)
        {
            var logger = App.GetRequiredService<ILogger<LogConsumerFail>>();
            var json = App.GetRequiredService<IJson>();
            try
            {
                var logService = App.GetRequiredService<ILogService>();
                if (await logService.Any(logDto.Log.Id))
                {
                    return true;
                }

                var result = await logService.Add(logDto);
                if (result <= 0)
                {
                    logger.LogError($"数据库保存失败，请求日志：{json.Serialize(logDto)}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "发生异常，请求日志：{0}", json.Serialize(logDto));
                return false;
            }

            return true;
        }
    }
}
