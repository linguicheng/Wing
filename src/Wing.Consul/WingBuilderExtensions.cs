using System;
using Consul;
using Microsoft.Extensions.DependencyInjection;
using Wing.Configuration;
using Wing.Configuration.ServiceBuilder;
using Wing.ServiceProvider;

namespace Wing.Consul
{
    public static class WingBuilderExtensions
    {
        public static IWingBuilder AddConsul(this IWingBuilder wingBuilder)
        {
            var discoveryConfig = wingBuilder.GetConfig<Config>("Consul");
            if (discoveryConfig == null)
            {
                throw new ArgumentNullException(nameof(discoveryConfig));
            }

            var consulClient = new ConsulClient(x => x.Address = new Uri(discoveryConfig.Url));
            IDiscoveryServiceProvider consulProvider;
            consulProvider = new ConsulProvider(consulClient);
            var consulBuilder = new ConsulBuilder(consulClient, consulProvider);
            if (discoveryConfig.Interval.GetValueOrDefault() > 0)
            {
                consulProvider = new IntervalConsulProvider(discoveryConfig.Interval.Value * 1000, consulProvider);
            }

            var service = discoveryConfig.Service;
            if (service != null)
            {
                if (service.Scheme == SchemeOptions.Http)
                {
                    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                }

                if (discoveryConfig.Service.Option == ServiceOptions.Http)
                {
                    consulBuilder.AddHttpHealthCheck(service).RegisterHttpService(service);
                }
                else
                {
                    consulBuilder.AddGRPCHealthCheck(service).RegisterGrpcService(service);
                }
            }
           
            wingBuilder.Services.AddSingleton(typeof(IDiscoveryServiceProvider), consulProvider);
            return wingBuilder;
        }
    }
}
