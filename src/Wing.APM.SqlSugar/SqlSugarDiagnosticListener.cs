using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SqlSugar;
using Wing.APM.FreeSql;
using Wing.APM.Listeners;
using Wing.Persistence.Apm;
using Wing.Persistence.APM;
using Wing.ServiceProvider.Config;

namespace Wing.APM.SqlSugar
{
    public class SqlSugarDiagnosticListener : IDiagnosticListener
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SqlSugarDiagnosticListener> _logger;
        private readonly ServiceData service;

        public virtual string Name => "SqlSugarDiagnosticListener";

        public SqlSugarDiagnosticListener(IHttpContextAccessor httpContextAccessor, ILogger<SqlSugarDiagnosticListener> logger)
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
                    case SqlSugarKey.OnLogExecuting:
                        OnLogExecuting(value);
                        break;
                    case SqlSugarKey.OnLogExecuted:
                        OnLogExecuted(value);
                        break;
                    case SqlSugarKey.OnError:
                        OnError(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wing.APM.SqlSugar监听异常");
            }
        }

        private void OnLogExecuting(KeyValuePair<string, object> value)
        {
            var data = (OnLogExecutingModel)value.Value;
            var id = data.Id;
            TracerDto tracerDto;
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                tracerDto = new TracerDto
                {
                    SqlTracer = new SqlTracer
                    {
                        Id = id,
                        Action = GetAction(data.ActionType),
                        BeginTime = data.BeginTime,
                        ServiceName = service.Name,
                        ServiceUrl = App.CurrentServiceUrl
                    }
                };
                ListenerTracer.Data.TryAdd(tracerDto.SqlTracer.Id, tracerDto);
                return;
            }

            tracerDto = ListenerTracer.Data[context.Items[ApmTools.TraceId].ToString()];
            tracerDto.SqlTracerDetails ??= new ConcurrentDictionary<string, SqlTracerDetail>();
            tracerDto.SqlTracerDetails.TryAdd(id, new SqlTracerDetail
            {
                Id = id,
                TraceId = tracerDto.Tracer.Id,
                Action = GetAction(data.ActionType),
                BeginTime = data.BeginTime,
                Sql = data.Sql
            });
        }

        private void OnLogExecuted(KeyValuePair<string, object> value)
        {
            var data = (OnLogExecutedModel)value.Value;
            var id = data.Id;
            TracerDto tracerDto;
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                tracerDto = ListenerTracer.Data[id];
                var sqlTracer = tracerDto.SqlTracer;
                sqlTracer.EndTime = data.EndTime;
                sqlTracer.UsedMillSeconds = Convert.ToInt64(data.ExecutionTime.TotalMilliseconds);
                tracerDto.IsStop = true;
                return;
            }

            tracerDto = ListenerTracer.Data[context.Items[ApmTools.TraceId].ToString()];
            var traceDetail = tracerDto.SqlTracerDetails[id];
            traceDetail.EndTime = data.EndTime;
            traceDetail.UsedMillSeconds = Convert.ToInt64(data.ExecutionTime.TotalMilliseconds);
        }

        private void OnError(KeyValuePair<string, object> value)
        {
            var data = (OnErrorModel)value.Value;
            var id = data.Id;
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

            tracerDto = ListenerTracer.Data[context.Items[ApmTools.TraceId].ToString()];
            var traceDetail = tracerDto.SqlTracerDetails[id];
            traceDetail.Exception = data.Exception.ToString();
        }

        private string GetAction(SugarActionType actionType)
        {
            switch (actionType)
            {
                case SugarActionType.Query:
                    return ApmTools.Sql_Action_Select;
                case SugarActionType.Delete:
                    return ApmTools.Sql_Action_Delete;
                case SugarActionType.Update:
                    return ApmTools.Sql_Action_Update;
                case SugarActionType.Insert:
                    return ApmTools.Sql_Action_Insert;
                default:
                    return string.Empty;
            }
        }
    }
}
