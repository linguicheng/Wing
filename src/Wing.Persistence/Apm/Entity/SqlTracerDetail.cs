using System;
using FreeSql.DataAnnotations;
using Wing.EventBus;

namespace Wing.Persistence.Apm
{
    [Table(Name = "APM_SqlTracerDetail")]
    public class SqlTracerDetail : EventMessage
    {
        [Column(DbType = "varchar(50)")]
        public string Id { get; set; }

        [Column(DbType = "varchar(50)")]
        public string TraceId { get; set; }

        [Column(DbType = "varchar(50)")]
        public string DbType { get; set; }

        public DateTime RequestTime { get; set; }

        [Column(DbType = "varchar(max)")]
        public string RequestValue { get; set; }

        public DateTime ResponseTime { get; set; }

        [Column(DbType = "varchar(max)")]
        public string ResponseValue { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        public long UsedMillSeconds { get; set; }

        [Column(DbType = "varchar(max)")]
        public string Exception { get; set; }
    }
}
