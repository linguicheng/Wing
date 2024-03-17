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
        private Timer _timer;
        private bool _wait = false;

        public TracerHostedService(ILogger<TracerHostedService> logger,
            ITracerService tracerService)
        {
            _logger = logger;
            _tracerService = tracerService;
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("APM链路跟踪平台开始启动...");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("APM链路跟踪平台停止运行...");
            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var persistenceSeconds = App.Configuration["Apm:PersistenceSeconds"];
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
                    var tracers = ListenerTracer.Data.Where(x => !x.Value.IsStop && (DateTime.Now - x.Value.BeginTime).TotalHours > 12).ToList();
                    if (tracers != null && tracers.Any())
                    {
                        ListenerTracer.Remove(tracers);
                    }

                    tracers = ListenerTracer.Data.Where(x => x.Value.IsStop).ToList();
                    if (tracers == null || !tracers.Any())
                    {
                        _wait = false;
                        return;
                    }

                    // Http请求
                    var toListenerUrls = App.GetConfig<List<string>>("Apm:ToListenerUrl");
                    var doNotListenerUrls = App.GetConfig<List<string>>("Apm:DoNotListenerUrl");

                    // 执行Sql
                    var toListenerSqls = App.GetConfig<List<string>>("Apm:ToListenerSql");
                    var doNotListenerSqls = App.GetConfig<List<string>>("Apm:DoNotListenerSql");
                    try
                    {
                        foreach (var item in tracers)
                        {
                            var tracer = item.Value;
                            if (tracer.Tracer != null)
                            {
                                if (toListenerUrls != null && toListenerUrls.Count > 0)
                                {
                                    foreach (var url in toListenerUrls)
                                    {
                                        if (tracer.Tracer != null && tracer.Tracer.RequestUrl.Contains(url))
                                        {
                                            _tracerService.Add(tracer).ConfigureAwait(false).GetAwaiter().GetResult();
                                            break;
                                        }
                                    }

                                    continue;
                                }

                                if (doNotListenerUrls != null && doNotListenerUrls.Count > 0)
                                {
                                    var isAdd = true;
                                    foreach (var url in doNotListenerUrls)
                                    {
                                        if (tracer.Tracer.RequestUrl.Contains(url))
                                        {
                                            isAdd = false;
                                            break;
                                        }
                                    }

                                    if (isAdd)
                                    {
                                        _tracerService.Add(tracer).ConfigureAwait(false).GetAwaiter().GetResult();
                                    }

                                    continue;
                                }

                                _tracerService.Add(tracer).ConfigureAwait(false).GetAwaiter().GetResult();
                                continue;
                            }

                            if (tracer.HttpTracer != null)
                            {
                                if (toListenerUrls != null && toListenerUrls.Count > 0)
                                {
                                    foreach (var url in toListenerUrls)
                                    {
                                        if (tracer.HttpTracer != null && tracer.HttpTracer.RequestUrl.Contains(url))
                                        {
                                            _tracerService.Add(tracer).ConfigureAwait(false).GetAwaiter().GetResult();
                                            break;
                                        }
                                    }

                                    continue;
                                }

                                if (doNotListenerUrls != null && doNotListenerUrls.Count > 0)
                                {
                                    var isAdd = true;
                                    foreach (var url in doNotListenerUrls)
                                    {
                                        if (tracer.HttpTracer.RequestUrl.Contains(url))
                                        {
                                            isAdd = false;
                                            break;
                                        }
                                    }

                                    if (isAdd)
                                    {
                                        _tracerService.Add(tracer).ConfigureAwait(false).GetAwaiter().GetResult();
                                    }

                                    continue;
                                }

                                _tracerService.Add(tracer).ConfigureAwait(false).GetAwaiter().GetResult();
                                continue;
                            }

                            if (tracer.SqlTracer != null)
                            {
                                if (string.IsNullOrWhiteSpace(tracer.SqlTracer.Sql) && tracer.SqlTracer.Action == ApmTools.Sql_Action_SyncStructure)
                                {
                                    continue;
                                }

                                if (toListenerSqls != null && toListenerSqls.Count > 0)
                                {
                                    foreach (var sql in toListenerSqls)
                                    {
                                        if (tracer.SqlTracer != null && tracer.SqlTracer.Sql != null && tracer.SqlTracer.Sql.Contains(sql))
                                        {
                                            _tracerService.Add(tracer).ConfigureAwait(false).GetAwaiter().GetResult();
                                            break;
                                        }
                                    }

                                    continue;
                                }

                                if (doNotListenerSqls != null && doNotListenerSqls.Count > 0)
                                {
                                    var isAdd = true;
                                    foreach (var sql in doNotListenerSqls)
                                    {
                                        if (tracer.SqlTracer.Sql != null && tracer.SqlTracer.Sql.Contains(sql))
                                        {
                                            isAdd = false;
                                        }
                                    }

                                    if (isAdd)
                                    {
                                        _tracerService.Add(tracer).ConfigureAwait(false).GetAwaiter().GetResult();
                                    }

                                    continue;
                                }

                                _tracerService.Add(tracer).ConfigureAwait(false).GetAwaiter().GetResult();
                            }
                        }

                        ListenerTracer.Remove(tracers);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Wing.APM链路跟踪平台持久化异常");
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
