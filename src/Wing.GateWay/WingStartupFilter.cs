using System;
using Microsoft.AspNetCore.Builder;
using Wing.GateWay.Middleware;

namespace Wing.GateWay
{
    internal class WingStartupFilter
    {
        public Action<IApplicationBuilder> Configure()
        {
            return app =>
            {
                var middlewareBuilder = new MiddlewareBuilder(app.ApplicationServices);
                middlewareBuilder.UseMiddleware<RouteMapMiddleware>();
                middlewareBuilder.UseMiddleware<AuthenticationMiddleware>();
                middlewareBuilder.UseMiddleware<NoPolicyMiddleware>();
                middlewareBuilder.UseMiddleware<PolicyMiddleware>();
                var firstDelegate = middlewareBuilder.Build();
                app.Use(async (context, task) =>
                {
                    var serviceContext = new ServiceContext(context);
                    await firstDelegate.Invoke(serviceContext);
                });
            };
        }
    }
}
