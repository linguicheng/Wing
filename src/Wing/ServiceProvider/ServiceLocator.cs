using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Wing.ServiceProvider.Config;

namespace Wing.ServiceProvider
{
    public class ServiceLocator
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static IDiscoveryServiceProvider DiscoveryService { get; set; }

        public static ServiceData CurrentService { get; set; }

        public static object GetService(Type serviceType)
        {
            return ServiceProvider.GetService(serviceType);
        }

        public static T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }

        public static IEnumerable<T> GetServices<T>()
        {
            return ServiceProvider.GetServices<T>();
        }

        public static object GetRequiredService(Type serviceType)
        {
            return ServiceProvider.GetRequiredService(serviceType);
        }

        public static T GetRequiredService<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        public static IEnumerable<object> GetServices(Type serviceType)
        {
            return ServiceProvider.GetServices(serviceType);
        }
    }
}
