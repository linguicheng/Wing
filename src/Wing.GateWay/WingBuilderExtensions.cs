using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wing.Configuration.ServiceBuilder;
using Wing.Gateway;
using Wing.Gateway.Config;

namespace Wing
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddGateWay(this IWingServiceBuilder wingBuilder)
        {
            return AddGateWay(wingBuilder, null, null, null, null, null);
        }

        public static IWingServiceBuilder AddGateWay(this IWingServiceBuilder wingBuilder, Action<IApplicationBuilder> app)
        {
            return AddGateWay(wingBuilder, null, null, null, null, app);
        }

        public static IWingServiceBuilder AddGateWay(this IWingServiceBuilder wingBuilder,
            WebSocketOptions webSocketOptions,
            Action<IApplicationBuilder> app = null)
        {
            return AddGateWay(wingBuilder, null, webSocketOptions, null, null, app);
        }

        public static IWingServiceBuilder AddGateWay(this IWingServiceBuilder wingBuilder,
            Func<IEnumerable<Downstream>, HttpContext, Task<bool>> authorization,
            Action<IApplicationBuilder> app = null)
        {
            return AddGateWay(wingBuilder, authorization, null, null, null, app);
        }

        public static IWingServiceBuilder AddGateWay(this IWingServiceBuilder wingBuilder,
            Func<IEnumerable<Downstream>, HttpContext, Task<bool>> authorization,
            WebSocketOptions webSocketOptions,
            Action<IApplicationBuilder> app = null)
        {
            return AddGateWay(wingBuilder, authorization, webSocketOptions, null, null, app);
        }

        public static IWingServiceBuilder AddGateWay(this IWingServiceBuilder wingBuilder,
            Func<IEnumerable<Downstream>, HttpContext, Task<bool>> authorization,
            Func<RequestData, Task<RequestData>> requestBefore, Action<IApplicationBuilder> app = null)
        {
            return AddGateWay(wingBuilder, authorization, null, requestBefore, null, app);
        }

        public static IWingServiceBuilder AddGateWay(this IWingServiceBuilder wingBuilder,
            Func<IEnumerable<Downstream>, HttpContext, Task<bool>> authorization,
            Func<RequestData, Task<RequestData>> requestBefore,
            Func<ResponseData, Task<ResponseData>> responseAfter, Action<IApplicationBuilder> app = null)
        {
            return AddGateWay(wingBuilder, authorization, null, requestBefore, responseAfter, app);
        }

        public static IWingServiceBuilder AddGateWay(this IWingServiceBuilder wingBuilder,
            Func<IEnumerable<Downstream>, HttpContext, Task<bool>> authorization,
            WebSocketOptions webSocketOptions,
            Func<RequestData, Task<RequestData>> requestBefore,
            Func<ResponseData, Task<ResponseData>> responseAfter,
            Action<IApplicationBuilder> app = null)
        {
            wingBuilder.Services.AddScoped<ILogProvider, LogProvider>();
            if (DataProvider.LogConfig != null
                && DataProvider.LogConfig.IsEnabled
                && !DataProvider.LogConfig.UseEventBus)
            {
                wingBuilder.Services.AddSingleton<IHostedService, LogHostedService>();
            }

            if (app != null)
            {
                wingBuilder.AppBuilder += app;
            }

            wingBuilder.AppBuilder += new WingStartupFilter()
                .Configure(authorization,
                           webSocketOptions,
                           requestBefore,
                           responseAfter);
            return wingBuilder;
        }
    }
}
