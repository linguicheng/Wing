using System;

namespace Wing.APM.SqlSugar
{
    public class OnLogExecutedModel
    {
        public Guid ContextID { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan ExecutionTime { get; set; }
    }
}
