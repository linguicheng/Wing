using AspectCore.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Wing.Configuration.HostBuilder
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddWing(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseServiceProviderFactory(new WingServiceProviderFactory()).ConfigureAppConfiguration((hostingContext, config) =>
             {
                 config.AddJsonFile("wing.json", optional: false, reloadOnChange: true);
             }).UseServiceContext();
        }
    }
}
