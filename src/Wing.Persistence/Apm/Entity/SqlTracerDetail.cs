using FreeSql.DataAnnotations;

namespace Wing.Persistence.Apm
{
    public class SqlTracerDetail
    {
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        public string TraceId { get; set; }

        /// <summary>
        /// 动作：增删改查、表结构变更
        /// </summary>
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
