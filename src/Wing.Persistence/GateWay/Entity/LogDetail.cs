using FreeSql.DataAnnotations;

namespace Wing.Persistence.Gateway
{
    [Table(Name = "GateWay_LogDetail")]
    [Index("IX_ReqTime", "RequestTime", false)]
    [Index("IX_Name", "ServiceName", false)]
    public class LogDetail
    {
        [Column(IsPrimary = true, StringLength = 50)]
        public string Id { get; set; }

        [Column(StringLength = 50)]
        public string LogId { get; set; }

        [Column(StringLength = 200)]
        public string ServiceName { get; set; }

        [Column(StringLength = -1)]
        public string RequestUrl { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        [Column(StringLength = 200)]
        public string ServiceAddress { get; set; }

        public DateTime RequestTime { get; set; }

        [Column(StringLength = 20)]
        public string RequestMethod { get; set; }

        public DateTime ResponseTime { get; set; }

        [Column(StringLength = -1)]
        public string ResponseValue { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        public long UsedMillSeconds { get; set; }

        public int StatusCode { get; set; }

        /// <summary>
        /// 聚合Key
        /// </summary>
        [Column(StringLength = 100)]
        public string Key { get; set; }

        [Column(StringLength = -1)]
        public string Policy { get; set; }

        [Column(StringLength = -1)]
        public string Exception { get; set; }
    }
}
