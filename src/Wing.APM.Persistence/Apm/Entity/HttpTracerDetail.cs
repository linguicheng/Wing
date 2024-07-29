using FreeSql.DataAnnotations;

namespace Wing.APM.Persistence
{
    [Table(Name = "APM_HttpTracerDetail")]
    [Index("IX_TraceId", "TraceId", false)]
    [Index("IX_ReqTime", "RequestTime", false)]
    public class HttpTracerDetail
    {
        [Column(IsPrimary = true, StringLength = 50)]
        public string Id { get; set; }

        [Column(StringLength = 50)]
        public string TraceId { get; set; }

        /// <summary>
        /// Grpc or Http
        /// </summary>
        [Column(StringLength = 20)]
        public string RequestType { get; set; }

        public string RequestUrl { get; set; }

        public DateTime RequestTime { get; set; }

        [Column(StringLength = 20)]
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
