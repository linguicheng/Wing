using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wing.Configuration.ServiceBuilder
{
    public interface IWingBuilder
    {
        IServiceCollection Services { get; }

        IConfiguration Configuration { get; }

        T GetConfig<T>(string key);
    }
}
