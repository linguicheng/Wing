using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Wing.Persistence.Apm;
using Wing.Persistence.APM;
using Wing.ServiceProvider;

namespace Wing.APM.Listeners
{
    public class HttpDiagnosticListener : IDiagnosticListener
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<HttpDiagnosticListener> _logger;

        public string Name => "HttpHandlerDiagnosticListener";

        public HttpDiagnosticListener(IHttpContextAccessor httpContextAccessor, ILogger<HttpDiagnosticListener> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
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
                    case "System.Net.Http.HttpRequestOut.Start":
                        Start(value.Value);
                        break;
                    case "System.Net.Http.Exception":
                        Exception(value.Value);
                        break;
                    case "System.Net.Http.HttpRequestOut.Stop":
                        Stop(value.Value);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("http监听异常：", ex);
            }
        }

        private void Start(object value)
        {
            var request = ListenerHelper.GetProperty<HttpRequestMessage>(value, "Request");
            if (request == null || request.Headers.Contains(ApmTag.TraceId))
            {
                return;
            }

            var service = ServiceLocator.CurrentService;
            var context = _httpContextAccessor.HttpContext;
            TracerDto tracerDto;
            if (context == null)
            {
                tracerDto = new TracerDto
                {
                    Tracer = new Tracer
                    {
                        Id = Guid.NewGuid().ToString(),
                        ClientIp = Tools.LocalIp,
                        ServiceName = service.Name,
                        ServiceUrl = $"{service.Scheme}://{service.Host}:{service.Port}",
                        RequestType = "http",
                        RequestMethod = request.Method.ToString(),
                        RequestTime = DateTime.Now,
                        RequestUrl = request.RequestUri.ToString(),
                        RequestValue = ListenerHelper.GetRequestValue(request)
                                                .ConfigureAwait(false)
                                                .GetAwaiter()
                                                .GetResult()
                    }
                };
                ListenerHelper.TracerData.TryAdd(tracerDto.Tracer.Id, tracerDto);
            }
            else
            {
                tracerDto = ListenerHelper.TracerData[context.Items[ApmTag.TraceId].ToString()];
                if (tracerDto.HttpTracerDetails == null)
                {
                    tracerDto.HttpTracerDetails = new List<HttpTracerDetail>();
                }

                var detailId = Guid.NewGuid().ToString();
                tracerDto.HttpTracerDetails.Add(new HttpTracerDetail
                {
                    Id = detailId,
                    TraceId = tracerDto.Tracer.Id,
                    RequestType = "http",
                    RequestMethod = request.Method.ToString(),
                    RequestTime = DateTime.Now,
                    RequestUrl = request.RequestUri.ToString(),
                    RequestValue = ListenerHelper.GetRequestValue(request)
                                                .ConfigureAwait(false)
                                                .GetAwaiter()
                                                .GetResult()
                });
                request.Headers.Add(ApmTag.TraceId, tracerDto.Tracer.Id);
                request.Properties.Add(ApmTag.TraceDetailId, detailId);
            }

            request.Properties.Add(ApmTag.TraceId, tracerDto.Tracer.Id);
        }

        private void Exception(object value)
        {
            var exception = ListenerHelper.GetProperty<Exception>(value, "Exception");
            var request = ListenerHelper.GetProperty<HttpRequestMessage>(value, "Request");
            if (request == null || !request.Properties.ContainsKey(ApmTag.TraceId))
            {
                return;
            }

            var traceId = request.Properties[ApmTag.TraceId].ToString();
            var tracerDto = ListenerHelper.TracerData[traceId];
            if (request.Properties.ContainsKey(ApmTag.TraceDetailId))
            {
                var traceDetail = tracerDto.HttpTracerDetails.Where(x => x.Id == request.Properties[ApmTag.TraceDetailId].ToString()).Single();
                traceDetail.Exception = exception.ToString();
            }
            else
            {
                tracerDto.Tracer.Exception = exception.ToString();
            }
        }

        private void Stop(object value)
        {
            var request = ListenerHelper.GetProperty<HttpRequestMessage>(value, "Request");
            if (request == null || !request.Properties.ContainsKey(ApmTag.TraceId))
            {
                return;
            }

            var response = ListenerHelper.GetProperty<HttpResponseMessage>(value, "Response");
            TracerDto tracerDto;
            var traceId = request.Properties[ApmTag.TraceId].ToString();
            tracerDto = ListenerHelper.TracerData[traceId];
            var responseValue = string.Empty;
            int? statusCode = null;
            if (response != null)
            {
                responseValue = ListenerHelper.GetResponseValue(response)
                                                    .ConfigureAwait(false)
                                                    .GetAwaiter()
                                                    .GetResult();
                statusCode = (int)response.StatusCode;
            }

            if (request.Properties.ContainsKey(ApmTag.TraceDetailId))
            {
                var traceDetail = tracerDto.HttpTracerDetails.Where(x => x.Id == request.Properties[ApmTag.TraceDetailId].ToString()).Single();
                traceDetail.ResponseTime = DateTime.Now;
                traceDetail.ResponseValue = responseValue;
                traceDetail.StatusCode = statusCode;
                traceDetail.UsedMillSeconds = Convert.ToInt64((traceDetail.ResponseTime - traceDetail.RequestTime).TotalMilliseconds);
            }
            else
            {
                tracerDto.Tracer.ResponseTime = DateTime.Now;
                tracerDto.Tracer.ResponseValue = responseValue;
                tracerDto.Tracer.StatusCode = statusCode;
                tracerDto.Tracer.UsedMillSeconds = Convert.ToInt64((tracerDto.Tracer.ResponseTime - tracerDto.Tracer.RequestTime).TotalMilliseconds);
            }

            tracerDto.IsStop = true;
        }
    }
}
