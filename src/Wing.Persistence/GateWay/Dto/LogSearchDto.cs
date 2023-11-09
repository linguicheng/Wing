using System;
using System.Collections.Generic;

namespace Wing.Persistence.Gateway
{
    public class LogSearchDto
    {
        public string ServiceName { get; set; }

        public string DownstreamUrl { get; set; }

        public string RequestUrl { get; set; }

        /// <summary>
        /// 网关服务器IP
        /// </summary>
        public string GateWayServerIp { get; set; }

        /// <summary>
        /// 客户端请求IP
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServiceAddress { get; set; }

        public List<DateTime> RequestTime { get; set; }

        public string RequestMethod { get; set; }

        public string RequestValue { get; set; }

        public string ResponseValue { get; set; }

        public int StatusCode { get; set; }

        public string Policy { get; set; }

        public string AuthKey { get; set; }

        public string Token { get; set; }

        public string Exception { get; set; }
    }
}
