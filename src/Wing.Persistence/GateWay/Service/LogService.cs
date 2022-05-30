using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Wing.Model;
using Wing.Result;

namespace Wing.Persistence.GateWay
{
    public class LogService : ILogService
    {
        private readonly IFreeSql _fsql;

        public LogService(IFreeSql fsql)
        {
            _fsql = fsql;
        }

        public Task<int> Add(Log log)
        {
            return _fsql.Insert(log).ExecuteAffrowsAsync();
        }

        public Task<int> Add(IEnumerable<Log> logs)
        {
            return _fsql.Insert(logs).ExecuteAffrowsAsync();
        }

        public Task<bool> Any(string id)
        {
            return _fsql.Select<Log>().AnyAsync(x => x.Id == id);
        }

        public async Task<PageResult<List<Log>>> List(PageModel<LogSearchDto> dto)
        {
            var result = await _fsql.Select<Log>()
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.ServiceName), u => u.ServiceName.Contains(dto.Data.ServiceName))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.DownstreamUrl), u => u.DownstreamUrl.Contains(dto.Data.DownstreamUrl))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.RequestUrl), u => u.RequestUrl.Contains(dto.Data.RequestUrl))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.RequestValue), u => u.RequestValue.Contains(dto.Data.RequestValue))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.ResponseValue), u => u.ResponseValue.Contains(dto.Data.ResponseValue))
                    .WhereIf(dto.Data.RequestTime != null && dto.Data.RequestTime.Count == 2, u => u.RequestTime >= dto.Data.RequestTime[0] && u.RequestTime <= dto.Data.RequestTime[1])
                    .OrderByDescending(x => x.RequestTime)
                    .Count(out var total)
                    .Page(dto.PageIndex, dto.PageSize)
                    .ToListAsync();
            return new PageResult<List<Log>>
            {
                TotalCount = total,
                Items = result
            };
        }
    }
}
