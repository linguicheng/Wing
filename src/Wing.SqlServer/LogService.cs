using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wing.Persistence.GateWay;

namespace Wing.SqlServer
{
    public class LogService : ILogService
    {
        private readonly GateWayDbContext _context;
        public LogService(GateWayDbContext context)
        {
            _context = context;
        }
        public async Task Add(Log log)
        {
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task Add(IEnumerable<Log> logs)
        {
            _context.Logs.AddRange(logs);
            await _context.SaveChangesAsync();
        }
    }
}
