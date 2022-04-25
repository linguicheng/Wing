using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Wing.APM
{
    internal class WingStartupFilter
    {
        public Action<IApplicationBuilder> Configure()
        {
            return app =>
            {
                DiagnosticListener.AllListeners.Subscribe(app.ApplicationServices.GetRequiredService<DiagnsticListenerObserver>());
            };
        }
    }
}
