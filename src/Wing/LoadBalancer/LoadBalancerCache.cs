using System.Collections.Generic;

namespace Wing.LoadBalancer
{
    public class LoadBalancerCache : ILoadBalancerCache
    {
        private readonly Dictionary<string, LoadBalancerConfig> _loadBalancerConfigs;

        public LoadBalancerCache()
        {
            _loadBalancerConfigs = new Dictionary<string, LoadBalancerConfig>();
        }

        public void Add(string serviceName, LoadBalancerConfig loadBalancerConfig)
        {
            if (!_loadBalancerConfigs.ContainsKey(serviceName))
            {
                _loadBalancerConfigs.Add(serviceName, loadBalancerConfig);
                return;
            }

            var old = _loadBalancerConfigs[serviceName];
            if (old.LoadBalancerOptions != loadBalancerConfig.LoadBalancerOptions)
            {
                _loadBalancerConfigs.Add(serviceName, loadBalancerConfig);
            }
        }

        public LoadBalancerConfig Get(string serviceName)
        {
            if (!_loadBalancerConfigs.ContainsKey(serviceName))
            {
                return null;
            }

            return _loadBalancerConfigs[serviceName];
        }
    }
}
