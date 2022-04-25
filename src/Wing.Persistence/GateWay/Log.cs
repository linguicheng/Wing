using System;
using FreeSql.DataAnnotations;
using Wing.EventBus;

namespace Wing.Persistence.GateWay
{
    [Table(Name = "GateWay_Log")]
    public class Log : EventMessage
    {
        [Column(DbType = "varchar(32)")]
        public string Id { get; set; }

        [Column(DbType = "varchar(800)")]
        public string ServiceName { get; set; }

        [Column(DbType = "varchar(8000)")]
        public string DownstreamUrl { get; set; }

        [Column(DbType = "varchar(8000)")]
        public string RequestUrl { get; set; }
        /// <summary>
        /// 网关服务器IP
        /// </summary>
        [Column(DbType = "varchar(100)")]
        public string GateWayServerIp { get; set; }
        /// <summary>
        /// 客户端请求IP
        /// </summary>
        [Column(DbType = "varchar(100)")]
        public string ClientIp { get; set; }
        /// <summary>
        /// 服务地址
        /// </summary>
        [Column(DbType = "varchar(200)")]
        public string ServiceAddress { get; set; }

        public DateTime RequestTime { get; set; }

        [Column(DbType = "varchar(20)")]
        public string RequestMethod { get; set; }

        [Column(DbType = "varchar(max)")]
        public string RequestValue { get; set; }

        public DateTime ResponseTime { get; set; }

        [Column(DbType = "varchar(max)")]
        public string ResponseValue { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        public double UsedMillSeconds { get; set; }

        public int StatusCode { get; set; }

        [Column(DbType = "varchar(max)")]
        public string Policy { get; set; }

        [Column(DbType = "varchar(8000)")]
        public string AuthKey { get; set; }

        [Column(DbType = "varchar(8000)")]
        public string Token { get; set; }
    }
}
