using Wing.ServiceProvider.Config;

namespace Wing.ServiceProvider
{
    public class Service
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ServiceAddress ServiceAddress { get; set; }

        public int Weight { get; set; }

        public HealthStatus Status { get; set; }

        public ServiceOptions ServiceOptions { get; set; }

        public LoadBalancerOptions LoadBalancer { get; set; }

        public string Developer { get; set; }

        public string ConfigKey { get; set; }

        public int EffectiveWeight { get; set; }

        internal int CurrentWeight { get; set; }
    }
}
