using Microsoft.AspNetCore.Http;
using System;

namespace Wing.GateWay
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

        public Policy Policy { get; set; }

        public DateTime RequestTime { get; set; }

        public DateTime ResponseTime { get; set; }
    }
}
