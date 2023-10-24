using System.Collections.Concurrent;
using Wing.Injection;

namespace Wing.LoadBalancer
{
    public class LoadBalancerCache : ILoadBalancerCache, ISingleton
    {
        private readonly ConcurrentDictionary<string, LoadBalancerConfig> _loadBalancerConfigs;

        public LoadBalancerCache()
        {
            _loadBalancerConfigs = new ConcurrentDictionary<string, LoadBalancerConfig>();
        }

        public void Add(string serviceName, LoadBalancerConfig loadBalancerConfig)
        {
            if (!_loadBalancerConfigs.ContainsKey(serviceName))
            {
                _loadBalancerConfigs.TryAdd(serviceName, loadBalancerConfig);
                return;
            }

            var old = _loadBalancerConfigs[serviceName];
            if (old.LoadBalancerOptions != loadBalancerConfig.LoadBalancerOptions)
            {
                _loadBalancerConfigs.TryAdd(serviceName, loadBalancerConfig);
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
