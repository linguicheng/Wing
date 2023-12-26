using Microsoft.AspNetCore.Mvc;
using Wing.Model;
using Wing.Persistence.Saga;
using Wing.Result;

namespace Wing.UI.Controllers
{
    public class SagaController : BaseController
    {
        private readonly ISagaTranService _tranService;
        private readonly ISagaTranUnitService _tranUnitService;

        public SagaController(ISagaTranService tranService, ISagaTranUnitService tranUnitService)
        {
            _tranService = tranService;
            _tranUnitService = tranUnitService;
        }

        [HttpGet]
        public Task<PageResult<List<SagaTran>>> List([FromQuery] PageModel<SagaTranSearchDto> dto)
        {
            return _tranService.List(dto);
        }

        [HttpGet]
        public Task<List<SagaTranUnit>> Detail(string tranId)
        {
            return _tranUnitService.List(tranId);
        }

        [HttpGet]
        public Task<List<SagaTranStatusCount>> FailedDataRanking()
        {
            return _tranService.FailedDataRanking();
        }
    }
}
