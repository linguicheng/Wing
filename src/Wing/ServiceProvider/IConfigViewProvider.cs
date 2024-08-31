using Microsoft.AspNetCore.Mvc;
using Wing.Model;
using Wing.Result;
using Wing.ServiceProvider.Dto;

namespace Wing.ServiceProvider
{
    public interface IConfigViewProvider
    {
        Task<Dictionary<string, string>> Get(string key);

        Task<PageResult<Dictionary<string, string>>> List([FromQuery] PageModel<string> dto);

        Task<bool> Save(List<ConfigDto> configDtos);

        Task<bool> Delete(string key);
    }
}
