using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Wing.Gateway
{
    public class ServiceContext
    {
        public ServiceContext(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }

        public HttpContext HttpContext { get; }

        public string ServiceName { get; set; }

        public Config.Route Route { get; set; }

        public string UpstreamPath { get; set; }

        public string DownstreamPath { get; set; }

        public Config.Policy Policy { get; set; }

        public DateTime RequestTime { get; set; }

        public string ServiceAddress { get; set; }

        public int StatusCode { get; set; }

        public string RequestValue { get; set; }

        public string ResponseValue { get; set; }

        public bool IsWebSocket { get; set; } = false;

        public string Exception { get; set; }
    }
}
