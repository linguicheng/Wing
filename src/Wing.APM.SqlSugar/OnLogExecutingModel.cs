using System;
using SqlSugar;

namespace Wing.APM.SqlSugar
{
    public class OnLogExecutingModel
    {
        public string Id { get; set; }

        public string Sql { get; set; }

        public DateTime BeginTime { get; set; }

        public SugarActionType ActionType { get; set; }
    }
}
