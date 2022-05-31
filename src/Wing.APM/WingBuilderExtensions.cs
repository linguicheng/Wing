using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wing.APM.Listeners;
using Wing.Configuration.ServiceBuilder;

namespace Wing.APM
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddAPM(this IWingServiceBuilder wingBuilder, Action<WingApmBuilder> wingApmBuilder = null)
        {
            wingBuilder.Services.AddSingleton<IDiagnosticListener, HttpDiagnosticListener>();
            wingBuilder.Services.AddSingleton<IDiagnosticListener, AspNetCoreDiagnosticListener>();
            wingBuilder.Services.AddSingleton<DiagnsticListenerObserver>();
            wingBuilder.Services.AddSingleton<IHostedService, TracerHostedService>();
            wingApmBuilder?.Invoke(new WingApmBuilder(wingBuilder.Services));
            wingBuilder.App += new WingStartupFilter().Configure();
            return wingBuilder;
        }
    }
}
