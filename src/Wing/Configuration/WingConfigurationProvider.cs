using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Wing.Configuration
{
    public class WingConfigurationProvider : ConfigurationProvider, IObserver
    {
        public override void Load()
        {
            ServiceLocator.DiscoveryService.GetKVData(SetData)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
        }

        public void SetData(Dictionary<string, string> configData)
        {
            Data = configData;
        }
    }
}
