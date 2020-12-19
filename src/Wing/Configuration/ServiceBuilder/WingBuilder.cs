using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wing.Convert;
using Wing.DynamicMethod;
using Wing.HttpTransport;
using Wing.LoadBalancer;
using Wing.Logger;
using Wing.ServiceProvider;

namespace Wing.Configuration.ServiceBuilder
{
    public class WingBuilder : IWingBuilder
    {
        public IServiceCollection Services { get; }

        public IConfiguration Configuration { get; }

        public WingBuilder(IServiceCollection services)
        {
            Services = services;
            Services.AddHttpClient();
            Services.AddHealthChecks();
            Services.AddSingleton<IMemoryCache, MemoryCache>();
            Services.AddSingleton(typeof(IWingLogger<>), typeof(WingLogger<>));
            Services.AddSingleton<IJson, JsonHelper>();
            Services.AddSingleton<IRequest, ApiRequest>();
            Services.AddSingleton<ILoadBalancerCache, LoadBalancerCache>();
            Services.AddSingleton<IServiceFactory, ServiceFactory>();
            GlobalInjection.RegisterCommandService(Services);
            ServiceLocator.ServiceProvider = Services.BuildServiceProvider();
            Configuration = ServiceLocator.GetRequiredService<IConfiguration>();
        }

        public T GetConfig<T>(string key)
        {
            return Configuration.GetSection(key).Get<T>();
        }
    }
}
