namespace Wing.ServiceProvider
{
    public interface IServiceFactory
    {
        T Invoke<T>(string serviceName, Func<ServiceAddress, T> func);

        T Invoke<T>(string serviceName, string key, Func<ServiceAddress, T> func);

        void Invoke(string serviceName, Action<ServiceAddress> action);

        void Invoke(string serviceName, string key, Action<ServiceAddress> action);

        Task<T> InvokeAsync<T>(string serviceName, Func<ServiceAddress, Task<T>> func);

        Task<T> InvokeAsync<T>(string serviceName, string key, Func<ServiceAddress, Task<T>> func);

        Task InvokeAsync(string serviceName, Func<ServiceAddress, Task> func);

        Task InvokeAsync(string serviceName, string key, Func<ServiceAddress, Task> func);
    }
}
