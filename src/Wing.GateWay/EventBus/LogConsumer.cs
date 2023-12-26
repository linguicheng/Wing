using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.EventBus;
using Wing.Persistence.Gateway;
using Wing.Persistence.GateWay;

namespace Wing.Gateway.EventBus
{
    public class LogConsumer : ISubscribe<LogAddDto>
    {
        public async Task<bool> Consume(LogAddDto logDto)
        {
            var logger = App.GetRequiredService<ILogger<LogConsumer>>();
            var json = App.GetRequiredService<IJson>();
            try
            {
                var result = await App.GetRequiredService<ILogService>().Add(logDto);
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
