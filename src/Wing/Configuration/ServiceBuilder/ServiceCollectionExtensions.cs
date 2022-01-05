using Microsoft.Extensions.DependencyInjection;

namespace Wing.Configuration.ServiceBuilder
{
    public static class ServiceCollectionExtensions
    {
        public static IWingServiceBuilder AddWing(this IServiceCollection services)
        {
            return new WingServiceBuilder(services);
        }
    }
}
