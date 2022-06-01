using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wing.APM.Listeners;
using Wing.Persistence.Apm;

namespace Wing.APM
{
    public class TracerHostedService : BackgroundService
    {
        private static readonly object _lock = new object();
        private readonly ILogger<TracerHostedService> _logger;
        private readonly ITracerService _tracerService;
        private readonly IConfiguration _configuration;
        private Timer _timer;
        private bool _wait = false;

        public TracerHostedService(ILogger<TracerHostedService> logger,
            ITracerService tracerService,
            IConfiguration configuration)
        {
            _logger = logger;
            _tracerService = tracerService;
            _configuration = configuration;
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
            _timer = new Timer(x =>
              {
                  lock (_lock)
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

                      var toListenerUrlStr = _configuration["Apm:ToListenerUrl"];
                      var toListenerUrls = string.IsNullOrWhiteSpace(toListenerUrlStr) ? null : toListenerUrlStr.Split(',');
                      var doNotListenerUrlStr = _configuration["Apm:DoNotListenerUrl"];
                      var doNotListenerUrls = string.IsNullOrWhiteSpace(doNotListenerUrlStr) ? null : doNotListenerUrlStr.Split(',');
                      try
                      {
                          foreach (var tracer in tracers)
                          {
                              if (toListenerUrls != null && toListenerUrls.Length > 0)
                              {
                                  foreach (var url in toListenerUrls)
                                  {
                                      if (tracer.Tracer.RequestUrl.Contains(url))
                                      {
                                          _tracerService.Add(tracer).ConfigureAwait(false).GetAwaiter().GetResult();
                                      }
                                  }
                              }
                              else if (doNotListenerUrls != null && doNotListenerUrls.Length > 0)
                              {
                                  foreach (var url in doNotListenerUrls)
                                  {
                                      if (!tracer.Tracer.RequestUrl.Contains(url))
                                      {
                                          _tracerService.Add(tracer).ConfigureAwait(false).GetAwaiter().GetResult();
                                      }
                                  }
                              }
                              else
                              {
                                  _tracerService.Add(tracer).ConfigureAwait(false).GetAwaiter().GetResult();
                              }

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
                  }
              }, null, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
            return Task.CompletedTask;
        }
    }
}
