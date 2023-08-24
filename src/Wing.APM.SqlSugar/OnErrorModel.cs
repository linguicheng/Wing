using System;
using SqlSugar;

namespace Wing.APM.SqlSugar
{
    public class OnErrorModel
    {
        public Guid ContextID { get; set; }

        public SqlSugarException Exception { get; set; }
    }
}
