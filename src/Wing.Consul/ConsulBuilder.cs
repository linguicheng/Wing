using Consul;
using System;
using System.Collections.Generic;
using Wing.ServiceProvider;
using System.Linq;
using Wing.Configuration;

namespace Wing.Consul
{
    internal class ConsulBuilder
    {
        private readonly ConsulClient _client;
        private readonly List<AgentServiceCheck> _checks = new List<AgentServiceCheck>();
        private readonly IDiscoveryServiceProvider _discoveryServiceProvider;

        public ConsulBuilder(ConsulClient client, IDiscoveryServiceProvider discoveryServiceProvider)
        {
            _client = client;
            _discoveryServiceProvider = discoveryServiceProvider;
        }

        public ConsulBuilder AddHealthCheck(AgentServiceCheck check)
        {
            _checks.Add(check);
            return this;
        }

        public ConsulBuilder AddHttpHealthCheck(ServiceConfig service)
        {
            var healthcheck = service.HealthCheck;
            _checks.Add(new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(healthcheck.RemoveService ?? 20),
                Interval = TimeSpan.FromSeconds(healthcheck.Interval ?? 10),
                HTTP = healthcheck.Url,
                Timeout = TimeSpan.FromSeconds(healthcheck.Timeout ?? 10)
            });
            return this;
        }

        public ConsulBuilder AddGRPCHealthCheck(ServiceConfig service)
        {
            var healthcheck = service.HealthCheck;
            _checks.Add(new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(healthcheck.RemoveService ?? 20),
                Interval = TimeSpan.FromSeconds(healthcheck.Interval ?? 10),
                GRPC = healthcheck.Url,
                GRPCUseTLS = healthcheck.GRPCUseTLS.GetValueOrDefault(),
                Timeout = TimeSpan.FromSeconds(healthcheck.Timeout ?? 10)
            });
            return this;
        }

        public void RegisterGrpcService(ServiceConfig service)
        {
            List<string> tags = new List<string>
            {
                ServiceTag.GRPC,
                $"{ServiceTag.LOADBALANCEROPTION}:{service.LoadBalancer.Option}",
                $"{ServiceTag.SCHEME}:{service.Scheme}"
            };
            int weight = service.LoadBalancer.Weight ?? 0;
            if (weight > 0)
            {
                tags.Add($"{ServiceTag.WEIGHT}:{weight}");
            }

            if (!string.IsNullOrWhiteSpace(service.Tag))
            {
                var tagArr = service.Tag.Split(",");
                foreach (var item in tagArr)
                {
                    tags.Add(item);
                }
            }

            RegisterService(service.Name, service.Host, service.Port, tags.ToArray());
        }

        public void RegisterHttpService(ServiceConfig service)
        {
            List<string> tags = new List<string>
            {
                $"{ServiceTag.LOADBALANCEROPTION}:{service.LoadBalancer.Option}",
                $"{ServiceTag.SCHEME}:{service.Scheme}"
            };
            int weight = service.LoadBalancer.Weight ?? 0;
            if (weight > 0)
            {
                tags.Add($"{ServiceTag.WEIGHT}:{weight}");
            }

            if (!string.IsNullOrWhiteSpace(service.Tag))
            {
                var tagArr = service.Tag.Split(",");
                foreach (var item in tagArr)
                {
                    if (item.Equals(ServiceTag.GRPC, StringComparison.OrdinalIgnoreCase))
                    {
                        tags.Add(item);
                    }
                }
            }

            RegisterService(service.Name, service.Host, service.Port, tags.ToArray());
        }

        private void RegisterService(string name, string host, int port, string[] tags)
        {
            var service = _discoveryServiceProvider.Get().Result.Where(s => s.ServiceAddress.Host == host && s.ServiceAddress.Port == port).FirstOrDefault();
            if (service != null)
            {
                _client.Agent.ServiceDeregister(service.Id);
            }

            var registration = new AgentServiceRegistration()
            {
                Checks = _checks.ToArray(),
                ID = Guid.NewGuid().ToString(),
                Name = name,
                Address = host,
                Port = port,
                Tags = tags
            };

            _client.Agent.ServiceRegister(registration).Wait();

            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                try
                {
                    _client.Agent.ServiceDeregister(registration.ID).Wait();
                }
                catch { }
            };
        }
    }
}
