using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wing.Persistence.GateWay;

[assembly: HostingStartup(typeof(Wing.SqlServer.SqlServerHostingStartup))]
namespace Wing.SqlServer
{
    public class SqlServerHostingStartup : IHostingStartup
    {

        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContext<GateWayDbContext>(options => options.UseSqlServer("name=ConnectionStrings:WingGateWay"));
                services.AddScoped<ILogService, LogService>();
            });
        }
    }
}
