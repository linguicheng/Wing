using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Wing.Configuration
{
    public class ConfigurationHostedService : BackgroundService
    {
        private readonly ILogger<ConfigurationHostedService> _logger;

        public ConfigurationHostedService(ILogger<ConfigurationHostedService> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("配置中心开始启动...");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("配置中心停止运行...");
            return base.StopAsync(cancellationToken);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await App.DiscoveryService.GetKVData(ConfigurationSubject.Notify)
                        .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "配置中心获取配置信息异常");
                }
            }
        }
    }
}
