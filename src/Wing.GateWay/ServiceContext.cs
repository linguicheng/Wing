using Microsoft.AspNetCore.Http;
using Wing.Gateway.Config;

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

        /// <summary>
        /// 服务聚合路由和自定义路由
        /// </summary>
        public Route Route { get; set; }

        public string TemplateParameterName { get; set; }

        public string TemplateParameterValue { get; set; }

        public string UpstreamPath { get; set; }

        public string DownstreamPath { get; set; }

        public string Method { get; set; }

        public Policy Policy { get; set; }

        public DateTime RequestTime { get; set; }

        public string ServiceAddress { get; set; }

        public int StatusCode { get; set; }

        public string RequestValue { get; set; }

        public string ResponseValue { get; set; }

        public bool IsWebSocket { get; set; } = false;

        public string Exception { get; set; }

        public bool IsReadRequestBody { get; set; } = false;

        /// <summary>
        /// 聚合服务
        /// </summary>
        public List<DownstreamService> DownstreamServices { get; set; }

        public Func<IEnumerable<Downstream>, HttpContext, Task<bool>> Authorization { get; set; }
    }

    public class DownstreamService
    {
        public Downstream Downstream { get; set; }

        public Policy Policy { get; set; }

        public DateTime RequestTime { get; set; }

        public string ServiceAddress { get; set; }

        public int StatusCode { get; set; }

        public string RequestValue { get; set; }

        public string ResponseValue { get; set; }

        public string Exception { get; set; }
    }
}
