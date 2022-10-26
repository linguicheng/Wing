using Wing.Persistence.Saga;

namespace Wing.Saga.Client
{
    public class SagaOptions
    {
        /// <summary>
        /// 事务执行策略
        /// </summary>
        public TranPolicy TranPolicy { get; set; }
        /// <summary>
        /// 熔断条件（重试指定次数失败后，则不再重试）
        /// </summary>
        public int BreakerCount { get; set; }
        /// <summary>
        /// 自定义策略条件（向前恢复指定次数，失败则向后恢复）
        /// </summary>
        public int CustomCount { get; set; }
        /// <summary>
        /// 事务描述
        /// </summary>
        public string Description { get; set; }
    }
}
