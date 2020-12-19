using System;
using Microsoft.Extensions.DependencyInjection;
using Wing.Cache;
using Wing.Configuration.ServiceBuilder;

namespace Wing.Redis
{
    public static class WingBuilderExtensions
    {
        public static IWingBuilder AddRedis(this IWingBuilder wingBuilder)
        {
            return AddConfig(wingBuilder, wingBuilder.GetConfig<Config>("Redis"));
        }

        public static IWingBuilder AddRedis(this IWingBuilder wingBuilder, Config config)
        {
            return AddConfig(wingBuilder, config);
        }

        private static IWingBuilder AddConfig(IWingBuilder wingBuilder, Config config)
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
