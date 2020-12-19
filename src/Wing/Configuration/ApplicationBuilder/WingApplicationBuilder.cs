using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wing.Configuration.ApplicationBuilder
{
    public class WingApplicationBuilder : IWingApplicationBuilder
    {
        public IApplicationBuilder ApplicationBuilder { get; }

        public IConfiguration Configuration { get; }

        public WingApplicationBuilder(IApplicationBuilder applicationBuilder)
        {
            ApplicationBuilder = applicationBuilder;
            Configuration = ApplicationBuilder.ApplicationServices.GetRequiredService<IConfiguration>();
        }

        public T GetConfig<T>(string key)
        {
            return Configuration.GetSection(key).Get<T>();
        }
    }
}
