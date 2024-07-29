using FreeSql.DataAnnotations;
using Wing.EventBus;
using Wing.ServiceProvider.Config;

namespace Wing.Persistence.Saga
{
    [Table(Name = "Saga_Tran")]
    [Index("IX_CTime", "CreatedTime", false)]
    [Index("IX_Name", "Name", false)]
    public class SagaTran : EventMessage
    {
        [Column(IsPrimary = true, StringLength = 50)]
        public string Id { get; set; }

        /// <summary>
        /// 事务名称
        /// </summary>
        [Column(StringLength = 200)]
        public string Name { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        [Column(StringLength = 200)]
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务类别：grpc or http
        /// </summary>
        public ServiceOptions ServiceType { get; set; }

        /// <summary>
        /// 事务状态
        /// </summary>
        public TranStatus Status { get; set; }

        /// <summary>
        /// 重试结果
        /// </summary>
        public ExecutedResult? RetryResult { get; set; }

        /// <summary>
        /// 重试动作
        /// </summary>
        [Column(StringLength = 50)]
        public string RetryAction { get; set; }

        public TranPolicy Policy { get; set; }

        /// <summary>
        /// 向前恢复执行次数
        /// </summary>
        public int CommittedCount { get; set; }

        /// <summary>
        /// 向后恢复回滚次数
        /// </summary>
        public int CancelledCount { get; set; }

        /// <summary>
        /// 熔断条件（重试指定次数失败后，则不再重试）
        /// </summary>
        public int BreakerCount { get; set; }

        /// <summary>
        /// 自定义策略条件（向前恢复指定次数，失败则向后恢复）
        /// </summary>
        public int CustomCount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 开始执行时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束执行时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        public long UsedMillSeconds { get; set; }

        public string Description { get; set; }
    }
}
