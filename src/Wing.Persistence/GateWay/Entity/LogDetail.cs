using FreeSql.DataAnnotations;

namespace Wing.Persistence.Gateway
{
    public class LogDetail
    {
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        public string LogId { get; set; }

        public string ServiceName { get; set; }

        public string RequestUrl { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServiceAddress { get; set; }

        public DateTime RequestTime { get; set; }

        public string RequestMethod { get; set; }

        public DateTime ResponseTime { get; set; }

        public string ResponseValue { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        public long UsedMillSeconds { get; set; }

        public int StatusCode { get; set; }

        /// <summary>
        /// 聚合Key
        /// </summary>
        public string Key { get; set; }

        public string Policy { get; set; }

        public string Exception { get; set; }
    }
}
