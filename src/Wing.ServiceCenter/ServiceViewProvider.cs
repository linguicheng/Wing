using Wing.Model;
using Wing.Result;
using Wing.ServiceCenter.Service;
using Wing.ServiceProvider;
using Wing.ServiceProvider.Dto;

namespace Wing.ServiceCenter
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
            var serviceDetail = await _discoveryService.Get();

            return serviceDetail.GroupBy(u => new { u.Name })
                                        .Select(x => new ServiceCriticalDto
                                        {
                                            ServiceName = x.Key.Name,
                                            Total = x.Count(),
                                            CriticalTotal = x.Count(y => y.Status == HealthStatus.Critical),
                                            CriticalLv = Math.Round(x.Count(y => y.Status == HealthStatus.Critical) * 100.0 / x.Count(), 2)
                                        })
                                        .Where(x => x.CriticalLv > 0)
                                        .OrderByDescending(x => x.CriticalLv)
                                        .ToList();
        }

        public async Task<bool> Delete(string serviceId)
        {
            var service = await _discoveryService.Detail(serviceId);
            if (service == null)
            {
                throw new Exception("该服务节点不存在");
            }

            if (service.Status != HealthStatus.Critical)
            {
                throw new Exception("仅能删除状态为“已死亡”的服务节点");
            }

            return await _discoveryService.Deregister(serviceId);
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
