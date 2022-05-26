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
            Expression<Func<Log, bool>> where = u => true;
            if (!string.IsNullOrWhiteSpace(dto.Data.ServiceName))
            {
                where = where.And(u => u.ServiceName.Contains(dto.Data.ServiceName));
            }

            if (!string.IsNullOrWhiteSpace(dto.Data.DownstreamUrl))
            {
                where = where.And(u => u.DownstreamUrl.Contains(dto.Data.DownstreamUrl));
            }

            if (!string.IsNullOrWhiteSpace(dto.Data.RequestUrl))
            {
                where = where.And(u => u.RequestUrl.Contains(dto.Data.RequestUrl));
            }

            if (!string.IsNullOrWhiteSpace(dto.Data.RequestValue))
            {
                where = where.And(u => u.RequestValue.Contains(dto.Data.RequestValue));
            }

            if (!string.IsNullOrWhiteSpace(dto.Data.ResponseValue))
            {
                where = where.And(u => u.ResponseValue.Contains(dto.Data.ResponseValue));
            }

            if (dto.Data.RequestTime != null && dto.Data.RequestTime.Count == 2)
            {
                where = where.And(u => u.RequestTime >= dto.Data.RequestTime[0] && u.RequestTime <= dto.Data.RequestTime[1]);
            }

            var result = await _fsql.Select<Log>()
                                    .Where(where)
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
