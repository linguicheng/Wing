using Wing.Configuration;

namespace Wing.Consul
{
    public class Config
    {
        public string Url { get; set; }

        public ServiceConfig Service { get; set; }

        public int? Interval { get; set; }
    }

    public class ServiceConfig
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

    public class Healthcheck
    {
        public string Url { get; set; }

        public int? Timeout { get; set; }

        public int? Interval { get; set; }

        public int? RemoveService { get; set; }

        public bool? GRPCUseTLS { get; set; }
    }

    public class LoadBalancer
    {
        public LoadBalancerOptions Option { get; set; }

        public int? Weight { get; set; }
    }

}
