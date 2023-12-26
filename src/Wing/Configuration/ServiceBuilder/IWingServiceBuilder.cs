using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Wing.Configuration.ServiceBuilder
{
    public interface IWingServiceBuilder
    {
        IServiceCollection Services { get; }

        Action<IApplicationBuilder> AppBuilder { get; set; }
    }
}
