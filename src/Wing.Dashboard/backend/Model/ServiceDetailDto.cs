using Wing.ServiceProvider;
using Wing.ServiceProvider.Config;

namespace Wing.Dashboard.Model
{
    public class ServiceDetailDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int Weight { get; set; }

        public LoadBalancerOptions LoadBalancer { get; set; }

        public ServiceOptions ServiceType { get; set; }

        public HealthStatus Status { get; set; }

        public string Developer { get; set; }
    }
}
