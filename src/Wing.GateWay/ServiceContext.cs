using Microsoft.AspNetCore.Http;

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

        public string DownstreamPath { get; set; }

        public Config.Policy Policy { get; set; }

        public DateTime RequestTime { get; set; }

        public string ServiceAddress { get; set; }

        public int StatusCode { get; set; }

        public string RequestValue { get; set; }

        public string ResponseValue { get; set; }

        public bool IsWebSocket { get; set; } = false;
    }
}
