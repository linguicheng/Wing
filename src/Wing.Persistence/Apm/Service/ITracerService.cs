using System.Collections.Generic;
using System.Threading.Tasks;
using Wing.Persistence.APM;

namespace Wing.Persistence.Apm
{
    public interface ITracerService
    {
        Task Add(TracerDto tracerDto);
    }
}
