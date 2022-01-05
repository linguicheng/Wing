using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Wing.ServiceProvider;

namespace Wing.Configuration
{
    public class WingConfigurationProvider : ConfigurationProvider, IObserver
    {
        public override void Load()
        {
            try
            {
                ServiceUtils.DiscoveryService.GetKVData(SetData)
                        .ConfigureAwait(false)
                        .GetAwaiter()
                        .GetResult();
            }
            catch (Exception ex)
            {
                ServiceLocator.GetRequiredService<ILogger<WingConfigurationProvider>>()
                     .LogError(ex, "配置中心获取配置信息异常");
            }
        }

        public void SetData(Dictionary<string, string> configData)
        {
            Data = configData;
        }
    }
}
