using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Wing.APM.Listeners;
using Wing.Persistence.Apm;
using Wing.Persistence.APM;
using Wing.ServiceProvider;

namespace Wing.APM
{
    public class ApmMiddleware
    {
        private readonly RequestDelegate _next;

        public ApmMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.HasValue && context.Request.Path.Value == "/")
            {
                await _next(context);
                return;
            }

            await HttpInvokeAsync(context);
        }

        private async Task HttpInvokeAsync(HttpContext context)
        {
            var service = ServiceLocator.CurrentService;
            var tracerDto = new TracerDto
            {
                Tracer = new Tracer
                {
                    Id = Guid.NewGuid().ToString(),
                    ClientIp = context.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                    ServiceName = service.Name,
                    ServiceUrl = $"{service.Scheme}://{service.Host}:{service.Port}",
                    RequestType = "http",
                    RequestMethod = context.Request.Method,
                    RequestTime = DateTime.Now,
                    RequestUrl = context.Request.GetDisplayUrl()
                }
            };
            if (context.Request.Headers.ContainsKey(ApmTag.TraceId))
            {
                tracerDto.Tracer.ParentId = context.Request.Headers[ApmTag.TraceId].ToString();
            }

            context.Items.Add(ApmTag.TraceId, tracerDto.Tracer.Id);
            if (context.Request.Body != null)
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body);
                tracerDto.Tracer.RequestValue = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            ListenerTracer.Data.Add(tracerDto);
            var originalResponseStream = context.Response.Body;
            using (var ms = new MemoryStream())
            {
                context.Response.Body = ms;
                await _next(context);
                tracerDto = new ListenerTracer()[tracerDto.Tracer.Id];
                tracerDto.Tracer.ResponseTime = DateTime.Now;
                tracerDto.Tracer.UsedMillSeconds = Convert.ToInt64((tracerDto.Tracer.ResponseTime - tracerDto.Tracer.RequestTime).TotalMilliseconds);
                if (context.Items.ContainsKey(ApmTag.Exception))
                {
                    tracerDto.Tracer.Exception = context.Items[ApmTag.Exception].ToString();
                }

                ms.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(ms);
                tracerDto.Tracer.ResponseValue = await reader.ReadToEndAsync();
                ms.Seek(0, SeekOrigin.Begin);
                await ms.CopyToAsync(originalResponseStream);
                context.Response.Body = originalResponseStream;
            }

            tracerDto.Tracer.StatusCode = context.Response.StatusCode;
            tracerDto.IsStop = true;
        }
    }
}
