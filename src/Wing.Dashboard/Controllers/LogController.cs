using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wing.Model;
using Wing.Persistence.GateWay;
using Wing.Result;

namespace Wing.Dashboard.Controllers
{
    public class LogController : BaseController
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public Task<PageResult<List<Log>>> List([FromQuery] PageModel<LogSearchDto> dto)
        {
           return _logService.List(dto);
        }
    }
}
