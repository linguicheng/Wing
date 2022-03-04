using Microsoft.Extensions.DependencyInjection;
using Wing.Configuration.ServiceBuilder;

namespace Wing.GateWay
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddGateWay(this IWingServiceBuilder wingBuilder)
        {
            wingBuilder.Services.AddScoped<ILogProvider, LogProvider>();
            return wingBuilder;
        }
    }
}
