using Microsoft.Extensions.Hosting;
using NLog.Web;

namespace Wing.NLog
{
    public static class WingHostBuilderExtensions
    {
        public static IHostBuilder AddNLog(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseNLog();
        }
    }
}
