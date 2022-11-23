using System;
using Wing.EventBus;

namespace Wing.Persistence.Saga
{
    public class RetryEvent : EventMessage
    {
        public string Id { get; set; }

        public TranStatus Status { get; set; }

        /// <summary>
        /// 开始执行时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束执行时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
