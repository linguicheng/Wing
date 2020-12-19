using System.Collections.Generic;
using System.Linq;
using Wing.ServiceProvider;

namespace Wing.LoadBalancer
{
    public abstract class BaseLoadBalancer : ILoadBalancer
    {
        protected static readonly object _lock = new object();

        protected List<Service> Services { get; set; }

        public BaseLoadBalancer(List<Service> services)
        {
            Services = services;
        }

        public abstract ServiceAddress GetServiceAddress();

        public void UpdateServices(List<Service> services)
        {
            if (Services.Count != services.Count)
            {
                Services = services;
                UpdateServiceAfter();
                return;
            }

            var exists = false;
            foreach (var s in Services)
            {
                exists = services.Any(x => x.ServiceAddress == s.ServiceAddress);
                if (!exists)
                {
                    Services = services;
                    UpdateServiceAfter();
                    break;
                }
            }
        }

        protected virtual void UpdateServiceAfter()
        {
        }
    }
}
