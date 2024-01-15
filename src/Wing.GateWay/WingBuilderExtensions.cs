using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wing.Configuration.ServiceBuilder;
using Wing.Gateway;

namespace Wing
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddGateWay(this IWingServiceBuilder wingBuilder, Func<IEnumerable<string>, HttpContext, Task<bool>> authorization = null, WebSocketOptions webSocketOptions = null)
        {
            wingBuilder.Services.AddScoped<ILogProvider, LogProvider>();
            if (DataProvider.LogConfig != null
                && DataProvider.LogConfig.IsEnabled
                && !DataProvider.LogConfig.UseEventBus)
            {
                wingBuilder.Services.AddSingleton<IHostedService, LogHostedService>();
            }

            wingBuilder.AppBuilder += new WingStartupFilter().Configure(authorization, webSocketOptions);
            return wingBuilder;
        }
    }
}
