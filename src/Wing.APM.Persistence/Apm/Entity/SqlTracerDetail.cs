using FreeSql.DataAnnotations;

namespace Wing.APM.Persistence
{
    [Table(Name = "APM_SqlTracerDetail")]
    [Index("IX_TraceId", "TraceId", false)]
    [Index("IX__BTime", "BeginTime", false)]
    public class SqlTracerDetail
    {
        [Column(IsPrimary = true, StringLength = 50)]
        public string Id { get; set; }

        [Column(StringLength = 50)]
        public string TraceId { get; set; }

        /// <summary>
        /// 动作：增删改查、表结构变更
        /// </summary>
        [Column(StringLength = 50)]
        public string Action { get; set; }

        public DateTime BeginTime { get; set; }

        public string Sql { get; set; }

        public DateTime EndTime { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        public long UsedMillSeconds { get; set; }

        public string Exception { get; set; }
    }
}
