using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FreeSql.Aop;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Wing.APM.Listeners;
using Wing.Persistence.Apm;
using Wing.Persistence.APM;
using Wing.ServiceProvider.Config;

namespace Wing.APM.FreeSql
{
    public class FreeSqlDiagnosticListener : IDiagnosticListener
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FreeSqlDiagnosticListener> _logger;
        private readonly ServiceData service;

        public virtual string Name => "FreeSqlDiagnosticListener";

        public Func<string, bool> DoNotDiagnostic { get; protected set; } = null;

        public FreeSqlDiagnosticListener(IHttpContextAccessor httpContextAccessor, ILogger<FreeSqlDiagnosticListener> logger)
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
                    case FreeSqlKey.CurdBefore:
                        CurdBefore(value);
                        break;
                    case FreeSqlKey.CurdAfter:
                        CurdAfter(value);
                        break;
                    case FreeSqlKey.SyncStructureBefore:
                        SyncStructureBefore(value);
                        break;
                    case FreeSqlKey.SyncStructureAfter:
                        SyncStructureAfter(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wing.APM-FreeSql监听异常");
            }
        }

        #region Curd
        private void CurdBefore(KeyValuePair<string, object> value)
        {
            var data = ListenerTracer.GetProperty<CurdBeforeEventArgs>(value, "Value");
            var id = data.Identifier.ToString();
            TracerDto tracerDto;
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                tracerDto = new TracerDto
                {
                    SqlTracer = new SqlTracer
                    {
                        Id = id,
                        Action = GetCurdType(data.CurdType),
                        BeginTime = DateTime.Now,
                        ServerIp = Tools.LocalIp,
                        ServiceName = service.Name,
                        ServiceUrl = ApmTools.GetServiceUrl(service)
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
                Action = GetCurdType(data.CurdType),
                BeginTime = DateTime.Now
            });
        }

        private void CurdAfter(KeyValuePair<string, object> value)
        {
            var data = ListenerTracer.GetProperty<CurdAfterEventArgs>(value, "Value");
            var id = data.Identifier.ToString();
            TracerDto tracerDto;
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                tracerDto = ListenerTracer.Data[id];
                if (DoNotDiagnostic != null && DoNotDiagnostic(data.Sql))
                {
                    ListenerTracer.Data.TryRemove(id, out _);
                    return;
                }

                var sqlTracer = tracerDto.SqlTracer;
                sqlTracer.Exception = data.Exception?.ToString();
                sqlTracer.EndTime = DateTime.Now;
                sqlTracer.UsedMillSeconds = data.ElapsedMilliseconds;
                sqlTracer.Sql = ApmTools.SqlFormat(data.Sql, data.DbParms);
                tracerDto.IsStop = true;
                return;
            }

            tracerDto = ListenerTracer.Data[context.Items[ApmTools.TraceId].ToString()];
            var traceDetail = tracerDto.SqlTracerDetails[id];
            if (DoNotDiagnostic != null && DoNotDiagnostic(data.Sql))
            {
                tracerDto.SqlTracerDetails.TryRemove(id, out _);
                return;
            }

            traceDetail.Exception = data.Exception?.ToString();
            traceDetail.EndTime = DateTime.Now;
            traceDetail.UsedMillSeconds = data.ElapsedMilliseconds;
            traceDetail.Sql = ApmTools.SqlFormat(data.Sql, data.DbParms);
        }
        #endregion

        #region SyncStructure
        private void SyncStructureBefore(KeyValuePair<string, object> value)
        {
            var data = ListenerTracer.GetProperty<SyncStructureBeforeEventArgs>(value, "Value");
            var id = data.Identifier.ToString();
            TracerDto tracerDto;
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                tracerDto = new TracerDto
                {
                    SqlTracer = new SqlTracer
                    {
                        Id = id,
                        Action = ApmTools.Sql_Action_SyncStructure,
                        BeginTime = DateTime.Now,
                        ServerIp = Tools.LocalIp,
                        ServiceName = service.Name,
                        ServiceUrl = ApmTools.GetServiceUrl(service)
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
                Action = ApmTools.Sql_Action_SyncStructure,
                BeginTime = DateTime.Now
            });
        }

        private void SyncStructureAfter(KeyValuePair<string, object> value)
        {
            var data = ListenerTracer.GetProperty<SyncStructureAfterEventArgs>(value, "Value");
            var id = data.Identifier.ToString();
            TracerDto tracerDto;
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                tracerDto = ListenerTracer.Data[id];
                if (DoNotDiagnostic != null && DoNotDiagnostic(data.Sql))
                {
                    ListenerTracer.Data.TryRemove(id, out _);
                    return;
                }

                var sqlTracer = tracerDto.SqlTracer;
                sqlTracer.Exception = data.Exception?.ToString();
                sqlTracer.EndTime = DateTime.Now;
                sqlTracer.UsedMillSeconds = data.ElapsedMilliseconds;
                sqlTracer.Sql = data.Sql;
                tracerDto.IsStop = true;
                return;
            }

            tracerDto = ListenerTracer.Data[context.Items[ApmTools.TraceId].ToString()];
            var traceDetail = tracerDto.SqlTracerDetails[id];
            if (DoNotDiagnostic != null && DoNotDiagnostic(data.Sql))
            {
                tracerDto.SqlTracerDetails.Remove(id, out _);
                return;
            }

            traceDetail.Exception = data.Exception?.ToString();
            traceDetail.EndTime = DateTime.Now;
            traceDetail.UsedMillSeconds = data.ElapsedMilliseconds;
            traceDetail.Sql = data.Sql;
        }
        #endregion

        private string GetCurdType(CurdType curdType)
        {
            switch (curdType)
            {
                case CurdType.Select:
                    return ApmTools.Sql_Action_Select;
                case CurdType.Delete:
                    return ApmTools.Sql_Action_Delete;
                case CurdType.Update:
                    return ApmTools.Sql_Action_Update;
                case CurdType.Insert:
                    return ApmTools.Sql_Action_Insert;
                case CurdType.InsertOrUpdate:
                    return ApmTools.Sql_Action_InsertOrUpdate;
                default:
                    return string.Empty;
            }
        }
    }
}
