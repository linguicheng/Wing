using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Wing.Gateway.Config;
using Wing.Gateway.Middleware;

namespace Wing.Gateway
{
    internal class WingStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Func<IEnumerable<Downstream>, HttpContext, Task<bool>> authorization,
            WebSocketOptions webSocketOptions,
            Func<RequestData, Task<RequestData>> requestBefore,
            Func<ResponseData, Task<ResponseData>> responseAfter)
        {
            return app =>
            {
                if (webSocketOptions == null)
                {
                    webSocketOptions = new WebSocketOptions
                    {
                        KeepAliveInterval = TimeSpan.FromMinutes(2)
                    };
                }

                app.UseWebSockets(webSocketOptions);

                var middlewareBuilder = new MiddlewareBuilder(app.ApplicationServices);
                middlewareBuilder.UseMiddleware<RouteMapMiddleware>();
                middlewareBuilder.UseMiddleware<AuthenticationMiddleware>();
                middlewareBuilder.UseMiddleware<NoPolicyMiddleware>();
                middlewareBuilder.UseMiddleware<PolicyMiddleware>();
                middlewareBuilder.UseMiddleware<RoutePolicyMiddleware>();
                middlewareBuilder.UseMiddleware<WebSocketMiddleware>();
                var firstDelegate = middlewareBuilder.Build();
                async Task Middleware(HttpContext context, Func<Task> next)
                {
                    var serviceContext = new ServiceContext(context)
                    {
                        Authorization = authorization,
                        RequestBefore = requestBefore,
                        ResponseAfter = responseAfter
                    };
                    await firstDelegate.Invoke(serviceContext);
                }

                app.Use(Middleware);
            };
        }
    }
}
