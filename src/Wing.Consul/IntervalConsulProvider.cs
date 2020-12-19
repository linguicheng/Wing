using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wing.Configuration;
using Wing.ServiceProvider;

namespace Wing.Consul
{
    public class IntervalConsulProvider : IDiscoveryServiceProvider, IDisposable
    {
        private List<Service> _services;
        private bool _wait = false;
        private Timer _timer;

        public IntervalConsulProvider(int interval, IDiscoveryServiceProvider discoveryServiceProvider)
        {
            _services = new List<Service>();
            _timer = new Timer(async x =>
             {
                 if (_wait)
                 {
                     return;
                 }

                 _wait = true;
                 _services = await discoveryServiceProvider.Get();
                 _wait = false;
             }, interval, 0, interval);
            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                Dispose();
            };
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
            return Task.FromResult(_services.Where(s => s.Name.Equals(serviceName, StringComparison.OrdinalIgnoreCase) && s.Tags.Contains(ServiceTag.GRPC)).ToList());
        }

        public Task<List<Service>> GetHttpServices(string serviceName)
        {
            return Task.FromResult(_services.Where(s => s.Name.Equals(serviceName, StringComparison.OrdinalIgnoreCase) && !s.Tags.Contains(ServiceTag.GRPC)).ToList());
        }
    }
}
