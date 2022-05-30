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
                var context = _httpContextAccessor.HttpContext;
                if (value.Value.GetType().GetProperty("exception").GetValue(value.Value) is Exception exception)
                {
                    if (context != null && exception != null)
                    {
                        var tracerDto = new ListenerTracer()[context.Items[ApmTag.TraceId].ToString()];
                        tracerDto.Tracer.Exception = exception.ToString();
                    }
                }
            }
        }
    }
}
