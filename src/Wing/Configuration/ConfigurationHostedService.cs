using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wing.ServiceProvider;

namespace Wing.Configuration
{
    public class ConfigurationHostedService : IHostedService
    {
        private static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        private readonly ILogger<ConfigurationHostedService> _logger;

        public ConfigurationHostedService(ILogger<ConfigurationHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                do
                {
                    try
                    {
                        await ServiceLocator.DiscoveryService.GetKVData(ConfigurationSubject.Notify)
                        .ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "配置中心获取配置信息异常");
                    }
                }
                while (!CancellationTokenSource.IsCancellationRequested);
            }, CancellationTokenSource.Token,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            CancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }
    }
}
