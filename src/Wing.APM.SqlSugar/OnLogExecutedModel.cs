using System;

namespace Wing.APM.SqlSugar
{
    public class OnLogExecutedModel
    {
        public string Id { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan ExecutionTime { get; set; }
    }
}
