using FreeSql.DataAnnotations;

namespace Wing.Persistence.Saga
{
    [Table(Name = "Saga_TranStatusCount")]
    [Index("IX_Name", "Name", false)]
    public class SagaTranStatusCount
    {
        [Column(IsPrimary = true, StringLength = 50)]
        public string Id { get; set; }

        /// <summary>
        /// 事务名称
        /// </summary>
        [Column(StringLength = 200)]
        public string Name { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        [Column(StringLength = 200)]
        public string ServiceName { get; set; }

        public int SuccessCount { get; set; }

        public int ExecutingCount { get; set; }

        public int FaildCount { get; set; }

        public int CancelledCount { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
