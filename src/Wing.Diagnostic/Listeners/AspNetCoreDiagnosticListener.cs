using System;
using System.Collections.Generic;
using System.Text;

namespace Wing.APM.Listeners
{
    public class AspNetCoreDiagnosticListener : IDiagnosticListener
    {
        public string Name => "Microsoft.AspNetCore";

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
            }
        }
    }
}
