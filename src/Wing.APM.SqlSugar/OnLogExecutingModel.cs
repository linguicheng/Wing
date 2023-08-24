using SqlSugar;
using System;

namespace Wing.APM.SqlSugar
{
    public class OnLogExecutingModel
    {
        public Guid ContextID { get; set; }

        public string Sql { get; set; }

        public DateTime BeginTime { get; set; }

        public SugarActionType ActionType { get; set; }
    }
}
