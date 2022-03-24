using System;
using AspectCore.DependencyInjection;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Wing.Configuration.HostBuilder
{
    public class WingServiceProviderFactory : IServiceProviderFactory<IServiceContext>
    {
        public IServiceContext CreateBuilder(IServiceCollection services)
        {
            ServiceLocator.ServiceProvider = services.BuildServiceProvider();
            return services.ToServiceContext();
        }

        public IServiceProvider CreateServiceProvider(IServiceContext contextBuilder)
        {
            return contextBuilder.Build();
        }
    }
}
