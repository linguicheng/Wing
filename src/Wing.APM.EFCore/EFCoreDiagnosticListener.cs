using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Wing.APM.Listeners;
using Wing.APM.Persistence;
using Wing.APM.Persistence;
using Wing.ServiceProvider.Config;

namespace Wing.APM.EFCore
{
    public class EFCoreDiagnosticListener : IDiagnosticListener
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EFCoreDiagnosticListener> _logger;
        private readonly ServiceData service;

        public virtual string Name => DbLoggerCategory.Name;

        public EFCoreDiagnosticListener(IHttpContextAccessor httpContextAccessor, ILogger<EFCoreDiagnosticListener> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            service = App.CurrentService;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            try
            {
                switch (value.Key)
                {
                    case EFCoreKey.CommandExecuting:
                        CommandExecuting(value);
                        break;
                    case EFCoreKey.CommandExecuted:
                        CommandExecuted(value);
                        break;
                    case EFCoreKey.CommandError:
                        CommandError(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wing.APM.EFCore监听异常");
            }
        }

        private void CommandExecuting(KeyValuePair<string, object> value)
        {
            var data = (CommandEventData)value.Value;
            var id = data.CommandId.ToString();
            TracerDto tracerDto;
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                tracerDto = new TracerDto
                {
                    SqlTracer = new SqlTracer
                    {
                        Id = id,
                        Sql = ApmTools.SqlFormat(data.Command.CommandText, data.Command.Parameters),
                        Action = GetAction(data.Command.CommandText),
                        BeginTime = data.StartTime.LocalDateTime,
                        ServiceName = service.Name,
                        ServiceUrl = App.CurrentServiceUrl
                    }
                };
                ListenerTracer.Data.TryAdd(tracerDto.SqlTracer.Id, tracerDto);
                return;
            }

            if (!context.Items.ContainsKey(ApmTools.TraceId))
            {
                return;
            }

            tracerDto = ListenerTracer.Data[context.Items[ApmTools.TraceId].ToString()];
            tracerDto.SqlTracerDetails ??= new ConcurrentDictionary<string, SqlTracerDetail>();
            tracerDto.SqlTracerDetails.TryAdd(id, new SqlTracerDetail
            {
                Id = id,
                TraceId = tracerDto.Tracer.Id,
                Action = GetAction(data.Command.CommandText),
                BeginTime = data.StartTime.LocalDateTime,
                Sql = ApmTools.SqlFormat(data.Command.CommandText, data.Command.Parameters)
            });
        }

        private void CommandExecuted(KeyValuePair<string, object> value)
        {
            var data = (CommandExecutedEventData)value.Value;
            var id = data.CommandId.ToString();
            TracerDto tracerDto;
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                tracerDto = ListenerTracer.Data[id];
                var sqlTracer = tracerDto.SqlTracer;
                sqlTracer.EndTime = DateTime.Now;
                sqlTracer.UsedMillSeconds = Convert.ToInt64(data.Duration.TotalMilliseconds);
                tracerDto.IsStop = true;
                return;
            }

            if (!context.Items.ContainsKey(ApmTools.TraceId))
            {
                return;
            }

            tracerDto = ListenerTracer.Data[context.Items[ApmTools.TraceId].ToString()];
            var traceDetail = tracerDto.SqlTracerDetails[id];
            traceDetail.EndTime = DateTime.Now;
            traceDetail.UsedMillSeconds = Convert.ToInt64(data.Duration.TotalMilliseconds);
        }

        private void CommandError(KeyValuePair<string, object> value)
        {
            var data = (CommandErrorEventData)value.Value;
            var id = data.CommandId.ToString();
            TracerDto tracerDto;
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                tracerDto = ListenerTracer.Data[id];
                var sqlTracer = tracerDto.SqlTracer;
                sqlTracer.Exception = data.Exception.ToString();
                tracerDto.IsStop = true;
                return;
            }

            if (!context.Items.ContainsKey(ApmTools.TraceId))
            {
                return;
            }

            tracerDto = ListenerTracer.Data[context.Items[ApmTools.TraceId].ToString()];
            var traceDetail = tracerDto.SqlTracerDetails[id];
            traceDetail.Exception = data.Exception.ToString();
        }

        private string GetAction(string sql)
        {
            if (sql.Contains("INSERT INTO"))
            {
                return ApmTools.Sql_Action_Insert;
            }

            if (sql.Contains("UPDATE"))
            {
                return ApmTools.Sql_Action_Update;
            }

            if (sql.Contains("DELETE"))
            {
                return ApmTools.Sql_Action_Delete;
            }

            if (sql.Contains("SELECT"))
            {
                return ApmTools.Sql_Action_Select;
            }

            return string.Empty;
        }
    }
}
