using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Wing.Saga.Server
{
    public class SagaTranHostedService : BackgroundService
    {
        private static readonly object _lock = new object();
        private readonly ILogger<SagaTranHostedService> _logger;
        private readonly IConfiguration _configuration;
        private Timer _timer;
        private bool _wait = false;

        public SagaTranHostedService(ILogger<SagaTranHostedService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Saga分布式事务服务端开始启动...");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            _logger.LogInformation("Saga分布式事务服务端停止运行...");
            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var persistenceSeconds = _configuration["Apm:PersistenceSeconds"];
            var seconds = string.IsNullOrWhiteSpace(persistenceSeconds) ? 3 : Convert.ToDouble(persistenceSeconds);
            _timer = new Timer(x =>
              {
                  lock (_lock)
                  {
                      if (_wait)
                      {
                          return;
                      }

                      _wait = true;
                     
                      try
                      {
                         
                      }
                      catch (Exception ex)
                      {
                          _logger.LogError(ex, "Saga分布式事务服务端执行异常");
                      }
                      finally
                      {
                          _wait = false;
                      }
                  }
              }, null, TimeSpan.FromSeconds(seconds), TimeSpan.FromSeconds(seconds));
            return Task.CompletedTask;
        }
    }
}
