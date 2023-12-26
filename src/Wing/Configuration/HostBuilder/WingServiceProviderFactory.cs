using Microsoft.Extensions.DependencyInjection;

namespace Wing.Configuration.HostBuilder
{
    public class WingServiceProviderFactory : IServiceProviderFactory<IServiceProvider>
    {
        public IServiceProvider CreateBuilder(IServiceCollection services)
        {
           return App.ServiceProvider = services.BuildServiceProvider();
        }

        public IServiceProvider CreateServiceProvider(IServiceProvider containerBuilder)
        {
            return containerBuilder;
        }
    }
}
