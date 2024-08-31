using Microsoft.AspNetCore.Mvc;
using Wing.Model;
using Wing.Result;
using Wing.ServiceProvider;
using Wing.ServiceProvider.Dto;

namespace Wing.UI.Controllers
{
    public class ConfigController : BaseController
    {
        private readonly IConfigViewProvider _configViewProvider;

        public ConfigController()
        {
            _configViewProvider = App.ConfigViewProvider;
        }

        [HttpGet]
        public async Task<Dictionary<string, string>> Get(string key)
        {
            return await _configViewProvider.Get(key);
        }

        [HttpGet]
        public async Task<PageResult<Dictionary<string, string>>> List([FromQuery] PageModel<string> dto)
        {
            return await _configViewProvider.List(dto);
        }

        [HttpPost]
        public async Task<bool> Save(List<ConfigDto> configDtos)
        {
            return await _configViewProvider.Save(configDtos);
        }

        [HttpGet]
        public async Task<bool> Delete(string key)
        {
            return await _configViewProvider.Delete(key);
        }
    }
}
