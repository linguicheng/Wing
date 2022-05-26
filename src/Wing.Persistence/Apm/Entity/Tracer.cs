using System;
using FreeSql.DataAnnotations;
using Wing.EventBus;

namespace Wing.Persistence.Apm
{
    [Table(Name = "APM_Tracer")]
    public class Tracer : EventMessage
    {
        [Column(DbType = "varchar(50)")]
        public string Id { get; set; }

        [Column(DbType = "varchar(50)")]
        public string ParentId { get; set; }

        [Column(DbType = "varchar(800)")]
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        [Column(DbType = "varchar(200)")]
        public string ServiceUrl { get; set; }

        public DateTime RequestTime { get; set; }

        /// <summary>
        /// Grpc or Http
        /// </summary>
        [Column(DbType = "varchar(20)")]
        public string RequestType { get; set; }

        [Column(DbType = "varchar(2000)")]
        public string RequestUrl { get; set; }

        [Column(DbType = "varchar(20)")]
        public string RequestMethod { get; set; }

        [Column(DbType = "varchar(max)")]
        public string RequestValue { get; set; }

        /// <summary>
        /// 客户端请求IP
        /// </summary>
        [Column(DbType = "varchar(100)")]
        public string ClientIp { get; set; }

        public DateTime ResponseTime { get; set; }

        [Column(DbType = "varchar(max)")]
        public string ResponseValue { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        public long UsedMillSeconds { get; set; }

        public int? StatusCode { get; set; }

        [Column(DbType = "varchar(max)")]
        public string Exception { get; set; }
    }
}
