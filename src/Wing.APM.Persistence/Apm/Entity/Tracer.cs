using FreeSql.DataAnnotations;

namespace Wing.APM.Persistence
{
    [Table(Name = "APM_Tracer")]
    [Index("IX_ReqTime", "RequestTime", false)]
    [Index("IX_Name", "ServiceName", false)]
    public class Tracer
    {
        [Column(IsPrimary = true, StringLength = 50)]
        public string Id { get; set; }

        [Column(StringLength = 50)]
        public string ParentId { get; set; }

        [Column(StringLength = 200)]
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        [Column(StringLength = 200)]
        public string ServiceUrl { get; set; }

        public DateTime RequestTime { get; set; }

        /// <summary>
        /// Grpc or Http
        /// </summary>
        [Column(StringLength = 20)]
        public string RequestType { get; set; }

        public string RequestUrl { get; set; }

        [Column(StringLength = 20)]
        public string RequestMethod { get; set; }

        public string RequestValue { get; set; }

        /// <summary>
        /// 客户端请求IP
        /// </summary>
        [Column(StringLength = 50)]
        public string ClientIp { get; set; }

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
