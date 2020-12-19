using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wing.ServiceProvider
{
    public interface IDiscoveryServiceProvider
    {
        Task<List<Service>> Get();

        Task<List<Service>> Get(string serviceName);

        Task<List<Service>> GetGrpcServices(string serviceName);

        Task<List<Service>> GetHttpServices(string serviceName);
    }
}
