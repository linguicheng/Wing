using Wing.ServiceProvider;

namespace Wing.LoadBalancer
{
    public interface ILoadBalancer
    {
        ServiceAddress GetServiceAddress(string key);

        ServiceAddress GetServiceAddress();

        void UpdateServices(List<Service> services);
    }
}
