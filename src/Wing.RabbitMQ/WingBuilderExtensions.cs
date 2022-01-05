using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Wing.Configuration.ServiceBuilder;
using Wing.Convert;
using Wing.DynamicMethod;
using Wing.EventBus;

namespace Wing.RabbitMQ
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddRabbitMQ(this IWingServiceBuilder wingBuilder)
        {
            return AddConfig(wingBuilder, wingBuilder.GetConfig<Config>("RabbitMQ"));
        }

        public static IWingServiceBuilder AddRabbitMQ(this IWingServiceBuilder wingBuilder, Config config)
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

            wingBuilder.Services.AddSingleton(typeof(IEventBus), serviceProvider =>
            {
                var eventBus = new RabbitMQEventBus(new RabbitMQConnection(factory),
                 new ConsumerConnection(factory),
                 serviceProvider.GetRequiredService<IJson>(),
                 serviceProvider.GetRequiredService<ILogger<RabbitMQEventBus>>(),
                 config);
                GlobalInjection.CreateSubscribe(eventBus);
                return eventBus;
            });
            return wingBuilder;
        }
    }
}
