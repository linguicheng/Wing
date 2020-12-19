using Microsoft.Extensions.DependencyInjection;

namespace Wing.Configuration.ServiceBuilder
{
    public static class ServiceCollectionExtensions
    {
        public static IWingBuilder AddWing(this IServiceCollection services)
        {
            return new WingBuilder(services);
        }
    }
}
