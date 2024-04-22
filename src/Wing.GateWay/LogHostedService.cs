using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.Gateway.Config;
using Wing.Persistence.Gateway;
using Wing.Persistence.GateWay;

namespace Wing.Gateway
{
    public class LogHostedService : BackgroundService
    {
        private static readonly object _lock = new();
        private readonly ILogger<LogHostedService> _logger;
        private readonly ILogService _logService;
        private readonly IJson _json;
        private Timer _timer;
        private bool _wait = false;

        public LogHostedService(ILogger<LogHostedService> logger, ILogService logService, IJson json)
        {
            _logger = logger;
            _logService = logService;
            _json = json;
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
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

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var logConfig = App.GetConfig<LogConfig>("Gateway:Log:Interval");
            int interval = 10;
            if (logConfig.Interval != null)
            {
                interval = logConfig.Interval.Value;
            }

            _timer = new Timer(x =>
            {
                lock (_lock)
                {
                    if (_wait)
                    {
                        return;
                    }

                    _wait = true;
                    if (!DataProvider.Data.IsEmpty)
                    {
                        List<LogAddDto> logs = new();
                        try
                        {
                            for (var i = 0; i < DataProvider.Data.Count; i++)
                            {
                                if (DataProvider.Data.TryDequeue(out var logDto))
                                {
                                    if (logConfig != null)
                                    {
                                        if (!logConfig.IsEnabled)
                                        {
                                            DataProvider.Data.Clear();
                                            return;
                                        }

                                        var filter = logConfig.Filter;
                                        if (filter != null)
                                        {
                                            if (filter.ServiceName != null && filter.ServiceName.Count > 0)
                                            {
                                                if (filter.ServiceName.Exists(x => x == logDto.Log.ServiceName))
                                                {
                                                    continue;
                                                }
                                            }

                                            if (filter.RequestUrl != null && filter.RequestUrl.Count > 0)
                                            {
                                                if (filter.RequestUrl.Exists(x => logDto.Log.RequestUrl.Contains(x)))
                                                {
                                                    continue;
                                                }
                                            }

                                            if (filter.DownstreamUrl != null && filter.DownstreamUrl.Count > 0)
                                            {
                                                if (filter.DownstreamUrl.Exists(x => logDto.Log.DownstreamUrl.Contains(x)))
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                    }

                                    logs.Add(logDto);
                                }
                            }

                            if (logs.Count > 0)
                            {
                                _logService.BatchInsert(logs).ConfigureAwait(false).GetAwaiter().GetResult();
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "网关日志写入发生异常,请求日志：{0}", _json.Serialize(logs));
                        }
                        finally
                        {
                            logConfig = App.GetConfig<LogConfig>("Gateway:Log");
                            _wait = false;
                        }
                    }

                    _wait = false;
                }
            }, null, TimeSpan.FromSeconds(interval), TimeSpan.FromSeconds(interval));
            return Task.CompletedTask;
        }
    }
}
