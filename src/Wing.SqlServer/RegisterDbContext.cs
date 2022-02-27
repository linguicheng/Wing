using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wing.Persistence;

namespace Wing.SqlServer
{
    public class RegisterDbContext : IRegisterContext
    {
        public void AddContext(IServiceCollection services, string gateWayConnectionString, string mqConnectionString)
        {
            services.AddDbContext<GateWayDbContext>(options => options.UseSqlServer(gateWayConnectionString));
        }
    }
}
