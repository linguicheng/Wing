using System.Collections.Generic;
using System.Threading.Tasks;
using Wing.Model;
using Wing.Result;

namespace Wing.Persistence.GateWay
{
    public interface ILogService
    {
        Task<int> Add(Log log);

        Task<int> Add(IEnumerable<Log> logs);

        Task<PageResult<List<Log>>> List(PageModel<LogSearchDto> model);
    }
}
