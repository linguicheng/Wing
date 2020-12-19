using Wing.Configuration;

namespace Wing.LoadBalancer
{
    public class LoadBalancerConfig
    {
        public LoadBalancerOptions LoadBalancerOptions { get; set; }

        public ILoadBalancer LoadBalancer { get; set; }
    }
}
