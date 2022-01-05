using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Wing.Configuration;
using Wing.Convert;
using Wing.ServiceProvider;
using Wing.ServiceProvider.Config;

namespace Wing.Consul
{
    public class ConsulProvider : IDiscoveryServiceProvider
    {
        private readonly Provider _config;
        private ulong LastIndex { get; set; }

        public ConsulProvider(Provider config)
        {
            _config = config;
        }

        private ConsulClient Connect()
        {
            return new ConsulClient(x =>
            {
                x.Address = new Uri(_config.Url);
                x.Token = _config.Token;
                x.Datacenter = _config.DataCenter;
            });
        }

        public async Task<List<Service>> Get()
        {
            using var client = Connect();
            var services = await client.Agent.Services();
            List<Service> result = new List<Service>();
            foreach (var s in services.Response.Values)
            {
                result.Add(new Service(s.ID, s.Service, s.Tags, new ServiceAddress(s.Address, s.Port, s.Tags)));
            }
            return result;
        }

        public async Task<List<Service>> Get(string serviceName)
        {
            using var client = Connect();
            var services = await client.Agent.Services();
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
            return await Get(serviceName, ServiceOptions.Grpc);
        }

        public async Task<List<Service>> GetHttpServices(string serviceName)
        {
            return await Get(serviceName, ServiceOptions.Http);
        }

        private async Task<List<Service>> Get(string serviceName, ServiceOptions serviceOption)
        {
            using var client = Connect();
            var services = await client.Agent.Services();
            List<Service> result = new List<Service>();
            foreach (var s in services.Response.Values)
            {
                var tag = s.Tags.Where(u => u == $"{nameof(ServiceOptions)}:{serviceOption}").FirstOrDefault();
                if (s.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(tag))
                {
                    result.Add(new Service(s.ID, s.Service, s.Tags, new ServiceAddress(s.Address, s.Port, s.Tags)));
                }
            }

            return result;
        }

        public async Task GetKVData(Action<Dictionary<string, string>> setData, CancellationToken ct = default)
        {
            using var client = Connect();
            var result = await client.KV.List(_config.Key, new QueryOptions
            {
                Token = _config.Token,
                Datacenter = _config.DataCenter,
                WaitIndex = LastIndex,
                WaitTime = TimeSpan.FromMinutes(_config.WaitTime)
            }, ct).ConfigureAwait(false);
            if (result.StatusCode != HttpStatusCode.OK)
                throw new Exception($"获取KV失败，状态码：{result.StatusCode}");
            if (result.LastIndex > LastIndex)
            {
                LastIndex = result.LastIndex;
                var kvData = result.Response;
                if (kvData == null || !kvData.Any())
                {
                    setData(null);
                    return;
                }
                var data = kvData.Where(kv => !kv.Key.EndsWith("/") && kv.Value != null && kv.Value.Any())
                       .SelectMany(kv => DataConverter.BytesToDictionary(kv.Value).Select(pair =>
                             {
                                 var key = $"{kv.Key.RemoveStart(_config.Key).TrimEnd('/').Replace('/', ':')}:{pair.Key}"
                                     .Trim(':');
                                 if (string.IsNullOrEmpty(key))
                                 {
                                     throw new ArgumentNullException("key");
                                 }

                                 return new KeyValuePair<string, string>(key, pair.Value);
                             }))
                       .ToDictionary(kv => kv.Key, kv => kv.Value);
                setData(data);
            }
        }

        public async Task Register(ServiceData config)
        {
            using var client = Connect();
            List<AgentServiceCheck> _checks = new List<AgentServiceCheck>();
            var healthcheck = config.HealthCheck;
            if (config.Option == ServiceOptions.Http)
            {
                _checks.Add(new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(healthcheck.RemoveService ?? 20),
                    Interval = TimeSpan.FromSeconds(healthcheck.Interval ?? 10),
                    HTTP = healthcheck.Url,
                    Timeout = TimeSpan.FromSeconds(healthcheck.Timeout ?? 10)
                });
            }
            else
            {
                _checks.Add(new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(healthcheck.RemoveService ?? 20),
                    Interval = TimeSpan.FromSeconds(healthcheck.Interval ?? 10),
                    GRPC = healthcheck.Url,
                    GRPCUseTLS = healthcheck.GRPCUseTLS.GetValueOrDefault(),
                    Timeout = TimeSpan.FromSeconds(healthcheck.Timeout ?? 10)
                });
            }
            var services = await client.Agent.Services();
            foreach (var s in services.Response.Values)
            {
                if (s.Address == config.Host && s.Port == config.Port)
                {
                    await client.Agent.ServiceDeregister(s.ID);
                    break;
                }
            }
            var registration = new AgentServiceRegistration()
            {
                Checks = _checks.ToArray(),
                ID = Guid.NewGuid().ToString(),
                Name = config.Name,
                Address = config.Host,
                Port = config.Port,
                Tags = BuildTags(config).ToArray()
            };
            await client.Agent.ServiceRegister(registration);
        }

        public async Task Deregister(string serviceId)
        {
            using var client = Connect();
            await client.Agent.ServiceDeregister(serviceId);
        }

        private List<string> BuildTags(ServiceData service)
        {
            List<string> tags = new List<string>
            {
                $"{nameof(ServiceOptions)}:{service.Option}",
                $"{ServiceDefaults.LOADBALANCEROPTION}:{service.LoadBalancer.Option}",
                $"{ServiceDefaults.SCHEME}:{service.Scheme}"
            };
            int weight = service.LoadBalancer.Weight ?? 0;
            if (weight > 0)
            {
                tags.Add($"{ServiceDefaults.WEIGHT}:{weight}");
            }

            if (!string.IsNullOrWhiteSpace(service.Tag))
            {
                var tagArr = service.Tag.Split(",");
                foreach (var item in tagArr)
                {
                    tags.Add(item);
                }
            }
            return tags;
        }
    }
}
