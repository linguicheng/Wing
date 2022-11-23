using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wing.Persistence.Saga;

namespace Wing.Saga.Server
{
    public class TranRetryHostedService : BackgroundService
    {
        private static readonly object _lock = new object();
        private readonly ILogger<TranRetryHostedService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISagaTranService _sagaTranService;
        private readonly ISagaTranUnitService _unitService;
        private Timer _timer;
        private bool _wait = false;

        public TranRetryHostedService(ILogger<TranRetryHostedService> logger,
            IConfiguration configuration,
            ISagaTranService sagaTranService,
            ISagaTranUnitService unitService)
        {
            _logger = logger;
            _configuration = configuration;
            _sagaTranService = sagaTranService;
            _unitService = unitService;
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
            var retrySeconds = _configuration["Saga:RetrySeconds"];
            var seconds = string.IsNullOrWhiteSpace(retrySeconds) ? 300 : Convert.ToDouble(retrySeconds);
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
                          var failedTrans = _sagaTranService.GetFailedData();
                          if (failedTrans == null || !failedTrans.Any())
                          {
                              return;
                          }

                          foreach (var tran in failedTrans)
                          {
                              var failedUnits = _unitService.GetFailedData(tran.Id);
                              if (failedUnits == null || !failedUnits.Any())
                              {
                                  continue;
                              }
                          }

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
