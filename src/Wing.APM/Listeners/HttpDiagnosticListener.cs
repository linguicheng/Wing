using System.Collections.Concurrent;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Wing.APM.Persistence;
using Wing.APM.Persistence;

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
                _logger.LogError(ex, "Wing.APM-http监听异常");
            }
        }

        private void Start(object value)
        {
            var request = ListenerTracer.GetProperty<HttpRequestMessage>(value, "Request");
            if (request == null || request.Headers.Contains(ApmTools.TraceId))
            {
                return;
            }

            var service = App.CurrentService;
            TracerDto tracerDto;
            var detailId = Guid.NewGuid().ToString();
            var context = _httpContextAccessor.HttpContext;
            var requestType = (request.Content?.Headers?.ContentType?.MediaType.Contains("application/grpc")).GetValueOrDefault() ? ApmTools.Grpc : ApmTools.Http;
            if (context == null)
            {
                tracerDto = new TracerDto
                {
                    HttpTracer = new HttpTracer
                    {
                        Id = detailId,
                        ServiceName = service.Name,
                        ServiceUrl = App.CurrentServiceUrl,
                        RequestType = requestType,
                        RequestMethod = request.Method.ToString(),
                        RequestTime = DateTime.Now,
                        RequestUrl = request.RequestUri.ToString(),
                        RequestValue = ListenerTracer.GetRequestValue(request)
                                                .ConfigureAwait(false)
                                                .GetAwaiter()
                                                .GetResult()
                    }
                };
                ListenerTracer.Data.TryAdd(detailId, tracerDto);
                request.Headers.Add(ApmTools.TraceId, tracerDto.HttpTracer.Id);
                request.Properties.Add(ApmTools.TraceId, tracerDto.HttpTracer.Id);
                return;
            }

            if (context.Items == null || !context.Items.Any())
            {
                return;
            }

            tracerDto = ListenerTracer.Data[context.Items[ApmTools.TraceId].ToString()];
            tracerDto.HttpTracerDetails ??= new ConcurrentDictionary<string, HttpTracerDetail>();
            tracerDto.HttpTracerDetails.TryAdd(detailId, new HttpTracerDetail
            {
                Id = detailId,
                TraceId = tracerDto.Tracer.Id,
                RequestType = requestType,
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
                tracerDto = ListenerTracer.Data[traceId];
                var traceDetail = tracerDto.HttpTracerDetails[request.Properties[ApmTools.TraceDetailId].ToString()];
                traceDetail.Exception = exception.ToString();
                return;
            }

            tracerDto = ListenerTracer.Data[traceId];
            tracerDto.HttpTracer.Exception = exception.ToString();
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
                tracerDto = ListenerTracer.Data[traceId];
                var traceDetail = tracerDto.HttpTracerDetails[request.Properties[ApmTools.TraceDetailId].ToString()];
                traceDetail.ResponseTime = DateTime.Now;
                traceDetail.ResponseValue = responseValue;
                traceDetail.StatusCode = statusCode;
                traceDetail.UsedMillSeconds = ApmTools.UsedMillSeconds(traceDetail.RequestTime, traceDetail.ResponseTime);
                return;
            }

            tracerDto = ListenerTracer.Data[traceId];
            tracerDto.HttpTracer.ResponseTime = DateTime.Now;
            tracerDto.HttpTracer.ResponseValue = responseValue;
            tracerDto.HttpTracer.StatusCode = statusCode;
            tracerDto.HttpTracer.UsedMillSeconds = ApmTools.UsedMillSeconds(tracerDto.HttpTracer.RequestTime, tracerDto.HttpTracer.ResponseTime);
            tracerDto.IsStop = true;
        }
    }
}
