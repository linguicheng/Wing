using System.Collections.Generic;
using System.Threading.Tasks;
using Wing.Model;
using Wing.Persistence.APM;
using Wing.Result;

namespace Wing.Persistence.Apm
{
    public interface ITracerService
    {
        Task<int> Add(TracerDto tracerDto);
    }
}
