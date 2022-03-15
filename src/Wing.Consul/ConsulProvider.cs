using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Wing.Convert;
using Wing.ServiceProvider;
using Wing.ServiceProvider.Config;
using consulHealthStatus = Consul.HealthStatus;
using wingHealthStatus = Wing.ServiceProvider.HealthStatus;

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

        public async Task<List<Service>> Get() => await Get(string.Empty, null, null);

        public async Task<List<Service>> Get(wingHealthStatus healthStatus) => await Get(string.Empty, null, healthStatus);

        public async Task<List<Service>> Get(string serviceName) => await Get(serviceName, null, null);

        public async Task<List<Service>> Get(string serviceName, wingHealthStatus healthStatus) => await Get(serviceName, null, healthStatus);

        public async Task<List<Service>> GetGrpcServices(string serviceName) => await Get(serviceName, ServiceOptions.Grpc, null);

        public async Task<List<Service>> GetHttpServices(string serviceName) => await Get(serviceName, ServiceOptions.Http, null);

        public async Task<List<Service>> GetGrpcServices(string serviceName, wingHealthStatus healthStatus) => await Get(serviceName, ServiceOptions.Grpc, healthStatus);

        public async Task<List<Service>> GetHttpServices(string serviceName, wingHealthStatus healthStatus) => await Get(serviceName, ServiceOptions.Http, healthStatus);

        public async Task<Dictionary<string, string>> GetKVData(string key, CancellationToken ct = default)
        {
            using var client = Connect();
            var kvResult = await client.KV.List(key, new QueryOptions
            {
                Token = _config.Token,
                Datacenter = _config.DataCenter,
                WaitIndex = LastIndex,
                WaitTime = TimeSpan.FromMinutes(_config.WaitTime)
            }, ct).ConfigureAwait(false);
            if (kvResult.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (kvResult.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"获取KV失败，状态码：{kvResult.StatusCode}");
            }

            var kvData = kvResult.Response;
            if (kvData == null || !kvData.Any())
            {
                return null;
            }

            var result = new Dictionary<string, string>();
            foreach (var kv in kvData)
            {
                result.Add(kv.Key, DataConverter.BytesToString(kv.Value));
            }

            return result;
        }

        public async Task GetKVData(Action<Dictionary<string, string>> setData, CancellationToken ct = default)
        {
            using var client = Connect();
            var result = await client.KV.List(_config.Service.ConfigKey, new QueryOptions
            {
                Token = _config.Token,
                Datacenter = _config.DataCenter,
                WaitIndex = LastIndex,
                WaitTime = TimeSpan.FromMinutes(_config.WaitTime)
            }, ct).ConfigureAwait(false);
            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception($"找不到配置Key【{_config.Service.ConfigKey}】的配置信息");
            }

            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"获取KV失败，状态码：{result.StatusCode}");
            }

            if (result.LastIndex > LastIndex)
            {
                LastIndex = result.LastIndex;
                var kvData = result.Response;
                if (kvData == null || !kvData.Any())
                {
                    throw new Exception($"配置Key【{_config.Service.ConfigKey}】的配置信息为空");
                }

                var data = kvData.Where(kv => !kv.Key.EndsWith("/") && kv.Value != null && kv.Value.Any())
                       .SelectMany(kv => DataConverter.BytesToDictionary(kv.Value).Select(pair =>
                             {
                                 var key = $"{kv.Key.RemoveStart(_config.Service.Name).TrimEnd('/').Replace('/', ':')}:{pair.Key}"
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

        public async Task<bool> Put(string key, byte[] value, CancellationToken ct = default)
        {
            using var client = Connect();
            var result = await client.KV.Put(new KVPair { Key = key, Value = value }, new WriteOptions
            {
                Token = _config.Token,
                Datacenter = _config.DataCenter
            }, ct).ConfigureAwait(false);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"保存KV失败，状态码：{result.StatusCode}");
            }

            return result.Response;
        }

        public async Task<bool> Delete(string key, CancellationToken ct = default)
        {
            var client = Connect();
            WriteResult<bool> result;
            if (key.EndsWith('/'))
            {
                result = await client.KV.DeleteTree(key, new WriteOptions
                {
                    Token = _config.Token,
                    Datacenter = _config.DataCenter
                }, ct).ConfigureAwait(false);
            }
            else
            {
                result = await client.KV.Delete(key, new WriteOptions
                {
                    Token = _config.Token,
                    Datacenter = _config.DataCenter
                }, ct).ConfigureAwait(false);
            }

            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"删除KV失败，状态码：{result.StatusCode}");
            }

            return result.Response;
        }

        public async Task Register()
        {
            var config = _config.Service;
            using var client = Connect();
            var services = await client.Agent.Services();
            foreach (var s in services.Response.Values)
            {
                if (s.Address == config.Host && s.Port == config.Port)
                {
                    await client.Agent.ServiceDeregister(s.ID);
                    break;
                }
            }

            List<AgentServiceCheck> checks = new List<AgentServiceCheck>();
            var healthcheck = config.HealthCheck;
            if (config.Option == ServiceOptions.Http)
            {
                checks.Add(new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(healthcheck.Interval ?? 10),
                    HTTP = healthcheck.Url,
                    Timeout = TimeSpan.FromSeconds(healthcheck.Timeout ?? 10)
                });
            }
            else
            {
                checks.Add(new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(healthcheck.Interval ?? 10),
                    GRPC = healthcheck.Url,
                    GRPCUseTLS = healthcheck.GRPCUseTLS.GetValueOrDefault(),
                    Timeout = TimeSpan.FromSeconds(healthcheck.Timeout ?? 10)
                });
            }

            var registration = new AgentServiceRegistration()
            {
                Checks = checks.ToArray(),
                ID = Guid.NewGuid().ToString(),
                Name = config.Name,
                Address = config.Host,
                Port = config.Port,
                Tags = BuildTags().ToArray()
            };
            await client.Agent.ServiceRegister(registration);
        }

        public async Task<bool> Deregister(string serviceId)
        {
            using var client = Connect();
            var result = await client.Agent.ServiceDeregister(serviceId);
            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<Service> Detail(string serviceId)
        {
            using var client = Connect();
            var checks = await client.Agent.Checks();
            var services = await client.Agent.Services();
            List<Service> result = new List<Service>();
            foreach (var s in services.Response.Values)
            {
                if (s.ID != serviceId)
                {
                    continue;
                }

                return BuildService(s, checks.Response);
            }

            return null;
        }

        private async Task<List<Service>> Get(string serviceName, ServiceOptions? serviceOption, wingHealthStatus? healthStatus)
        {
            using var client = Connect();
            var checks = await client.Agent.Checks();
            var services = await client.Agent.Services();
            List<Service> result = new List<Service>();
            foreach (var s in services.Response.Values)
            {
                var service = BuildService(s, checks.Response);
                if (serviceOption == null)
                {
                    if (string.IsNullOrWhiteSpace(serviceName))
                    {
                        if (healthStatus != null && healthStatus != service.Status)
                        {
                            continue;
                        }

                        result.Add(service);
                        continue;
                    }

                    if (s.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (healthStatus != null && healthStatus != service.Status)
                        {
                            continue;
                        }

                        result.Add(service);
                    }

                    continue;
                }

                if (service.ServiceOptions != serviceOption)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(serviceName))
                {
                    if (healthStatus != null && healthStatus != service.Status)
                    {
                        continue;
                    }

                    result.Add(service);
                    continue;
                }

                if (s.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase))
                {
                    if (healthStatus != null && healthStatus != service.Status)
                    {
                        continue;
                    }

                    result.Add(service);
                }
            }

            return result;
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

        private void ServiceTagSplit(IEnumerable<string> tags, string startsWith, Action<string> action)
        {
            foreach (var tag in tags)
            {
                if (tag.StartsWith($"{startsWith}:"))
                {
                    var tagArr = tag.Split(":");
                    if (tagArr.Length == 2)
                    {
                        action(tagArr[1]);
                        break;
                    }
                }
            }
        }

        private List<string> BuildTags()
        {
            var service = _config.Service;
            List<string> tags = new List<string>
            {
                $"{ServiceTag.SERVICE_OPTION}:{service.Option}",
                $"{ServiceTag.LOADBALANCER_OPTION}:{service.LoadBalancer.Option}",
                $"{ServiceTag.SCHEME}:{service.Scheme}",
                $"{ServiceTag.DEVELOPER}:{service.Developer}",
                $"{ServiceTag.CONFIG_KEY}:{service.ConfigKey}"
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

            return tags;
        }

        private Service BuildService(AgentService s, Dictionary<string, AgentCheck> checks)
        {
            var status = wingHealthStatus.Healthy;
            var checkService = checks.Values.Where(x => x.ServiceID == s.ID).FirstOrDefault();
            if (checkService != null)
            {
                if (checkService.Status.Equals(consulHealthStatus.Passing))
                {
                    status = wingHealthStatus.Healthy;
                }
                else if (checkService.Status.Equals(consulHealthStatus.Maintenance))
                {
                    status = wingHealthStatus.Maintenance;
                }
                else if (checkService.Status.Equals(consulHealthStatus.Warning))
                {
                    status = wingHealthStatus.Warning;
                }
                else
                {
                    status = wingHealthStatus.Critical;
                }
            }

            var scheme = string.Empty;
            ServiceTagSplit(s.Tags, ServiceTag.SCHEME, x => scheme = x);
            var service = new Service()
            {
                Id = s.ID,
                Name = s.Service,
                Status = status,
                ServiceAddress = new ServiceAddress(s.Address, s.Port, scheme)
            };
            ServiceTagSplit(s.Tags, ServiceTag.WEIGHT, x =>
            {
                int.TryParse(x, out int weight);
                service.EffectiveWeight = service.Weight = weight;
            });
            ServiceTagSplit(s.Tags, ServiceTag.SERVICE_OPTION, x =>
            {
                service.ServiceOptions = (ServiceOptions)Enum.Parse(typeof(ServiceOptions), x);
            });
            ServiceTagSplit(s.Tags, ServiceTag.DEVELOPER, x =>
            {
                service.Developer = x;
            });
            ServiceTagSplit(s.Tags, ServiceTag.CONFIG_KEY, x =>
            {
                service.ConfigKey = x;
            });
            ServiceTagSplit(s.Tags, ServiceTag.LOADBALANCER_OPTION, x =>
            {
                service.LoadBalancer = (LoadBalancerOptions)Enum.Parse(typeof(LoadBalancerOptions), x);
            });
            return service;
        }
    }
}
