using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Wing.Configuration.HostBuilder;

namespace Wing
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddWing(this IHostBuilder hostBuilder, Action<IConfigurationBuilder> action = null)
        {
            return hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
             {
                 action?.Invoke(config);
             }).UseServiceProviderFactory(new WingServiceProviderFactory());
        }
    }
}
