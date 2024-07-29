namespace Wing.APM.Persistence
{
    public class TracerSearchDto
    {
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServiceUrl { get; set; }

        public List<DateTime> RequestTime { get; set; }

        /// <summary>
        /// Grpc or Http
        /// </summary>
        public string RequestType { get; set; }

        public string RequestUrl { get; set; }

        public string RequestMethod { get; set; }

        public string RequestValue { get; set; }

        /// <summary>
        /// 客户端请求IP
        /// </summary>
        public string ClientIp { get; set; }

        public string ResponseValue { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        public long UsedMillSeconds { get; set; }

        public int? StatusCode { get; set; }

        public string Exception { get; set; }
    }
}
