using System;
using Microsoft.Extensions.Configuration;
using Wing.Configuration;
using Wing.ServiceProvider;
using Wing.ServiceProvider.Config;

namespace Wing.Consul
{
    public static class WingBuilderExtensions
    {
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder)
        {
            var configration = builder.Build();
            var discoveryConfig = configration.GetSection("Consul").Get<Provider>();
            if (discoveryConfig == null)
            {
                throw new ArgumentNullException(nameof(discoveryConfig));
            }

            IDiscoveryServiceProvider consulProvider;
            consulProvider = new ConsulProvider(discoveryConfig);
            if (discoveryConfig.Interval.GetValueOrDefault() > 0)
            {
                consulProvider = new IntervalConsulProvider(discoveryConfig.Interval.Value * 1000, consulProvider);
            }
            var service = discoveryConfig.Service;
            if (service != null)
            {
                if (string.IsNullOrWhiteSpace(service.ConfigKey))
                {
                    service.ConfigKey = service.Name;
                }
                if (service.Scheme == "http")
                {
                    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                }
                consulProvider.Register();
            }
            ServiceLocator.DiscoveryService = consulProvider;
            var configCenterEnabled = configration["ConfigCenterEnabled"];
            if (configCenterEnabled!="False")
            {
                builder.Add(new ConfigurationSource());
            }
            
            return builder;
        }
    }
}
