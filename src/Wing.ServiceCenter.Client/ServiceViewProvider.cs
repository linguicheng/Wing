using Wing.Model;
using Wing.Result;
using Wing.ServiceCenter.Client.Service;
using Wing.ServiceProvider;
using Wing.ServiceProvider.Dto;

namespace Wing.ServiceCenter.Client
{
    public class ServiceViewProvider : IServiceViewProvider
    {
        private readonly RegisterService _registerService;

        public ServiceViewProvider(RegisterService registerService)
        {
            _registerService = registerService;
        }

        public async Task<List<ServiceCriticalDto>> CritiCalLvRanking()
        {
            return await _registerService.CritiCalLvRanking();
        }

        public async Task<bool> Delete(string serviceId)
        {
            var service = await _registerService.Detail(serviceId);
            if (service == null)
            {
                throw new Exception("该服务节点不存在");
            }

            if (service.Status != HealthStatus.Critical)
            {
                throw new Exception("仅能删除状态为“已死亡”的服务节点");
            }

            return await _registerService.Delete(serviceId);
        }

        public async Task<PageResult<List<ServiceDetailDto>>> Detail(PageModel<ServiceSearchDto> dto)
        {
           return await _registerService.Detail(dto);
        }

        public async Task<PageResult<List<ServiceDto>>> List(PageModel<string> dto)
        {
            return await _registerService.List(dto);
        }
    }
}
