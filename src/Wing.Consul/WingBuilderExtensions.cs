using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wing.Configuration;
using Wing.Configuration.ServiceBuilder;
using Wing.Consul;
using Wing.ServiceProvider;
using Wing.ServiceProvider.Config;

namespace Wing
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

            App.CurrentService = discoveryConfig.Service;
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

                if (!string.IsNullOrWhiteSpace(service.Host))
                {
                    consulProvider.Register();
                }

                service.HealthCheck ??= new Healthcheck();
                if (string.IsNullOrWhiteSpace(service.HealthCheck.Url))
                {
                    if (service.Option == ServiceOptions.Http)
                    {
                        service.HealthCheck.Url = $"{service.Scheme}://{service.Host}:{service.Port}/health";
                    }
                    else
                    {
                        service.HealthCheck.Url = $"{service.Host}:{service.Port}";
                    }
                }
                service.HealthCheck.Interval ??= 10;
                service.HealthCheck.Timeout ??= 10;
            }

            App.DiscoveryService = consulProvider;
            App.ServiceViewProvider = new ServiceViewProvider();
            var configCenterEnabled = configration["ConfigCenterEnabled"];
            if (configCenterEnabled != "False")
            {
                builder.Add(new ConfigurationSource());
            }

            return builder;
        }
    }
}
