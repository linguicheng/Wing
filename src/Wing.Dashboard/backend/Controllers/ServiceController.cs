using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wing.Dashboard.Model;
using Wing.Dashboard.Result;
using Wing.ServiceProvider;

namespace Wing.Dashboard.Controllers
{
    public class ServiceController : BaseController
    {

        private readonly IDiscoveryServiceProvider _discoveryService;

        public ServiceController()
        {
            _discoveryService = ServiceLocator.DiscoveryService;
        }

        [HttpGet]
        public async Task<PageResult<ServiceDetailOutputDto>> Detail([FromQuery] PageModel<ServiceSearchDto> dto)
        {
            if (dto.Data == null)
            {
                throw new Exception("请求参数错误");
            }

            List<Service> services;
            if (dto.Data.Status == null)
            {
                services = await _discoveryService.Get();
            }
            else
            {
                services = await _discoveryService.Get(dto.Data.Status.Value);
            }

            PageResult<ServiceDetailOutputDto> result = new PageResult<ServiceDetailOutputDto>
            {
                TotalCount = services.Count
            };
            result.Items = new List<ServiceDetailOutputDto>();
            foreach (var s in services)
            {
                if (result.Items.Count >= dto.PageSize * dto.PageIndex)
                {
                    break;
                }
                if (!string.IsNullOrWhiteSpace(dto.Data.Name) && !s.Name.Contains(dto.Data.Name))
                {
                    continue;
                }

                if (dto.Data.ServiceType != null && dto.Data.ServiceType != s.ServiceOptions)
                {
                    continue;
                }

                if (dto.Data.LoadBalancer != null && dto.Data.LoadBalancer != s.LoadBalancer)
                {
                    continue;
                }
                result.Items.Add(new ServiceDetailOutputDto
                {
                    Id = s.Id,
                    Address = s.ServiceAddress.ToString(),
                    Name = s.Name,
                    Weight = s.Weight,
                    ServiceType = s.ServiceOptions,
                    LoadBalancer = s.LoadBalancer,
                    Status = s.Status,
                    Developer = s.Developer
                });
            }
            return result;
        }

        [HttpGet]
        public async Task<PageResult<ServiceOutputDto>> List([FromQuery] PageModel<string> dto)
        {
            var serviceDetail = await _discoveryService.Get();

            var services = serviceDetail.GroupBy(u => new { u.Name })
                                        .Select(x => new ServiceOutputDto
                                        {
                                            Name = x.Key.Name,
                                            Total = x.Count(),
                                            CriticalTotal = x.Count(y => y.Status == HealthStatus.Critical),
                                            HealthyTotal = x.Count(y => y.Status == HealthStatus.Healthy),
                                            MaintenanceTotal = x.Count(y => y.Status == HealthStatus.Maintenance),
                                            WarningTotal = x.Count(y => y.Status == HealthStatus.Warning)
                                        });

            PageResult<ServiceOutputDto> result = new PageResult<ServiceOutputDto>
            {
                TotalCount = services.Count()
            };
            result.Items = new List<ServiceOutputDto>();
            foreach (var s in services)
            {
                if (result.Items.Count >= dto.PageSize * dto.PageIndex)
                {
                    break;
                }
                if (!string.IsNullOrWhiteSpace(dto.Data) && !s.Name.Contains(dto.Data))
                {
                    continue;
                }
                s.CriticalLv = Math.Round(s.CriticalTotal * 100.0 / s.Total, 2);
                s.HealthyLv = Math.Round(s.HealthyTotal * 100.0 / s.Total, 2);
                s.MaintenanceLv = Math.Round(s.MaintenanceTotal * 100.0 / s.Total, 2);
                s.WarningLv = Math.Round(s.WarningTotal * 100.0 / s.Total, 2);
                result.Items.Add(s);
            }
            return result;
        }

        [HttpGet]
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
    }
}
