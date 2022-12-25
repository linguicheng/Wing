using System;
using Wing.EventBus;

namespace Wing.Persistence.Saga
{
    public class RetryEvent : EventMessage
    {
        public string Id { get; set; }

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
        /// 重试结果
        /// </summary>
        public ExecutedResult RetryResult { get; set; }

        /// <summary>
        /// 重试动作
        /// </summary>
        public string RetryAction { get; set; }
    }
}
