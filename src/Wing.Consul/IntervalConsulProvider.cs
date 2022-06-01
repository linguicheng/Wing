using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wing.ServiceProvider;
using Wing.ServiceProvider.Config;

namespace Wing.Consul
{
    public class IntervalConsulProvider : IDiscoveryServiceProvider, IDisposable
    {
        private static readonly object _lock = new object();
        private readonly IDiscoveryServiceProvider _discoveryServiceProvider;
        private readonly Timer _timer;
        private List<Service> _services;
        private bool _wait = false;

        public IntervalConsulProvider(int interval, IDiscoveryServiceProvider discoveryServiceProvider)
        {
            _discoveryServiceProvider = discoveryServiceProvider;
            _services = new List<Service>();
            _timer = new Timer(x =>
             {
                 lock (_lock)
                 {
                     if (_wait)
                     {
                         return;
                     }

                     _wait = true;
                     try
                     {
                         _services = _discoveryServiceProvider.Get().ConfigureAwait(false).GetAwaiter().GetResult();
                     }
                     catch
                     {
                     }

                     _wait = false;
                 }

             }, null, 0, interval);
        }

        public async Task<bool> Deregister(string serviceId)
        {
            return await _discoveryServiceProvider.Deregister(serviceId);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task<List<Service>> Get()
        {
            return Task.FromResult(_services);
        }

        public Task<List<Service>> Get(string serviceName)
        {
            return Task.FromResult(_services.Where(s => s.Name.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).ToList());
        }

        public Task<List<Service>> GetGrpcServices(string serviceName)
        {
            return Task.FromResult(_services.Where(s => s.Name.Equals(serviceName, StringComparison.OrdinalIgnoreCase) && s.ServiceOptions == ServiceOptions.Grpc).ToList());
        }

        public Task<List<Service>> GetHttpServices(string serviceName)
        {
            return Task.FromResult(_services.Where(s => s.Name.Equals(serviceName, StringComparison.OrdinalIgnoreCase) && s.ServiceOptions == ServiceOptions.Http).ToList());
        }

        public async Task GetKVData(Action<Dictionary<string, string>> setData, CancellationToken ct = default)
        {
            await _discoveryServiceProvider.GetKVData(setData, ct);
        }

        public async Task Register()
        {
            await _discoveryServiceProvider.Register();
        }

        public Task<List<Service>> Get(HealthStatus healthStatus)
        {
            return Task.FromResult(_services.Where(s => s.Status == healthStatus).ToList());
        }

        public Task<List<Service>> Get(string serviceName, HealthStatus healthStatus)
        {
            return Task.FromResult(_services.Where(s => s.Name.Equals(serviceName, StringComparison.OrdinalIgnoreCase)
                                    && s.Status == healthStatus).ToList());
        }

        public Task<List<Service>> GetGrpcServices(string serviceName, HealthStatus healthStatus)
        {
            return Task.FromResult(_services.Where(s => s.Name.Equals(serviceName, StringComparison.OrdinalIgnoreCase)
                                    && s.ServiceOptions == ServiceOptions.Grpc
                                    && s.Status == healthStatus).ToList());
        }

        public Task<List<Service>> GetHttpServices(string serviceName, HealthStatus healthStatus)
        {
            return Task.FromResult(_services.Where(s => s.Name.Equals(serviceName, StringComparison.OrdinalIgnoreCase)
                                    && s.ServiceOptions == ServiceOptions.Http
                                    && s.Status == healthStatus).ToList());
        }

        public async Task<Service> Detail(string serviceId)
        {
            return await _discoveryServiceProvider.Detail(serviceId);
        }

        public async Task<Dictionary<string, string>> GetKVData(string key, CancellationToken ct = default)
        {
            return await _discoveryServiceProvider.GetKVData(key, ct);
        }

        public async Task<bool> Put(string key, byte[] value, CancellationToken ct = default)
        {
            return await _discoveryServiceProvider.Put(key, value, ct);
        }

        public async Task<bool> Delete(string key, CancellationToken ct = default)
        {
            return await _discoveryServiceProvider.Delete(key, ct);
        }
    }
}
