using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wing.APM;
using Wing.APM.Builder;
using Wing.APM.Listeners;
using Wing.Configuration.ServiceBuilder;

namespace Wing
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddAPM(this IWingServiceBuilder wingBuilder, Action<WingApmBuilder> wingApmBuilder = null)
        {
            wingBuilder.Services.AddSingleton<IDiagnosticListener, HttpDiagnosticListener>();
            wingBuilder.Services.AddSingleton<IDiagnosticListener, AspNetCoreDiagnosticListener>();
            wingBuilder.Services.AddSingleton<DiagnsticListenerObserver>();
            wingBuilder.Services.AddSingleton<IHostedService, TracerHostedService>();
            wingBuilder.Services.AddGrpc(x => x.Interceptors.Add<GrpcInterceptor>());
            wingApmBuilder?.Invoke(new WingApmBuilder(wingBuilder));
            wingBuilder.AppBuilder += new WingStartupFilter().Configure();
            return wingBuilder;
        }
    }
}
