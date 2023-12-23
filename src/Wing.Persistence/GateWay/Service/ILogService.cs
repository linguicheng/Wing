using Wing.Model;
using Wing.Persistence.GateWay;
using Wing.Result;

namespace Wing.Persistence.Gateway
{
    public interface ILogService
    {
        Task<int> Add(LogAddDto logDto);

        Task<bool> Any(string id);

        Task<PageResult<List<Log>>> List(PageModel<LogSearchDto> model);

        Task<List<LogDetail>> DetailList(string logId);

        long TimeoutTotal();

        Task<List<Log>> TimeoutList();

        Task<List<MonthCountDto>> TimeoutMonth();
    }
}
