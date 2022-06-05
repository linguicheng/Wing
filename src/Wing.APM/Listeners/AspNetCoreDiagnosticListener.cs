using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Wing.APM.Listeners
{
    public class AspNetCoreDiagnosticListener : IDiagnosticListener
    {
        private readonly HttpContext context;

        public string Name => "Microsoft.AspNetCore";

        public AspNetCoreDiagnosticListener(IHttpContextAccessor httpContextAccessor)
        {
            context = httpContextAccessor.HttpContext;
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
                    if (context != null && exception != null)
                    {
                        var tracerDto = new ListenerTracer()[context.Items[ApmTools.TraceId].ToString()];
                        tracerDto.Tracer.Exception = exception.ToString();
                    }
                }
            }
        }
    }
}
