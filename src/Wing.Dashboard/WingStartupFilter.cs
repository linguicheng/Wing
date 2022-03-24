using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Wing.Injection;

namespace Wing.Dashboard
{
    internal class WingStartupFilter
    {
        public Action<IApplicationBuilder> Configure()
        {
            return app =>
            {
                var wingDashboard = GlobalInjection.Assemblies.Where(u => u.FullName.Contains("Wing.Dashboard")).First();
                var personEmbeddedFileProvider = new EmbeddedFileProvider(
                    wingDashboard,
                    "Wing.Dashboard.wwwroot.dist");
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = personEmbeddedFileProvider,
                    RequestPath = new PathString("/wing")
                });
            };
        }
    }
}
