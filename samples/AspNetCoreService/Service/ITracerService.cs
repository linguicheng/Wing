using System.Threading.Tasks;

namespace Sample.AspNetCoreService
{
    public interface ITracerService
    {
        Task Add(Tracer tracer);
    }
}
