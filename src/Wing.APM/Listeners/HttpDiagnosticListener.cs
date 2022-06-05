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
        private readonly HttpContext context;
        private readonly ILogger<HttpDiagnosticListener> _logger;
        private readonly ListenerTracer _listenerTracer;

        public string Name => "HttpHandlerDiagnosticListener";

        public HttpDiagnosticListener(IHttpContextAccessor httpContextAccessor, ILogger<HttpDiagnosticListener> logger)
        {
            context = httpContextAccessor.HttpContext;
            _logger = logger;
            _listenerTracer = new ListenerTracer();
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
                _logger.LogError(ex, "http监听异常");
            }
        }

        private void Start(object value)
        {
            var request = ListenerTracer.GetProperty<HttpRequestMessage>(value, "Request");
            if (request == null || request.Headers.Contains(ApmTools.TraceId))
            {
                return;
            }

            var service = ServiceLocator.CurrentService;
            TracerDto tracerDto;
            var detailId = Guid.NewGuid().ToString();
            if (context == null)
            {
                tracerDto = new TracerDto
                {
                    HttpTracer = new HttpTracer
                    {
                        Id = detailId,
                        ServerIp = Tools.LocalIp,
                        ServiceName = service.Name,
                        ServiceUrl = ApmTools.GetServiceUrl(service),
                        RequestType = "http",
                        RequestMethod = request.Method.ToString(),
                        RequestTime = DateTime.Now,
                        RequestUrl = request.RequestUri.ToString(),
                        RequestValue = ListenerTracer.GetRequestValue(request)
                                                .ConfigureAwait(false)
                                                .GetAwaiter()
                                                .GetResult()
                    }
                };
                ListenerTracer.Data.Add(tracerDto);
                request.Headers.Add(ApmTools.TraceId, tracerDto.HttpTracer.Id);
                request.Properties.Add(ApmTools.TraceId, tracerDto.HttpTracer.Id);
                return;
            }

            tracerDto = _listenerTracer[context.Items[ApmTools.TraceId].ToString()];
            if (tracerDto.HttpTracerDetails == null)
            {
                tracerDto.HttpTracerDetails = new List<HttpTracerDetail>();
            }

            tracerDto.HttpTracerDetails.Add(new HttpTracerDetail
            {
                Id = detailId,
                TraceId = tracerDto.Tracer.Id,
                RequestType = "http",
                RequestMethod = request.Method.ToString(),
                RequestTime = DateTime.Now,
                RequestUrl = request.RequestUri.ToString(),
                RequestValue = ListenerTracer.GetRequestValue(request)
                                            .ConfigureAwait(false)
                                            .GetAwaiter()
                                            .GetResult()
            });
            request.Headers.Add(ApmTools.TraceId, tracerDto.Tracer.Id);
            request.Properties.Add(ApmTools.TraceDetailId, detailId);
            request.Properties.Add(ApmTools.TraceId, tracerDto.Tracer.Id);
        }

        private void Exception(object value)
        {
            var exception = ListenerTracer.GetProperty<Exception>(value, "Exception");
            var request = ListenerTracer.GetProperty<HttpRequestMessage>(value, "Request");
            if (request == null || !request.Properties.ContainsKey(ApmTools.TraceId))
            {
                return;
            }

            var traceId = request.Properties[ApmTools.TraceId].ToString();
            TracerDto tracerDto;
            if (request.Properties.ContainsKey(ApmTools.TraceDetailId))
            {
                tracerDto = _listenerTracer[traceId];
                var traceDetail = tracerDto.HttpTracerDetails.Where(x => x.Id == request.Properties[ApmTools.TraceDetailId].ToString()).Single();
                traceDetail.Exception = exception.ToString();
            }
            else
            {
                tracerDto = ListenerTracer.HttpTracer(traceId);
                tracerDto.HttpTracer.Exception = exception.ToString();
            }
        }

        private void Stop(object value)
        {
            var request = ListenerTracer.GetProperty<HttpRequestMessage>(value, "Request");
            if (request == null || !request.Properties.ContainsKey(ApmTools.TraceId))
            {
                return;
            }

            var response = ListenerTracer.GetProperty<HttpResponseMessage>(value, "Response");
            TracerDto tracerDto;
            var traceId = request.Properties[ApmTools.TraceId].ToString();
            var responseValue = string.Empty;
            int? statusCode = null;
            if (response != null)
            {
                responseValue = ListenerTracer.GetResponseValue(response)
                                                    .ConfigureAwait(false)
                                                    .GetAwaiter()
                                                    .GetResult();
                statusCode = (int)response.StatusCode;
            }

            if (request.Properties.ContainsKey(ApmTools.TraceDetailId))
            {
                tracerDto = _listenerTracer[traceId];
                var traceDetail = tracerDto.HttpTracerDetails.Where(x => x.Id == request.Properties[ApmTools.TraceDetailId].ToString()).Single();
                traceDetail.ResponseTime = DateTime.Now;
                traceDetail.ResponseValue = responseValue;
                traceDetail.StatusCode = statusCode;
                traceDetail.UsedMillSeconds = ApmTools.UsedMillSeconds(traceDetail.RequestTime, traceDetail.ResponseTime);
            }
            else
            {
                tracerDto = ListenerTracer.HttpTracer(traceId);
                tracerDto.HttpTracer.ResponseTime = DateTime.Now;
                tracerDto.HttpTracer.ResponseValue = responseValue;
                tracerDto.HttpTracer.StatusCode = statusCode;
                tracerDto.HttpTracer.UsedMillSeconds = ApmTools.UsedMillSeconds(tracerDto.HttpTracer.RequestTime, tracerDto.HttpTracer.ResponseTime);
                tracerDto.IsStop = true;
            }
        }
    }
}
