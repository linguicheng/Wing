using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Wing.APM.Listeners;

namespace Wing.APM.Builder
{
    internal class WingStartupFilter
    {
        public Action<IApplicationBuilder> Configure()
        {
            return app =>
            {
                DiagnosticListener.AllListeners.Subscribe(app.ApplicationServices.GetRequiredService<DiagnsticListenerObserver>());
                app.UseMiddleware<ApmMiddleware>();
            };
        }
    }
}
