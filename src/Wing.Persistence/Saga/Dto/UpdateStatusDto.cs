using System;
using Wing.EventBus;

namespace Wing.Persistence.Saga
{
    public class UpdateStatusDto : EventMessage
    {
        public string Id { get; set; }

        public TranStatus Status { get; set; }

        public DateTime EndTime { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        public long UsedMillSeconds { get; set; }
    }
}
