using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Wing.APM.Listeners
{
    public class AspNetCoreDiagnosticListener : IDiagnosticListener
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string Name => "Microsoft.AspNetCore";

        public AspNetCoreDiagnosticListener(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            if (value.Key == "Microsoft.AspNetCore.Diagnostics.UnhandledException" || value.Key == "Microsoft.AspNetCore.Hosting.UnhandledException")
            {
                if (value.Value.GetType().GetProperty("exception").GetValue(value.Value) is Exception exception)
                {
                    var context = _httpContextAccessor.HttpContext;
                    if (context != null && exception != null)
                    {
                        var tracerDto = ListenerTracer.Data[context.Items[ApmTools.TraceId].ToString()];
                        if (tracerDto != null && tracerDto.Tracer != null)
                        {
                            tracerDto.Tracer.Exception = exception.ToString();
                        }
                    }
                }
            }
        }
    }
}
