using Microsoft.Extensions.DependencyInjection;
using Wing.APM.Listeners;
using Wing.Configuration.ServiceBuilder;

namespace Wing.APM
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddAPM(this IWingServiceBuilder wingBuilder)
        {
            wingBuilder.Services.AddSingleton<IDiagnosticListener, HttpDiagnosticListener>();
            wingBuilder.Services.AddSingleton<IDiagnosticListener, AspNetCoreDiagnosticListener>();
            wingBuilder.Services.AddSingleton<DiagnsticListenerObserver>();
            wingBuilder.App += new WingStartupFilter().Configure();
            return wingBuilder;
        }
    }
}
