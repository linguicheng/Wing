using System.Collections.Generic;
using System.Threading.Tasks;
using Wing.Model;
using Wing.Result;

namespace Wing.Persistence.Gateway
{
    public interface ILogService
    {
        Task<int> Add(Log log);

        Task<int> Add(IEnumerable<Log> logs);

        Task<bool> Any(string id);

        Task<PageResult<List<Log>>> List(PageModel<LogSearchDto> model);

        long TimeoutTotal();

        Task<List<Log>> TimeoutList();

        Task<List<MonthCountDto>> TimeoutMonth();
    }
}
