using System;
using Microsoft.Extensions.DependencyInjection;
using Wing.Cache;
using Wing.Configuration.ServiceBuilder;

namespace Wing.Redis
{
    public static class WingBuilderExtensions
    {
        public static IWingServiceBuilder AddRedis(this IWingServiceBuilder wingBuilder)
        {
            return AddConfig(wingBuilder, wingBuilder.GetConfig<Config>("Redis"));
        }

        public static IWingServiceBuilder AddRedis(this IWingServiceBuilder wingBuilder, Config config)
        {
            return AddConfig(wingBuilder, config);
        }

        private static IWingServiceBuilder AddConfig(IWingServiceBuilder wingBuilder, Config config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            wingBuilder.Services.AddSingleton(typeof(ICache), new RedisProvider(config));
            return wingBuilder;
        }
    }
}
