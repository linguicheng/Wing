using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.Persistence.Gateway;

namespace Wing.Gateway
{
    public class LogHostedService : BackgroundService
    {
        private readonly ILogger<LogHostedService> _logger;
        private readonly ILogService _logService;
        private readonly IJson _json;

        public LogHostedService(ILogger<LogHostedService> logger, ILogService logService, IJson json)
        {
            _logger = logger;
            _logService = logService;
            _json = json;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("网关日志记录服务开始启动...");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("网关日志记录服务停止运行...");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                if (!DataProvider.Data.IsEmpty)
                {
                    if (DataProvider.Data.TryDequeue(out var logDto))
                    {
                        try
                        {
                            await _logService.Add(logDto);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "数据库保存发生异常,请求日志：{0}", _json.Serialize(logDto));
                        }
                    }
                }
            }
        }
    }
}
