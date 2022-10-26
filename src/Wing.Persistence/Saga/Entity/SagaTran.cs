using System;
using FreeSql.DataAnnotations;
using Wing.EventBus;

namespace Wing.Persistence.Saga
{
    public class SagaTran : EventMessage
    {
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        public string Name { get; set; }

        public TranStatus Status { get; set; }

        public TranPolicy Policy { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int ExecutedCount { get; set; }

        /// <summary>
        /// 回滚次数
        /// </summary>
        public int RollbackCount { get; set; }

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
        /// 更新时间
        /// </summary>
        public DateTime UpdatedTime { get; set; }

        public string Description { get; set; }
    }
}
