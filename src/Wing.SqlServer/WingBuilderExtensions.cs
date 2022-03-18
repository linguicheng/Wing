using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wing.Configuration.ServiceBuilder;
using Wing.Persistence.GateWay;
using Wing.SqlServer;

namespace Wing.Persistence
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddPersistence(this IWingServiceBuilder wingBuilder, string gateWayConnectionString = "name=ConnectionStrings:WingGateWay", string mqConnectionString = "name=ConnectionStrings:WingMQ")
        {
            wingBuilder.Services.AddDbContext<GateWayDbContext>(options => options.UseSqlServer(gateWayConnectionString));
            wingBuilder.Services.AddTransient<ILogService, LogService>();
            return wingBuilder;
        }
    }
}
