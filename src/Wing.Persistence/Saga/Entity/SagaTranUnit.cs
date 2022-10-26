using System;
using FreeSql.DataAnnotations;
using Wing.EventBus;

namespace Wing.Persistence.Saga
{
    public class SagaTranUnit : EventMessage
    {
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string TranId { get; set; }

        public int OrderNo { get; set; }

        public TranStatus Status { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int ExecutedCount { get; set; }

        /// <summary>
        /// 回滚次数
        /// </summary>
        public int RollbackCount { get; set; }

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
