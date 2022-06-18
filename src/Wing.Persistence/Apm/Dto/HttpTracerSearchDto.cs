using System;
using System.Collections.Generic;

namespace Wing.Persistence.Apm
{
    public class HttpTracerSearchDto
    {

        public string ServerIp { get; set; }

        public string ServiceName { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// Grpc or Http
        /// </summary>
        public string RequestType { get; set; }

        public string RequestUrl { get; set; }

        public List<DateTime> RequestTime { get; set; }

        public string RequestMethod { get; set; }

        public string RequestValue { get; set; }

        public string ResponseValue { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        public long UsedMillSeconds { get; set; }

        public int? StatusCode { get; set; }

        public string Exception { get; set; }
    }
}
