using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
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

        public List<string> FirstWildcardMatchPath { get; set; }

        public List<string> LastWildcardMatchPath { get; set; }

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

        public Dictionary<string, StringValues> ResponseHeaders { get; set; }

        public Stream ResponseStream { get; set; }

        public bool IsFile { get; set; }

        public bool IsWebSocket { get; set; } = false;

        public string Exception { get; set; }

        public bool IsReadRequestBody { get; set; } = false;

        public string ContentType { get; set; }

        /// <summary>
        /// 请求超时，单位：秒
        /// </summary>
        public double TimeOut { get; set; }

        /// <summary>
        /// 聚合服务
        /// </summary>
        public List<DownstreamService> DownstreamServices { get; set; }

        /// <summary>
        /// 权限认证
        /// </summary>
        public Func<IEnumerable<Downstream>, HttpContext, Task<bool>> Authorization { get; set; }

        /// <summary>
        /// 请求下游服务之前请求参数的回调
        /// </summary>
        public Func<RequestData, Task<RequestData>> RequestBefore { get; set; }

        /// <summary>
        /// 请求下游服务之后返回值的回调
        /// </summary>
        public Func<ResponseData, Task<ResponseData>> ResponseAfter { get; set; }
    }

    public class RequestData
    {
        public string ServiceName { get; set; }

        public string DownstreamPath { get; set; }

        public Dictionary<string, IEnumerable<string>> Headers { get; set; }

        public Dictionary<string, StringValues> QueryParams { get; set; }

        public byte[] Body { get; set; }

        public bool RequestBreak { get; set; } = false;

        public string ResponseValue { get; set; }

        public int StatusCode { get; set; } = 200;
    }

    public class ResponseData
    {
        public string ServiceName { get; set; }

        public string DownstreamPath { get; set; }

        public Dictionary<string, IEnumerable<string>> Headers { get; set; }

        public string Body { get; set; }
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
