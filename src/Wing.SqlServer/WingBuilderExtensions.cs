using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wing.Configuration.ServiceBuilder;

namespace Wing.Persistence
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddPersistence(this IWingServiceBuilder wingBuilder, string gateWayConnectionString = "name=ConnectionStrings:WingGateWay", string mqConnectionString = "name=ConnectionStrings:WingMQ")
        {
            wingBuilder.Services.AddDbContext<GateWayDbContext>(options => options.UseSqlServer(gateWayConnectionString));
            return wingBuilder;
        }
    }
}
