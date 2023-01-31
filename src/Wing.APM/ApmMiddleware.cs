using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Wing.APM.Listeners;
using Wing.Persistence.Apm;
using Wing.Persistence.APM;

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
            var path = context.Request.Path.ToString();
            if (path == "/")
            {
                await _next(context);
                return;
            }

            if ((context.Request.ContentType?.Contains("application/grpc")).GetValueOrDefault())
            {
                await GrpcInvoke(context);
                return;
            }

            if (!path.Contains("."))
            {
                await HttpInvoke(context);
                return;
            }

            await _next(context);
        }

        private Task GrpcInvoke(HttpContext context)
        {
            return Invoke(context, ApmTools.Grpc, async tracerDto =>
             {
                 await _next(context);

                 if (context.Items.ContainsKey(ApmTools.GrpcRequest))
                 {
                     tracerDto.Tracer.RequestValue = context.Items[ApmTools.GrpcRequest].ToString();
                 }

                 if (context.Items.ContainsKey(ApmTools.GrpcResponse))
                 {
                     tracerDto.Tracer.ResponseValue = context.Items[ApmTools.GrpcResponse].ToString();
                 }
             });
        }

        private Task HttpInvoke(HttpContext context)
        {
            return Invoke(context, ApmTools.Http, async tracerDto =>
            {
                if (context.Request.Body != null)
                {
                    context.Request.EnableBuffering();
                    var reader = new StreamReader(context.Request.Body);
                    tracerDto.Tracer.RequestValue = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }

                var originalResponseStream = context.Response.Body;
                using (var ms = new MemoryStream())
                {
                    context.Response.Body = ms;
                    await _next(context);
                    ms.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(ms);
                    tracerDto.Tracer.ResponseValue = await reader.ReadToEndAsync();
                    ms.Seek(0, SeekOrigin.Begin);
                    await ms.CopyToAsync(originalResponseStream);
                    context.Response.Body = originalResponseStream;
                }
            });
        }

        private async Task Invoke(HttpContext context, string requestType, Func<TracerDto, Task> action)
        {
            var service = App.CurrentService;
            var tracerDto = new TracerDto
            {
                Tracer = new Tracer
                {
                    Id = Guid.NewGuid().ToString(),
                    ClientIp = Tools.RemoteIp,
                    ServiceName = service.Name,
                    ServiceUrl = ApmTools.GetServiceUrl(service),
                    RequestType = requestType,
                    RequestMethod = context.Request.Method,
                    RequestTime = DateTime.Now,
                    RequestUrl = context.Request.GetDisplayUrl()
                }
            };
            if (context.Request.Headers.ContainsKey(ApmTools.TraceId))
            {
                tracerDto.Tracer.ParentId = context.Request.Headers[ApmTools.TraceId].ToString();
            }

            context.Items.Add(ApmTools.TraceId, tracerDto.Tracer.Id);
            ListenerTracer.Data.Add(tracerDto);
            await action?.Invoke(tracerDto);
            tracerDto.Tracer.ResponseTime = DateTime.Now;
            tracerDto.Tracer.UsedMillSeconds = ApmTools.UsedMillSeconds(tracerDto.Tracer.RequestTime, tracerDto.Tracer.ResponseTime);
            if (context.Items.ContainsKey(ApmTools.Exception))
            {
                tracerDto.Tracer.Exception = context.Items[ApmTools.Exception].ToString();
            }

            tracerDto.Tracer.StatusCode = context.Response.StatusCode;
            tracerDto.IsStop = true;
        }
    }
}
