using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wing.Injection;

namespace Wing.Configuration.ServiceBuilder
{
    public class WingServiceBuilder : IWingServiceBuilder
    {
        public IServiceCollection Services { get; }

        public Action<IApplicationBuilder> AppBuilder { get; set; }

        public WingServiceBuilder(IServiceCollection services)
        {
            Services = services;
            Services.AddHttpClient();
            Services.AddHealthChecks();
            Services.AddSingleton<IMemoryCache, MemoryCache>();
            GlobalInjection.Injection(Services);
            App.ServiceProvider = Services.BuildServiceProvider();
            App.Configuration = App.GetRequiredService<IConfiguration>();
            var configCenterEnabled = App.Configuration["ConfigCenterEnabled"];
            if (configCenterEnabled != "False")
            {
                Services.AddSingleton<IHostedService, ConfigurationHostedService>();
            }
        }
    }
}
