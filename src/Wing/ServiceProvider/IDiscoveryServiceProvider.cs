using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Wing.Configuration;
using Wing.ServiceProvider.Config;

namespace Wing.ServiceProvider
{
    public interface IDiscoveryServiceProvider
    {
        Task<List<Service>> Get();

        Task<List<Service>> Get(HealthStatus healthStatus);

        Task<List<Service>> Get(string serviceName);

        Task<Service> Detail(string serviceId);

        Task<List<Service>> Get(string serviceName, HealthStatus healthStatus);

        Task<List<Service>> GetGrpcServices(string serviceName);

        Task<List<Service>> GetHttpServices(string serviceName);

        Task<List<Service>> GetGrpcServices(string serviceName, HealthStatus healthStatus);

        Task<List<Service>> GetHttpServices(string serviceName, HealthStatus healthStatus);

        Task<Dictionary<string, string>> GetKVData(string serviceName, CancellationToken ct = default);

        Task GetKVData(Action<Dictionary<string, string>> setData, CancellationToken ct = default);

        Task<bool> Put(string key, byte[] value, CancellationToken ct = default);

        Task<bool> Delete(string key, CancellationToken ct = default);

        Task<bool> Deregister(string serviceId);

        Task Register(ServiceData service);
    }
}
