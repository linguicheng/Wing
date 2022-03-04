using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wing.Persistence.GateWay
{
    public interface ILogService
    {
        Task Add(Log log);
        Task Add(IEnumerable<Log> logs);
    }
}
