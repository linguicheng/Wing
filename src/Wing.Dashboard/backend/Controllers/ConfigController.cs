using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wing.Convert;
using Wing.Dashboard.Helper;
using Wing.Dashboard.Model;
using Wing.Dashboard.Result;
using Wing.ServiceProvider;

namespace Wing.Dashboard.Controllers
{
    public class ConfigController : BaseController
    {
        private readonly IDiscoveryServiceProvider _discoveryService;

        public ConfigController()
        {
            _discoveryService = ServiceLocator.DiscoveryService;
        }

        [HttpGet]
        public async Task<Dictionary<string, string>> Get(string key)
        {
            return await _discoveryService.GetKVData(key);
        }

        [HttpGet]
        public async Task<PageResult<Dictionary<string, string>>> List([FromQuery] PageModel<string> dto)
        {
            var kvs = await _discoveryService.GetKVData(dto.Data);
            return kvs.ToPage(null, dto, (kv, result) => result.Add(kv.Key, kv.Value));
        }

        [HttpPost]
        public async Task<bool> Save(List<ConfigDto> configDtos)
        {
            foreach (var configDto in configDtos)
            {
                var value = DataConverter.StringToBytes(configDto.Value);
                if (value != null)
                {
                    DataConverter.BuildConfig(value);
                }
                await _discoveryService.Put(configDto.Key, value);
            }
            return true;
        }

        [HttpGet]
        public async Task<bool> Delete(string key)
        {
            return await _discoveryService.Delete(key);
        }
    }
}
