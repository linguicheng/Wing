using System;
using System.Threading.Tasks;

namespace Wing.ServiceProvider
{
    public interface IServiceFactory
    {
        T GrpcServiceInvoke<T>(string serviceName, Func<ServiceAddress, T> func);

        T HttpServiceInvoke<T>(string serviceName, Func<ServiceAddress, T> func);

        void GrpcServiceInvoke(string serviceName, Action<ServiceAddress> action);

        void HttpServiceInvoke(string serviceName, Action<ServiceAddress> action);

        Task<T> GrpcServiceInvokeAsync<T>(string serviceName, Func<ServiceAddress, Task<T>> func);

        Task<T> HttpServiceInvokeAsync<T>(string serviceName, Func<ServiceAddress, Task<T>> func);

        Task GrpcServiceInvokeAsync(string serviceName, Action<ServiceAddress> action);

        Task HttpServiceInvokeAsync(string serviceName, Action<ServiceAddress> action);
    }
}
