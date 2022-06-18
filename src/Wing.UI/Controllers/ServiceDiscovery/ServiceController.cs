using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wing.Model;
using Wing.Result;
using Wing.ServiceProvider;
using Wing.UI.Helper;
using Wing.UI.Model;

namespace Wing.UI.Controllers
{
    public class ServiceController : BaseController
    {
        private readonly IDiscoveryServiceProvider _discoveryService;

        public ServiceController()
        {
            _discoveryService = ServiceLocator.DiscoveryService;
        }

        [HttpGet]
        public async Task<PageResult<List<ServiceDetailDto>>> Detail([FromQuery] PageModel<ServiceSearchDto> dto)
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

            return services.ToPage<Service, ServiceDetailDto>(s =>
            {
                if (!string.IsNullOrWhiteSpace(dto.Data.Name) && !s.Name.Contains(dto.Data.Name))
                {
                    return false;
                }

                if (dto.Data.ServiceType != null && dto.Data.ServiceType != s.ServiceOptions)
                {
                    return false;
                }

                if (dto.Data.LoadBalancer != null && dto.Data.LoadBalancer != s.LoadBalancer)
                {
                    return false;
                }

                return true;
            }, dto,
            (s, result) =>
            {
                result.Add(new ServiceDetailDto
                {
                    Id = s.Id,
                    Address = s.ServiceAddress.ToString(),
                    Name = s.Name,
                    Weight = s.Weight,
                    ServiceType = s.ServiceOptions,
                    LoadBalancer = s.LoadBalancer,
                    Status = s.Status,
                    Developer = s.Developer,
                    ConfigKey = s.ConfigKey
                });
            });
        }

        [HttpGet]
        public async Task<PageResult<List<ServiceDto>>> List([FromQuery] PageModel<string> dto)
        {
            var serviceDetail = await _discoveryService.Get();

            var services = serviceDetail.GroupBy(u => new { u.Name })
                                        .Select(x => new ServiceDto
                                        {
                                            Name = x.Key.Name,
                                            Total = x.Count(),
                                            CriticalTotal = x.Count(y => y.Status == HealthStatus.Critical),
                                            HealthyTotal = x.Count(y => y.Status == HealthStatus.Healthy),
                                            MaintenanceTotal = x.Count(y => y.Status == HealthStatus.Maintenance),
                                            WarningTotal = x.Count(y => y.Status == HealthStatus.Warning)
                                        }).ToList();
            return services.ToPage<ServiceDto, ServiceDto>(s =>
            {
                if (!string.IsNullOrWhiteSpace(dto.Data) && !s.Name.Contains(dto.Data))
                {
                    return false;
                }

                return true;
            }, dto,
            (s, result) =>
            {
                s.CriticalLv = Math.Round(s.CriticalTotal * 100.0 / s.Total, 2);
                s.HealthyLv = Math.Round(s.HealthyTotal * 100.0 / s.Total, 2);
                s.MaintenanceLv = Math.Round(s.MaintenanceTotal * 100.0 / s.Total, 2);
                s.WarningLv = Math.Round(s.WarningTotal * 100.0 / s.Total, 2);
                result.Add(s);
            });
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
