using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Wing.Configuration.ServiceBuilder;
using Wing.Gateway;

namespace Wing
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddGateWay(this IWingServiceBuilder wingBuilder, WebSocketOptions webSocketOptions = null)
        {
            wingBuilder.Services.AddScoped<ILogProvider, LogProvider>();
            wingBuilder.AppBuilder += new WingStartupFilter().Configure(webSocketOptions);
            return wingBuilder;
        }
    }
}
