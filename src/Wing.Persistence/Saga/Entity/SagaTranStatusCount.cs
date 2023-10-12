using System;
using FreeSql.DataAnnotations;

namespace Wing.Persistence.Saga
{
    public class SagaTranStatusCount
    {
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        /// <summary>
        /// 事务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        public int SuccessCount { get; set; }

        public int ExecutingCount { get; set; }

        public int FaildCount { get; set; }

        public int CancelledCount { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
