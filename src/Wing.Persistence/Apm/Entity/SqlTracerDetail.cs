using System;
using FreeSql.DataAnnotations;

namespace Wing.Persistence.Apm
{
    public class SqlTracerDetail
    {
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        public string TraceId { get; set; }

        public string DbType { get; set; }

        public DateTime RequestTime { get; set; }

        public string RequestValue { get; set; }

        public DateTime ResponseTime { get; set; }

        public string ResponseValue { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        public long UsedMillSeconds { get; set; }

        public string Exception { get; set; }
    }
}
