using Wing.Model;
using Wing.Persistence.GateWay;
using Wing.Result;

namespace Wing.Persistence.Gateway
{
    public class LogService : ILogService
    {
        private readonly IFreeSql<WingDbFlag> _fsql;

        public LogService(IFreeSql<WingDbFlag> fsql)
        {
            _fsql = fsql;
        }

        public async Task<int> Add(LogAddDto logDto)
        {
            var result = await _fsql.Insert(logDto.Log).ExecuteAffrowsAsync();
            if (result < 1)
            {
                return result;
            }

            if (logDto.LogDetails != null && logDto.LogDetails.Count > 0)
            {
                result = await _fsql.Insert(logDto.LogDetails).ExecuteAffrowsAsync();
            }

            return result;
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

        public long TimeoutTotal()
        {
            return _fsql.Select<Log>().Where(x => x.UsedMillSeconds >= Config.GatewayTimeOut && x.RequestTime > Config.SearchTime).Count();
        }

        /// <summary>
        /// 查询最近一个月超时请求
        /// </summary>
        /// <returns></returns>
        public Task<List<Log>> TimeoutList()
        {
            return _fsql.Select<Log>()
                .Where(x => x.UsedMillSeconds >= Config.GatewayTimeOut && x.RequestTime > Config.SearchTime)
                .OrderByDescending(x => x.UsedMillSeconds)
                .ToListAsync();
        }

        public async Task<List<MonthCountDto>> TimeoutMonth()
        {
            var beginDate = Convert.ToDateTime(DateTime.Now.AddYears(-1).ToString("yyyy-MM") + "-01");
            var endDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM") + "-01");
            var result = await _fsql.Select<Log>()
                .Where(x => x.UsedMillSeconds >= Config.GatewayTimeOut && x.RequestTime >= beginDate && x.RequestTime < endDate)
                .GroupBy(x => new { Month = x.RequestTime.ToString("yyyy-MM") })
                .ToListAsync(x => new MonthCountDto
                {
                    Month = x.Key.Month,
                    Count = x.Count(),
                });
            var data = new List<MonthCountDto>
            {
                new MonthCountDto { Month = beginDate.ToString("yyyy-MM"), Count = 0 }
            };

            for (var i = 1; i < 12; i++)
            {
                data.Add(new MonthCountDto { Month = beginDate.AddMonths(i).ToString("yyyy-MM"), Count = 0 });
            }

            if (result == null)
            {
                return data;
            }

            if (result.Count != 12)
            {
                data.ForEach(x =>
                {
                    if (!result.Any(m => m.Month == x.Month))
                    {
                        result.Add(x);
                    }
                });
            }

            result = result.OrderBy(x => x.Month).ToList();
            return result;
        }
    }
}
