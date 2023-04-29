using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            return ServiceInvoke(serviceName, func);
        }

        public void Invoke(string serviceName, Action<ServiceAddress> action)
        {
            ServiceInvoke(serviceName, action).GetAwaiter().GetResult();
        }

        public async Task<T> InvokeAsync<T>(string serviceName, Func<ServiceAddress, Task<T>> func)
        {
            return await ServiceInvoke(serviceName, func);
        }

        public async Task InvokeAsync(string serviceName, Action<ServiceAddress> action)
        {
            await ServiceInvoke(serviceName, action);
        }

        private async Task<T> ServiceInvoke<T>(string serviceName, Func<ServiceAddress, Task<T>> func)
        {
            WeightRoundRobin weightRoundRobin = null;
            ServiceAddress serviceAddress = null;
            try
            {
                var serviceInfo = await GetServices(serviceName);
                serviceAddress = serviceInfo.Item1;
                var loadBalancer = serviceInfo.Item2;
                var result = await func(serviceAddress);
                if (loadBalancer is WeightRoundRobin)
                {
                    weightRoundRobin = loadBalancer as WeightRoundRobin;
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
                throw ex;
            }
        }

        private T ServiceInvoke<T>(string serviceName, Func<ServiceAddress, T> func)
        {
            WeightRoundRobin weightRoundRobin = null;
            ServiceAddress serviceAddress = null;
            try
            {
                var serviceInfo = GetServices(serviceName).GetAwaiter().GetResult();
                serviceAddress = serviceInfo.Item1;
                var loadBalancer = serviceInfo.Item2;
                var result = func(serviceAddress);
                if (loadBalancer is WeightRoundRobin)
                {
                    weightRoundRobin = loadBalancer as WeightRoundRobin;
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
                throw ex;
            }
        }

        private async Task ServiceInvoke(string serviceName, Action<ServiceAddress> action)
        {
            WeightRoundRobin weightRoundRobin = null;
            ServiceAddress serviceAddress = null;
            try
            {
                var serviceInfo = await GetServices(serviceName);
                serviceAddress = serviceInfo.Item1;
                var loadBalancer = serviceInfo.Item2;
                action(serviceAddress);
                if (loadBalancer is WeightRoundRobin)
                {
                    weightRoundRobin = loadBalancer as WeightRoundRobin;
                    weightRoundRobin.AddWeight(serviceAddress);
                    return;
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
                throw ex;
            }
        }

        private async Task<(ServiceAddress, ILoadBalancer)> GetServices(string serviceName)
        {
            serviceName.IsNotNull();
            var services = await _discoveryServiceProvider.Get(serviceName, HealthStatus.Healthy);

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
                    LoadBalancerOptions = LoadBalancerOptions.RoundRobin,
                    LoadBalancer = new RoundRobin(services)
                };
                var serviceAddress = loadBalancerConfig.LoadBalancer.GetServiceAddress();

                _loadBalancerCache.Add(serviceName, loadBalancerConfig);
                return (serviceAddress, loadBalancerConfig.LoadBalancer);
            }

            return (GetServiceAddress(loadBalancerConfig.LoadBalancer, services), loadBalancerConfig.LoadBalancer);
        }

        private ServiceAddress GetServiceAddress(ILoadBalancer loadBalancer, List<Service> services)
        {
            loadBalancer.UpdateServices(services);
            return loadBalancer.GetServiceAddress();
        }
    }
}
