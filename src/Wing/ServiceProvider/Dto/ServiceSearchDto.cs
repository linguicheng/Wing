using Wing.ServiceProvider.Config;

namespace Wing.ServiceProvider.Dto
{
    public class ServiceSearchDto
    {
        public string Name { get; set; }

        public ServiceOptions? ServiceType { get; set; }

        public LoadBalancerOptions? LoadBalancer { get; set; }

        public HealthStatus? Status { get; set; }
    }
}
