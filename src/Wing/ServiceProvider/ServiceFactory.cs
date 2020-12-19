using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wing.Configuration;
using Wing.Exceptions;
using Wing.LoadBalancer;
using Wing.Logger;

namespace Wing.ServiceProvider
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IDiscoveryServiceProvider _discoveryServiceProvider;
        private readonly IWingLogger<ServiceFactory> _logger;
        private readonly ILoadBalancerCache _loadBalancerCache;

        public ServiceFactory(IDiscoveryServiceProvider discoveryServiceProvider, IWingLogger<ServiceFactory> logger, ILoadBalancerCache loadBalancerCache)
        {
            _discoveryServiceProvider = discoveryServiceProvider;
            _logger = logger;
            _loadBalancerCache = loadBalancerCache;
        }

        public async Task<T> GrpcServiceInvoke<T>(string serviceName, Func<ServiceAddress, Task<T>> func)
        {
            return await ServiceInvoke(serviceName, func, ServiceOptions.Grpc);
        }

        public async Task<T> HttpServiceInvoke<T>(string serviceName, Func<ServiceAddress, Task<T>> func)
        {
            return await ServiceInvoke(serviceName, func, ServiceOptions.Http);
        }

        public async Task GrpcServiceInvoke(string serviceName, Action<ServiceAddress> action)
        {
            await ServiceInvoke(serviceName, action, ServiceOptions.Grpc);
        }

        public async Task HttpServiceInvoke(string serviceName, Action<ServiceAddress> action)
        {
            await ServiceInvoke(serviceName, action, ServiceOptions.Http);
        }

        private async Task<T> ServiceInvoke<T>(string serviceName, Func<ServiceAddress, Task<T>> func, ServiceOptions serviceOptions)
        {
            LeastConnection leastConnection = null;
            WeightRoundRobin weightRoundRobin = null;
            ServiceAddress serviceAddress = null;
            try
            {
                serviceAddress = await GetServices(serviceName, serviceOptions, leastConnection, weightRoundRobin);
                var result = await func(serviceAddress);
                weightRoundRobin?.AddWeight(serviceAddress);
                leastConnection?.ReLease(serviceAddress);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"服务【{serviceName}】调用异常");
                weightRoundRobin?.ReduceWeight(serviceAddress);
                throw ex;
            }
        }

        private async Task ServiceInvoke(string serviceName, Action<ServiceAddress> action, ServiceOptions serviceOptions)
        {
            LeastConnection leastConnection = null;
            WeightRoundRobin weightRoundRobin = null;
            ServiceAddress serviceAddress = null;
            try
            {
                serviceAddress = await GetServices(serviceName, serviceOptions, leastConnection, weightRoundRobin);
                action(serviceAddress);
                weightRoundRobin?.AddWeight(serviceAddress);
                leastConnection?.ReLease(serviceAddress);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"服务【{serviceName}】调用异常");
                weightRoundRobin?.ReduceWeight(serviceAddress);
                throw ex;
            }
        }

        private async Task<ServiceAddress> GetServices(string serviceName, ServiceOptions serviceOptions, LeastConnection leastConnection, WeightRoundRobin weightRoundRobin)
        {
            List<Service> services;
            if (serviceOptions == ServiceOptions.Grpc)
            {
                services = await _discoveryServiceProvider.GetGrpcServices(serviceName);
            }
            else
            {
                services = await _discoveryServiceProvider.GetHttpServices(serviceName);
            }

            if (services.Count == 0)
            {
                throw new ServiceNotFoundException(serviceName);
            }

            if (services.Count == 1)
            {
                return services[0].ServiceAddress;
            }

            LoadBalancerOptions loadBalancerOptions = LoadBalancerOptions.RoundRobin;
            ServiceTool.GetServiceTagConfig(services[0].Tags, ServiceTag.LOADBALANCEROPTION, loadBalancerOption => loadBalancerOptions = (LoadBalancerOptions)Enum.Parse(typeof(LoadBalancerOptions), loadBalancerOption));
            ServiceAddress serviceAddress = null;
            var loadBalancerConfig = _loadBalancerCache.Get(serviceName);
            if (loadBalancerConfig == null || loadBalancerConfig.LoadBalancerOptions != loadBalancerOptions)
            {
                loadBalancerConfig = new LoadBalancerConfig
                {
                    LoadBalancerOptions = loadBalancerOptions
                };
                switch (loadBalancerOptions)
                {
                    case LoadBalancerOptions.LeastConnection:
                        leastConnection = new LeastConnection(services);
                        loadBalancerConfig.LoadBalancer = leastConnection;
                        serviceAddress = leastConnection.GetServiceAddress();
                        break;
                    case LoadBalancerOptions.WeightRoundRobin:
                        weightRoundRobin = new WeightRoundRobin(services);
                        loadBalancerConfig.LoadBalancer = weightRoundRobin;
                        serviceAddress = weightRoundRobin.GetServiceAddress();
                        break;
                    default:
                        var roundRobin = new RoundRobin(services);
                        loadBalancerConfig.LoadBalancer = roundRobin;
                        serviceAddress = roundRobin.GetServiceAddress();
                        break;
                }

                _loadBalancerCache.Add(serviceName, loadBalancerConfig);
            }
            else
            {
                switch (loadBalancerOptions)
                {
                    case LoadBalancerOptions.LeastConnection:
                        leastConnection = loadBalancerConfig.LoadBalancer as LeastConnection;
                        serviceAddress = GetServiceAddress(leastConnection, services);
                        break;
                    case LoadBalancerOptions.WeightRoundRobin:
                        weightRoundRobin = loadBalancerConfig.LoadBalancer as WeightRoundRobin;
                        serviceAddress = GetServiceAddress(weightRoundRobin, services);
                        break;
                    default:
                        var roundRobin = loadBalancerConfig.LoadBalancer as RoundRobin;
                        serviceAddress = GetServiceAddress(roundRobin, services);
                        break;
                }
            }

            return serviceAddress;
        }

        private ServiceAddress GetServiceAddress(ILoadBalancer loadBalancer, List<Service> services)
        {
            loadBalancer.UpdateServices(services);
            return loadBalancer.GetServiceAddress();
        }
    }
}
