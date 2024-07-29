using FreeSql.DataAnnotations;

namespace Wing.Persistence.Gateway
{
    [Table(Name = "GateWay_Log")]
    [Index("IX_ReqTime", "RequestTime", false)]
    [Index("IX_Name", "ServiceName", false)]
    public class Log
    {
        [Column(IsPrimary = true, StringLength = 50)]
        public string Id { get; set; }

        [Column(StringLength = 200)]
        public string ServiceName { get; set; }

        public string DownstreamUrl { get; set; }

        public string RequestUrl { get; set; }

        /// <summary>
        /// 网关服务器IP
        /// </summary>
        [Column(StringLength = 50)]
        public string GateWayServerIp { get; set; }

        /// <summary>
        /// 客户端请求IP
        /// </summary>
        [Column(StringLength = 50)]
        public string ClientIp { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        [Column(StringLength = 200)]
        public string ServiceAddress { get; set; }

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

        public int StatusCode { get; set; }

        public string Policy { get; set; }

        public string AuthKey { get; set; }

        public string Token { get; set; }

        public string Exception { get; set; }
    }
}
