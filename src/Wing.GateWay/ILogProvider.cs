using Microsoft.AspNetCore.Http;
using Wing.Persistence.GateWay;

namespace Wing.Gateway
{
    public interface ILogProvider
    {
        Task Add(ServiceContext serviceContext);

        Task Add(LogAddDto logDto, HttpContext httpContext);
    }
}
