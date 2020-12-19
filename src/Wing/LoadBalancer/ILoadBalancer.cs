using System.Collections.Generic;
using Wing.ServiceProvider;

namespace Wing.LoadBalancer
{
    public interface ILoadBalancer
    {
        ServiceAddress GetServiceAddress();

        void UpdateServices(List<Service> services);
    }
}
