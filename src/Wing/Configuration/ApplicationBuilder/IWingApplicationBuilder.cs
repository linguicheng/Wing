using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Wing.Configuration.ApplicationBuilder
{
    public interface IWingApplicationBuilder
    {
        public IApplicationBuilder ApplicationBuilder { get; }

        public IConfiguration Configuration { get; }

        T GetConfig<T>(string key);
    }
}
