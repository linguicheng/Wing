using Microsoft.AspNetCore.Mvc;
using Wing.Model;
using Wing.Persistence.Gateway;
using Wing.Result;

namespace Wing.UI.Controllers
{
    public class GatewayController : BaseController
    {
        private readonly ILogService _logService;

        public GatewayController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public Task<PageResult<List<Log>>> List([FromQuery] PageModel<LogSearchDto> dto)
        {
           return _logService.List(dto);
        }

        [HttpGet]
        public Task<List<LogDetail>> DetailList(string logId)
        {
            return _logService.DetailList(logId);
        }

        [HttpGet]
        public Task<List<Log>> TimeoutList()
        {
            return _logService.TimeoutList();
        }

        [HttpGet]
        public Task<List<MonthCountDto>> TimeoutMonth()
        {
            return _logService.TimeoutMonth();
        }
    }
}
