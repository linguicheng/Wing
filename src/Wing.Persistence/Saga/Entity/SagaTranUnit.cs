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
        /// 向前恢复执行次数
        /// </summary>
        public int ExecutedCount { get; set; }

        /// <summary>
        /// 向后恢复回滚次数
        /// </summary>
        public int RollbackCount { get; set; }

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

        /// <summary>
        /// 参数命名空间
        /// </summary>
        public string ParamsNamespace { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamsValue { get; set; }

        public string Description { get; set; }

        public string ErrorMsg { get; set; }
    }
}
