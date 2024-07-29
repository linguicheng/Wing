using FreeSql.DataAnnotations;

namespace Wing.APM.Persistence
{
    [Table(Name = "APM_WS_SqlTracer")]
    [Index("IX_BTime", "BeginTime", false)]
    [Index("IX_Name", "ServiceName", false)]
    public class SqlTracer
    {
        [Column(IsPrimary = true, StringLength = 50)]
        public string Id { get; set; }

        /// <summary>
        /// 动作：增删改查、表结构变更
        /// </summary>
        [Column(StringLength = 50)]
        public string Action { get; set; }

        [Column(StringLength = 200)]
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        [Column(StringLength = 200)]
        public string ServiceUrl { get; set; }

        /// <summary>
        /// 开始执行时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        public string Sql { get; set; }

        /// <summary>
        /// 结束执行时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        public long UsedMillSeconds { get; set; }

        public string Exception { get; set; }
    }
}
