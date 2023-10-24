using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Wing.APM.Listeners;

namespace Wing.APM.Builder
{
    internal class WingStartupFilter
    {
        public Action<IApplicationBuilder> Configure()
        {
            return app =>
            {
                DiagnosticListener.AllListeners.Subscribe(App.GetRequiredService<DiagnsticListenerObserver>());
                app.UseMiddleware<ApmMiddleware>();
            };
        }
    }
}
