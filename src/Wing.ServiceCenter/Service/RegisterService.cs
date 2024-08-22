using Wing.Model;
using Wing.Result;
using Wing.ServiceProvider.Dto;

namespace Wing.ServiceCenter.Service
{
    public class RegisterService
    {
        private readonly IFreeSql<WingDbFlag> _fsql;

        public RegisterService(IFreeSql<WingDbFlag> fsql)
        {
            _fsql = fsql;
        }

        public async Task<int> Add(Entity.Service service)
        {
            var exists = await _fsql.Select<Entity.Service>().AnyAsync(x => x.Host == service.Host && x.Port == service.Port);
            if (exists)
            {
                await _fsql.Delete<Entity.Service>()
                    .Where(x => x.Host == service.Host && x.Port == service.Port)
                    .ExecuteAffrowsAsync();
            }

            return await _fsql.Insert(service).ExecuteAffrowsAsync();
        }

        public async Task<PageResult<List<ServiceDetailDto>>> Detail(PageModel<ServiceSearchDto> dto)
        {
            var result = await _fsql.Select<Entity.Service>()
                    .WhereIf(dto.Data.Status != null, u => u.Status == dto.Data.Status)
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.Name), u => u.Name.Contains(dto.Data.Name))
                    .WhereIf(dto.Data.ServiceType != null, u => u.ServiceType == dto.Data.ServiceType)
                    .WhereIf(dto.Data.LoadBalancer != null, u => u.LoadBalancer == dto.Data.LoadBalancer)
                    .Count(out var total)
                    .Page(dto.PageIndex, dto.PageSize)
                    .ToListAsync<ServiceDetailDto>();
            if (result != null)
            {
                result.ForEach(x => x.Address = $"{x.Scheme}://{x.Host}:{x.Port}");
            }

            return new PageResult<List<ServiceDetailDto>>
            {
                TotalCount = total,
                Items = result
            };
        }
    }
}
