using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Wing.Gateway.Middleware;

namespace Wing.Gateway
{
    internal class WingStartupFilter
    {
        public Action<IApplicationBuilder> Configure(WebSocketOptions webSocketOptions = null)
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
                middlewareBuilder.UseMiddleware<WebSocketMiddleware>();
                var firstDelegate = middlewareBuilder.Build();
                async Task Middleware(HttpContext context, Func<Task> next)
                {
                    var serviceContext = new ServiceContext(context);
                    await firstDelegate.Invoke(serviceContext);
                }

                app.Use(Middleware);
            };
        }
    }
}
