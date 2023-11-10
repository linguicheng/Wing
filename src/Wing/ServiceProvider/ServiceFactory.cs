using Microsoft.Extensions.Logging;
using Wing.Converter;
using Wing.Exceptions;
using Wing.Injection;
using Wing.LoadBalancer;
using Wing.ServiceProvider.Config;

namespace Wing.ServiceProvider
{
    public class ServiceFactory : IServiceFactory, ISingleton
    {
        private static readonly object _lock = new();
        private readonly IDiscoveryServiceProvider _discoveryServiceProvider;
        private readonly ILogger<ServiceFactory> _logger;
        private readonly ILoadBalancerCache _loadBalancerCache;

        public ServiceFactory(ILogger<ServiceFactory> logger, ILoadBalancerCache loadBalancerCache)
        {
            _discoveryServiceProvider = App.DiscoveryService;
            _logger = logger;
            _loadBalancerCache = loadBalancerCache;
        }

        public T Invoke<T>(string serviceName, Func<ServiceAddress, T> func)
        {
            return ServiceInvoke(serviceName, null, func);
        }

        public T Invoke<T>(string serviceName, string key, Func<ServiceAddress, T> func)
        {
            return ServiceInvoke(serviceName, key, func);
        }

        public void Invoke(string serviceName, Action<ServiceAddress> action)
        {
            ServiceInvoke(serviceName, null, action);
        }

        public void Invoke(string serviceName, string key, Action<ServiceAddress> action)
        {
            ServiceInvoke(serviceName, key, action);
        }

        public async Task<T> InvokeAsync<T>(string serviceName, Func<ServiceAddress, Task<T>> func)
        {
            return await ServiceInvoke(serviceName, null, func);
        }

        public async Task<T> InvokeAsync<T>(string serviceName, string key, Func<ServiceAddress, Task<T>> func)
        {
            return await ServiceInvoke(serviceName, key, func);
        }

        public async Task InvokeAsync(string serviceName, Func<ServiceAddress, Task> func)
        {
            await ServiceInvoke(serviceName, null, func);
        }

        public async Task InvokeAsync(string serviceName, string key, Func<ServiceAddress, Task> func)
        {
            await ServiceInvoke(serviceName, key, func);
        }

        private async Task<T> ServiceInvoke<T>(string serviceName, string key, Func<ServiceAddress, Task<T>> func)
        {
            WeightRoundRobin weightRoundRobin = null;
            ServiceAddress serviceAddress = null;
            try
            {
                var serviceInfo = await GetServices(serviceName, key);
                serviceAddress = serviceInfo.Item1;
                var loadBalancer = serviceInfo.Item2;
                if (loadBalancer is WeightRoundRobin)
                {
                    weightRoundRobin = loadBalancer as WeightRoundRobin;
                }

                var result = await func(serviceAddress);
                if (weightRoundRobin != null)
                {
                    weightRoundRobin.AddWeight(serviceAddress);
                    return result;
                }

                if (loadBalancer is LeastConnection)
                {
                    (loadBalancer as LeastConnection).ReLease(serviceAddress);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"服务【{serviceName}】调用异常");
                weightRoundRobin?.ReduceWeight(serviceAddress);
                throw;
            }
        }

        private async Task ServiceInvoke(string serviceName, string key, Func<ServiceAddress, Task> func)
        {
            WeightRoundRobin weightRoundRobin = null;
            ServiceAddress serviceAddress = null;
            try
            {
                var serviceInfo = await GetServices(serviceName, key);
                serviceAddress = serviceInfo.Item1;
                var loadBalancer = serviceInfo.Item2;
                if (loadBalancer is WeightRoundRobin)
                {
                    weightRoundRobin = loadBalancer as WeightRoundRobin;
                }

                await func(serviceAddress);
                if (weightRoundRobin != null)
                {
                    weightRoundRobin.AddWeight(serviceAddress);
                }

                if (loadBalancer is LeastConnection)
                {
                    (loadBalancer as LeastConnection).ReLease(serviceAddress);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"服务【{serviceName}】调用异常");
                weightRoundRobin?.ReduceWeight(serviceAddress);
                throw;
            }
        }

