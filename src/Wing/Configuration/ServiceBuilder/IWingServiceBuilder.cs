using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wing.Configuration.ServiceBuilder
{
    public interface IWingServiceBuilder
    {
        IServiceCollection Services { get; }

        IConfiguration Configuration { get; }

        T GetConfig<T>(string key);
    }
}
