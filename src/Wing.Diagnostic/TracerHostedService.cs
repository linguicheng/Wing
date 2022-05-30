using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wing.APM.Listeners;
using Wing.Persistence.Apm;

namespace Wing.APM
{
    public class TracerHostedService : BackgroundService
    {
        private readonly ILogger<TracerHostedService> _logger;
        private readonly ITracerService _tracerService;
        private Timer _timer;
        private bool _wait = false;

        public TracerHostedService(ILogger<TracerHostedService> logger, ITracerService tracerService)
        {
            _logger = logger;
            _tracerService = tracerService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("APM链路跟踪平台开始启动...");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            _logger.LogInformation("APM链路跟踪平台停止运行...");
            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(async x =>
              {
                  if (_wait)
                  {
                      return;
                  }

                  _wait = true;
                  var tracers = ListenerTracer.Data.Where(x => x.IsStop).ToList();
                  if (tracers == null || !tracers.Any())
                  {
                      _wait = false;
                      return;
                  }

                  try
                  {
                      foreach (var tracer in tracers)
                      {
                          await _tracerService.Add(tracer);
                          ListenerTracer.Data.Remove(tracer);
                      }
                  }
                  catch (Exception ex)
                  {
                      _logger.LogError(ex, "APM链路跟踪平台持久化异常");
                  }
                  finally
                  {
                      _wait = false;
                  }
              }, null, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
            return Task.CompletedTask;
        }
    }
}
