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

        Task<List<Service>> Get(string serviceName);

        Task<List<Service>> GetGrpcServices(string serviceName);

        Task<List<Service>> GetHttpServices(string serviceName);

        Task GetKVData(Action<Dictionary<string, string>> setData, CancellationToken ct = default);

        Task Deregister(string serviceId);

        Task Register(ServiceData service);
    }
}
