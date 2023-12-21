using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wing.Injection;
using Wing.Persistence.Saga;

namespace Wing.Saga.Server
{
    public class TranReportHostedService : BackgroundService
    {
        private static readonly object _lock = new object();
        private readonly ILogger<TranReportHostedService> _logger;
        private Timer _timer;
        private bool _wait = false;

        public TranReportHostedService(ILogger<TranReportHostedService> logger)
        {
            _logger = logger;
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Saga报表服务开始运行...");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Saga报表服务停止运行...");
            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var period = new TimeSpan(24, 0, 0);
            var dueTime = Tools.DueTime(App.Configuration["Saga:ReportUpdateTime"], period);
            _timer = new Timer(x =>
              {
                  lock (_lock)
                  {
                      if (_wait)
                      {
                          return;
                      }

                      _wait = true;
                      Scoped.Create(scoped =>
                      {
                          try
                          {
                              var result = scoped.ServiceProvider
                              .GetRequiredService<ISagaTranService>()
                              .AddStatusCount();
                              _logger.LogInformation($"Saga报表更新总数：{result}");
                          }
                          catch (Exception ex)
                          {
                              _logger.LogError(ex, "Saga报表服务执行异常");
                          }
                          finally
                          {
                              _wait = false;
                          }
                      });
                  }
              }, null, dueTime, period);
            return Task.CompletedTask;
        }
    }
}
