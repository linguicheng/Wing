using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wing.Model;
using Wing.Persistence.Apm;
using Wing.Result;

namespace Wing.UI.Controllers
{
    public class TracerController : BaseController
    {
        private readonly ITracerService _tracerService;

        public TracerController(ITracerService tracerService)
        {
            _tracerService = tracerService;
        }

        [HttpGet]
        public Task<PageResult<List<Tracer>>> List([FromQuery] PageModel<TracerSearchDto> dto)
        {
            return _tracerService.List(dto);
        }

        [HttpGet]
        public Task<PageResult<List<HttpTracer>>> HttpList([FromQuery] PageModel<HttpTracerSearchDto> dto)
        {
            return _tracerService.List(dto);
        }

        [HttpGet]
        public Task<PageResult<List<SqlTracer>>> SqlList([FromQuery] PageModel<SqlTracerSearchDto> dto)
        {
            return _tracerService.List(dto);
        }

        [HttpGet]
        public Task<List<HttpTracerDetail>> GetHttpDetail(string traceId)
        {
            return _tracerService.HttpDetail(traceId);
        }

        [HttpGet]
        public Task<List<SqlTracerDetail>> GetSqlDetail(string traceId)
        {
            return _tracerService.SqlDetail(traceId);
        }
    }
}
