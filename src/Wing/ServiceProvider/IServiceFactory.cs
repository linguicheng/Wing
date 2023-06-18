using System;
using System.Threading.Tasks;

namespace Wing.ServiceProvider
{
    public interface IServiceFactory
    {
        T Invoke<T>(string serviceName, Func<ServiceAddress, T> func);

        void Invoke(string serviceName, Action<ServiceAddress> action);

        Task<T> InvokeAsync<T>(string serviceName, Func<ServiceAddress, Task<T>> func);

        Task InvokeAsync(string serviceName, Func<ServiceAddress, Task> func);
    }
}
