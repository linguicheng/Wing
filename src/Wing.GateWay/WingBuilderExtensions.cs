using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wing.Configuration.ServiceBuilder;
using Wing.Gateway;

namespace Wing
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddGateWay(this IWingServiceBuilder wingBuilder, WebSocketOptions webSocketOptions = null)
        {
            wingBuilder.Services.AddScoped<ILogProvider, LogProvider>();
            if (DataProvider.LogConfig != null
                && DataProvider.LogConfig.IsEnabled
                && !DataProvider.LogConfig.UseEventBus)
            {
                wingBuilder.Services.AddSingleton<IHostedService, LogHostedService>();
            }

            wingBuilder.AppBuilder += new WingStartupFilter().Configure(webSocketOptions);
            return wingBuilder;
        }
    }
}
