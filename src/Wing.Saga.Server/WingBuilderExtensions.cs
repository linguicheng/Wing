using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wing.Configuration.ServiceBuilder;

namespace Wing.Saga.Server
{
    public static class WingBuilderExtensions
    {
        private static IWingServiceBuilder AddConfig(IWingServiceBuilder wingBuilder)
        {
            //wingBuilder.Services.AddSingleton<IHostedService,>();
            return wingBuilder;
        }
    }
}
