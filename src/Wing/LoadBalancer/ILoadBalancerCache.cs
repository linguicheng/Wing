namespace Wing.LoadBalancer
{
    public interface ILoadBalancerCache
    {
        void Add(string serviceName, LoadBalancerConfig loadBalancerConfig);

        LoadBalancerConfig Get(string serviceName);
    }
}
