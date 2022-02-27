using Microsoft.Extensions.DependencyInjection;

namespace Wing.Persistence
{
    public interface IRegisterContext
    {
        void AddContext(IServiceCollection services, string gateWayConnectionString, string mqConnectionString);
    }
}
