using Microsoft.Extensions.DependencyInjection;
using Wing.Configuration.ServiceBuilder;
using Wing.GateWay;

namespace Wing
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddGateWay(this IWingServiceBuilder wingBuilder)
        {
            wingBuilder.Services.AddScoped<ILogProvider, LogProvider>();
            wingBuilder.AppBuilder += new WingStartupFilter().Configure();
            return wingBuilder;
        }
    }
}
