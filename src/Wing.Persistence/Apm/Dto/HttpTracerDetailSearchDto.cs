using System;

namespace Wing.Persistence.Apm
{
    public class HttpTracerDetailSearchDto
    {
        public string TraceId { get; set; }

        /// <summary>
        /// Grpc or Http
        /// </summary>
        public string RequestType { get; set; }

        public string RequestUrl { get; set; }

        public DateTime RequestTime { get; set; }

        public string RequestMethod { get; set; }

        public string RequestValue { get; set; }

        public DateTime ResponseTime { get; set; }

        public string ResponseValue { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        public long UsedMillSeconds { get; set; }

        public int? StatusCode { get; set; }

        public string Exception { get; set; }
    }
}
