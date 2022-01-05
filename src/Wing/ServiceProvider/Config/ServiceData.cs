using Wing.Configuration;

namespace Wing.ServiceProvider.Config
{
    public class ServiceData
    {
        public ServiceOptions Option { get; set; }

        public Healthcheck HealthCheck { get; set; }

        public string Name { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Tag { get; set; }

        public LoadBalancer LoadBalancer { get; set; }

        public SchemeOptions Scheme { get; set; }
    }
}
