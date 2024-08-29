using Microsoft.AspNetCore.Mvc;
using Wing.Model;
using Wing.Result;
using Wing.ServiceProvider;
using Wing.ServiceProvider.Dto;

namespace Wing.UI.Controllers
{
    public class ServiceController : BaseController
    {
        private readonly IServiceViewProvider _serviceViewProvider;

        public ServiceController()
        {
            _serviceViewProvider = App.ServiceViewProvider;
        }

        [HttpGet]
        public async Task<PageResult<List<ServiceDetailDto>>> Detail([FromQuery] PageModel<ServiceSearchDto> dto)
        {
            if (dto.Data == null)
            {
                throw new Exception("请求参数错误");
            }

            return await _serviceViewProvider.Detail(dto);
        }

        [HttpGet]
        public async Task<PageResult<List<ServiceDto>>> List([FromQuery] PageModel<string> dto)
        {
            return await _serviceViewProvider.List(dto);
        }

        [HttpGet]
        public async Task<bool> Delete(string serviceId)
        {
            return await _serviceViewProvider.Delete(serviceId);
        }

        /// <summary>
        /// 服务死亡率排行
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ServiceCriticalDto>> CritiCalLvRanking()
        {
            return await _serviceViewProvider.CritiCalLvRanking();
        }
    }
}
