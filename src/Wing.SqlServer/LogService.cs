using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wing.Extensions;
using Wing.Model;
using Wing.Persistence.GateWay;
using Wing.Result;

namespace Wing.SqlServer
{
    public class LogService : ILogService
    {
        private readonly GateWayDbContext _context;

        public LogService(GateWayDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(Log log)
        {
            _context.Logs.Add(log);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Add(IEnumerable<Log> logs)
        {
            _context.Logs.AddRange(logs);
            return await _context.SaveChangesAsync();
        }

        public async Task<PageResult<List<Log>>> List(PageModel<LogSearchDto> dto)
        {
            Expression<Func<Log, bool>> where = u => true;
            if (!string.IsNullOrWhiteSpace(dto.Data.ServiceName))
            {
                where.And(u => u.ServiceName.Contains(dto.Data.ServiceName));
            }

            if (!string.IsNullOrWhiteSpace(dto.Data.DownstreamUrl))
            {
                where.And(u => u.DownstreamUrl.Contains(dto.Data.DownstreamUrl));
            }

            if (!string.IsNullOrWhiteSpace(dto.Data.RequestUrl))
            {
                where.And(u => u.RequestUrl.Contains(dto.Data.RequestUrl));
            }

            if (!string.IsNullOrWhiteSpace(dto.Data.RequestValue))
            {
                where.And(u => u.RequestValue.Contains(dto.Data.RequestValue));
            }

            if (!string.IsNullOrWhiteSpace(dto.Data.ResponseValue))
            {
                where.And(u => u.ResponseValue.Contains(dto.Data.ResponseValue));
            }

            if (dto.Data.RequestTimeBegin != null)
            {
                where.And(u => u.RequestTime >= dto.Data.RequestTimeBegin.GetValueOrDefault().Date);
            }

            if (dto.Data.RequestTimeEnd != null)
            {
                where.And(u => u.RequestTime < dto.Data.RequestTimeEnd.GetValueOrDefault().Date.AddDays(1));
            }

            return new PageResult<List<Log>>
            {
                TotalCount = await _context.Logs.AsNoTracking().CountAsync(),
                Items = await _context.Logs.AsNoTracking()
                        .Where(where)
                        .Skip(dto.PageSize * (dto.PageIndex - 1))
                        .Take(dto.PageSize)
                        .ToListAsync()
            };
        }
    }
}
