using Wing.Model;
using Wing.Persistence.APM;
using Wing.Result;

namespace Wing.Persistence.Apm
{
    public class TracerService : ITracerService
    {
        private readonly IFreeSql<WingDbFlag> _fsql;

        public TracerService(IFreeSql<WingDbFlag> fsql)
        {
            _fsql = fsql;
        }

        public async Task Add(TracerDto tracerDto)
        {
            using var uow = _fsql.CreateUnitOfWork();
            await uow.Orm.Insert(tracerDto.Tracer).ExecuteAffrowsAsync();
            await uow.Orm.Insert(tracerDto.HttpTracer).ExecuteAffrowsAsync();
            await uow.Orm.Insert(tracerDto.SqlTracer).ExecuteAffrowsAsync();
            await uow.Orm.Insert(tracerDto.HttpTracerDetails?.Values.ToList()).ExecuteAffrowsAsync();
            await uow.Orm.Insert(tracerDto.SqlTracerDetails?.Values.ToList()).ExecuteAffrowsAsync();
            uow.Commit();
        }

        public Task<bool> Any(string tracerId)
        {
            return _fsql.Select<Tracer>().AnyAsync(x => x.Id == tracerId);
        }

        public async Task<PageResult<List<Tracer>>> List(PageModel<TracerSearchDto> dto)
        {
            var result = await _fsql.Select<Tracer>()
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.ServiceName), u => u.ServiceName.Contains(dto.Data.ServiceName))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.ServiceUrl), u => u.ServiceUrl.Contains(dto.Data.ServiceUrl))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.RequestUrl), u => u.RequestUrl.Contains(dto.Data.RequestUrl))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.RequestValue), u => u.RequestValue.Contains(dto.Data.RequestValue))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.ResponseValue), u => u.ResponseValue.Contains(dto.Data.ResponseValue))
                    .WhereIf(dto.Data.RequestTime != null && dto.Data.RequestTime.Count == 2, u => u.RequestTime >= dto.Data.RequestTime[0] && u.RequestTime <= dto.Data.RequestTime[1])
                    .OrderByDescending(x => x.RequestTime)
                    .Count(out var total)
                    .Page(dto.PageIndex, dto.PageSize)
                    .ToListAsync();
            return new PageResult<List<Tracer>>
            {
                TotalCount = total,
                Items = result
            };
        }

        public async Task<PageResult<List<HttpTracer>>> List(PageModel<HttpTracerSearchDto> dto)
        {
            var result = await _fsql.Select<HttpTracer>()
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.ServiceName), u => u.ServiceName.Contains(dto.Data.ServiceName))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.ServiceUrl), u => u.ServiceUrl.Contains(dto.Data.ServiceUrl))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.RequestUrl), u => u.RequestUrl.Contains(dto.Data.RequestUrl))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.RequestValue), u => u.RequestValue.Contains(dto.Data.RequestValue))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.ResponseValue), u => u.ResponseValue.Contains(dto.Data.ResponseValue))
                    .WhereIf(dto.Data.RequestTime != null && dto.Data.RequestTime.Count == 2, u => u.RequestTime >= dto.Data.RequestTime[0] && u.RequestTime <= dto.Data.RequestTime[1])
                    .OrderByDescending(x => x.RequestTime)
                    .Count(out var total)
                    .Page(dto.PageIndex, dto.PageSize)
                    .ToListAsync();
            return new PageResult<List<HttpTracer>>
            {
                TotalCount = total,
                Items = result
            };
        }

        public async Task<PageResult<List<SqlTracer>>> List(PageModel<SqlTracerSearchDto> dto)
        {
            var result = await _fsql.Select<SqlTracer>()
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.ServiceName), u => u.ServiceName.Contains(dto.Data.ServiceName))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.ServiceUrl), u => u.ServiceUrl.Contains(dto.Data.ServiceUrl))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.Sql), u => u.Sql.Contains(dto.Data.Sql))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.Action), u => u.Action.Contains(dto.Data.Action))
                    .WhereIf(dto.Data.BeginTime != null && dto.Data.BeginTime.Count == 2, u => u.BeginTime >= dto.Data.BeginTime[0] && u.BeginTime <= dto.Data.BeginTime[1])
                    .OrderByDescending(x => x.BeginTime)
                    .Count(out var total)
                    .Page(dto.PageIndex, dto.PageSize)
                    .ToListAsync();
            return new PageResult<List<SqlTracer>>
            {
                TotalCount = total,
                Items = result
            };
        }

        public Task<List<HttpTracerDetail>> HttpDetail(string traceId)
        {
            return _fsql.Select<HttpTracerDetail>()
                .Where(u => u.TraceId == traceId)
                .ToListAsync();
        }

        public Task<List<SqlTracerDetail>> SqlDetail(string traceId)
        {
            return _fsql.Select<SqlTracerDetail>()
                .Where(u => u.TraceId == traceId)
                .ToListAsync();
        }

        public long TimeoutTotal()
        {
            return _fsql.Select<Tracer>()
                .Where(x => x.UsedMillSeconds >= Config.ApmTimeOut && x.RequestTime > Config.SearchTime)
                .Count();
        }
    }
}
