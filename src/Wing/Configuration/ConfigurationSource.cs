using Microsoft.Extensions.Configuration;

namespace Wing.Configuration
{
    public class ConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            var provider = new WingConfigurationProvider();
            ConfigurationSubject.Add(provider);
            return provider;
        }
    }
}
