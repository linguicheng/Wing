namespace Wing.APM.Persistence
{
    public class SqlTracerSearchDto
    {
        /// <summary>
        /// 动作：增删改查、表结构变更
        /// </summary>
        public string Action { get; set; }

        public string ServerIp { get; set; }

        public string ServiceName { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// 开始执行时间
        /// </summary>
        public List<DateTime> BeginTime { get; set; }

        public string Sql { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        public long UsedMillSeconds { get; set; }

        public string Exception { get; set; }
    }
}
