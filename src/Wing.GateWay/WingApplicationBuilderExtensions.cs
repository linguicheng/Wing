using Microsoft.AspNetCore.Builder;
using Wing.Configuration.ApplicationBuilder;
using Wing.GateWay.Middleware;

namespace Wing.GateWay
{
    public static class WingApplicationBuilderExtensions
    {
        public static IWingApplicationBuilder UseGateWay(this IWingApplicationBuilder app)
        {
            var middlewareBuilder = new MiddlewareBuilder(app.ApplicationBuilder.ApplicationServices);
            middlewareBuilder.UseMiddleware<RouteMapMiddleware>();
            middlewareBuilder.UseMiddleware<AuthenticationMiddleware>();
            middlewareBuilder.UseMiddleware<NoPolicyMiddleware>();
            middlewareBuilder.UseMiddleware<PolicyMiddleware>();
            var firstDelegate = middlewareBuilder.Build();
            app.ApplicationBuilder.Use(async (context, task) =>
            {
                var serviceContext = new ServiceContext(context);
                await firstDelegate.Invoke(serviceContext);
            });

            return app;
        }
    }
}
