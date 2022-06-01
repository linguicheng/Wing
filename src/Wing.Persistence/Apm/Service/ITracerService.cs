using System.Collections.Generic;
using System.Threading.Tasks;
using Wing.Model;
using Wing.Persistence.APM;
using Wing.Result;

namespace Wing.Persistence.Apm
{
    public interface ITracerService
    {
        Task Add(TracerDto tracerDto);

        Task<bool> Any(string tracerId);

        Task<PageResult<List<Tracer>>> List(PageModel<TracerSearchDto> dto);

        Task<List<HttpTracerDetail>> HttpDetail(HttpTracerDetailSearchDto dto);
    }
}
