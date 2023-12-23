using Microsoft.AspNetCore.Mvc;
using Wing.Converter;
using Wing.Model;
using Wing.Result;
using Wing.ServiceProvider;
using Wing.UI.Helper;
using Wing.UI.Model;

namespace Wing.UI.Controllers
{
    public class ConfigController : BaseController
    {
        private readonly IDiscoveryServiceProvider _discoveryService;

        public ConfigController()
        {
            _discoveryService = App.DiscoveryService;
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
