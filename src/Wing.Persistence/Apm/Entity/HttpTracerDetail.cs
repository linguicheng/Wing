using System;
using FreeSql.DataAnnotations;
using Wing.EventBus;

namespace Wing.Persistence.Apm
{
    [Table(Name = "APM_HttpTracerDetail")]
    public class HttpTracerDetail : EventMessage
    {
        [Column(DbType = "varchar(50)")]
        public string Id { get; set; }

        [Column(DbType = "varchar(50)")]
        public string TraceId { get; set; }

        /// <summary>
        /// Grpc or Http
        /// </summary>
        [Column(DbType = "varchar(20)")]
        public string RequestType { get; set; }

        [Column(DbType = "varchar(2000)")]
        public string RequestUrl { get; set; }

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
        public long UsedMillSeconds { get; set; }

        public int? StatusCode { get; set; }

        [Column(DbType = "varchar(max)")]
        public string Exception { get; set; }
    }
}
