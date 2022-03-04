using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wing.Convert;
using Wing.DynamicMethod;
using Wing.HttpTransport;
using Wing.LoadBalancer;
using Wing.ServiceProvider;

namespace Wing.Configuration.ServiceBuilder
{
    public class WingServiceBuilder : IWingServiceBuilder
    {
        public IServiceCollection Services { get; }

        public IConfiguration Configuration { get; }

        public WingServiceBuilder(IServiceCollection services)
        {
            Services = services;
            Services.AddHttpClient();
            Services.AddHealthChecks();
            Services.AddSingleton<IMemoryCache, MemoryCache>();
            Services.AddSingleton<IJson, JsonConverter>();
            Services.AddSingleton<IRequest, ApiRequest>();
            Services.AddSingleton<ILoadBalancerCache, LoadBalancerCache>();
            Services.AddSingleton<IServiceFactory, ServiceFactory>();
            GlobalInjection.RegisterCommandService(Services);
            ServiceLocator.ServiceProvider = Services.BuildServiceProvider();
            Configuration = ServiceLocator.GetRequiredService<IConfiguration>();
            var configCenterEnabled = Configuration["ConfigCenterEnabled"];
            if (configCenterEnabled != "False")
            {
                Services.AddSingleton<IHostedService, ConfigurationHostedService>();
            }
        }

        public T GetConfig<T>(string key)
        {
            return Configuration.GetSection(key).Get<T>();
        }
    }
}
