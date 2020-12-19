using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Consul;
using Wing.Configuration;
using Wing.ServiceProvider;

namespace Wing.Consul
{
    public class ConsulProvider : IDiscoveryServiceProvider
    {
        private readonly ConsulClient _client;

        public ConsulProvider(ConsulClient client)
        {
            _client = client;
        }

        public async Task<List<Service>> Get()
        {
            var services = await _client.Agent.Services();
            List<Service> result = new List<Service>();
            foreach (var s in services.Response.Values)
            {
                result.Add(new Service(s.ID, s.Service, s.Tags, new ServiceAddress(s.Address, s.Port, s.Tags)));
            }

            return result;
        }

        public async Task<List<Service>> Get(string serviceName)
        {
            var services = await _client.Agent.Services();
            List<Service> result = new List<Service>();
            foreach (var s in services.Response.Values)
            {
                if (s.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(new Service(s.ID, s.Service, s.Tags, new ServiceAddress(s.Address, s.Port, s.Tags)));
                }
            }

            return result;
        }

        public async Task<List<Service>> GetGrpcServices(string serviceName)
        {
            var services = await _client.Agent.Services();
            List<Service> result = new List<Service>();
            foreach (var s in services.Response.Values)
            {
                if (s.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase) && Array.IndexOf(s.Tags, ServiceTag.GRPC) >= 0)
                {
                    result.Add(new Service(s.ID, s.Service, s.Tags, new ServiceAddress(s.Address, s.Port, s.Tags)));
                }
            }

            return result;
        }

        public async Task<List<Service>> GetHttpServices(string serviceName)
        {
            var services = await _client.Agent.Services();
            List<Service> result = new List<Service>();
            foreach (var s in services.Response.Values)
            {
                if (s.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase) && Array.IndexOf(s.Tags, ServiceTag.GRPC) < 0)
                {
                    result.Add(new Service(s.ID, s.Service, s.Tags, new ServiceAddress(s.Address, s.Port, s.Tags)));
                }
            }

            return result;
        }
    }
}
