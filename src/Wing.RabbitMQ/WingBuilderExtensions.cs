using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Wing.Configuration.ServiceBuilder;
using Wing.Converter;
using Wing.EventBus;
using Wing.RabbitMQ;

namespace Wing
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddEventBus(this IWingServiceBuilder wingBuilder)
        {
            return AddConfig(wingBuilder, App.GetConfig<Config>("RabbitMQ"));
        }

        public static IWingServiceBuilder AddEventBus(this IWingServiceBuilder wingBuilder, Config config)
        {
            return AddConfig(wingBuilder, config);
        }

        private static IWingServiceBuilder AddConfig(IWingServiceBuilder wingBuilder, Config config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var factory = new ConnectionFactory()
            {
                HostName = config.HostName,
                UserName = config.UserName,
                Password = config.Password,
                VirtualHost = config.VirtualHost,
                Port = config.Port
            };
            var eventBus = new RabbitMQEventBus(new RabbitMQConnection(factory),
                 new ConsumerConnection(factory),
                 App.GetRequiredService<IJson>(),
                 App.GetRequiredService<ILogger<RabbitMQEventBus>>(),
                 config);
            wingBuilder.Services.AddSingleton(typeof(IEventBus), eventBus);
            wingBuilder.AppBuilder += new WingStartupFilter().Configure(eventBus);
            return wingBuilder;
        }
    }
}