        private T ServiceInvoke<T>(string serviceName, string key, Func<ServiceAddress, T> func)
        {
            WeightRoundRobin weightRoundRobin = null;
            ServiceAddress serviceAddress = null;
            try
            {
                var serviceInfo = GetServices(serviceName, key).GetAwaiter().GetResult();
                serviceAddress = serviceInfo.Item1;
                var loadBalancer = serviceInfo.Item2;
                if (loadBalancer is WeightRoundRobin)
                {
                    weightRoundRobin = loadBalancer as WeightRoundRobin;
                }

                var result = func(serviceAddress);
                if (weightRoundRobin != null)
                {
                    weightRoundRobin.AddWeight(serviceAddress);
                    return result;
                }

                if (loadBalancer is LeastConnection)
                {
                    (loadBalancer as LeastConnection).ReLease(serviceAddress);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"服务【{serviceName}】调用异常");
                weightRoundRobin?.ReduceWeight(serviceAddress);
                throw;
            }
        }

        private void ServiceInvoke(string serviceName, string key, Action<ServiceAddress> action)
        {
            WeightRoundRobin weightRoundRobin = null;
            ServiceAddress serviceAddress = null;
            try
            {
                var serviceInfo = GetServices(serviceName, key).GetAwaiter().GetResult();
                serviceAddress = serviceInfo.Item1;
                var loadBalancer = serviceInfo.Item2;
                if (loadBalancer is WeightRoundRobin)
                {
                    weightRoundRobin = loadBalancer as WeightRoundRobin;
                }

                action(serviceAddress);
                if (weightRoundRobin != null)
                {
                    weightRoundRobin.AddWeight(serviceAddress);
                }

                if (loadBalancer is LeastConnection)
                {
                    (loadBalancer as LeastConnection).ReLease(serviceAddress);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"服务【{serviceName}】调用异常");
                weightRoundRobin?.ReduceWeight(serviceAddress);
                throw;
            }
        }

        private async Task<(ServiceAddress, ILoadBalancer)> GetServices(string serviceName, string key)
        {
            serviceName.IsNotNull();
            var services = await _discoveryServiceProvider.Get(serviceName, HealthStatus.Healthy);
            lock (_lock)
            {
                if (services.Count == 0)
                {
                    throw new ServiceNotFoundException(serviceName);
                }

                if (services.Count == 1)
                {
                    return (services[0].ServiceAddress, null);
                }

                var loadBalancerConfig = _loadBalancerCache.Get(serviceName);
                if (loadBalancerConfig == null || loadBalancerConfig.LoadBalancerOptions != services[0].LoadBalancer)
                {
                    loadBalancerConfig = new LoadBalancerConfig
                    {
                        LoadBalancerOptions = services[0].LoadBalancer,
                    };
                    ServiceAddress serviceAddress;
                    switch (loadBalancerConfig.LoadBalancerOptions)
                    {
                        case LoadBalancerOptions.LeastConnection:
                            loadBalancerConfig.LoadBalancer = new LeastConnection(services);
                            serviceAddress = loadBalancerConfig.LoadBalancer.GetServiceAddress();
                            break;
                        case LoadBalancerOptions.WeightRoundRobin:
                            loadBalancerConfig.LoadBalancer = new WeightRoundRobin(services);
                            serviceAddress = loadBalancerConfig.LoadBalancer.GetServiceAddress();
                            break;
                        case LoadBalancerOptions.ConsistentHash:
                            if (string.IsNullOrWhiteSpace(key))
                            {
                                throw new ArgumentEmptyException(nameof(key));
                            }

                            loadBalancerConfig.LoadBalancer = new ConsistentHash(services);
                            serviceAddress = loadBalancerConfig.LoadBalancer.GetServiceAddress(key);
                            break;
                        default:
                            loadBalancerConfig.LoadBalancer = new RoundRobin(services);
                            serviceAddress = loadBalancerConfig.LoadBalancer.GetServiceAddress();
                            break;
                    }

                    _loadBalancerCache.Add(serviceName, loadBalancerConfig);
                    return (serviceAddress, loadBalancerConfig.LoadBalancer);
                }

                return (GetServiceAddress(loadBalancerConfig.LoadBalancer, services, key), loadBalancerConfig.LoadBalancer);
            }
        }

        private ServiceAddress GetServiceAddress(ILoadBalancer loadBalancer, List<Service> services, string key)
        {
            loadBalancer.UpdateServices(services);
            if (loadBalancer is ConsistentHash)
            {
                return loadBalancer.GetServiceAddress(key);
            }

            return loadBalancer.GetServiceAddress();
        }
    }
}
