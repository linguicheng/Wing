using System;
using System.Threading.Tasks;

namespace Wing.ServiceProvider
{
    public interface IServiceFactory
    {
        Task<T> GrpcServiceInvoke<T>(string serviceName, Func<ServiceAddress, Task<T>> func);

        Task<T> HttpServiceInvoke<T>(string serviceName, Func<ServiceAddress, Task<T>> func);

        Task GrpcServiceInvoke(string serviceName, Action<ServiceAddress> action);

        Task HttpServiceInvoke(string serviceName, Action<ServiceAddress> action);
    }
}
