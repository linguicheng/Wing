using Wing.ServiceProvider;
using Wing.ServiceProvider.Config;

namespace Wing.UI.Model
{
    public class ServiceSearchDto
    {
        public string Name { get; set; }

        public ServiceOptions? ServiceType { get; set; }

        public LoadBalancerOptions? LoadBalancer { get; set; }

        public HealthStatus? Status { get; set; }
    }
}
