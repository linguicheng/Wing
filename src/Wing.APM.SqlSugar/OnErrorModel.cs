using System;
using SqlSugar;

namespace Wing.APM.SqlSugar
{
    public class OnErrorModel
    {
        public string Id { get; set; }

        public SqlSugarException Exception { get; set; }
    }
}
