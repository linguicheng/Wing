using FreeSql.DataAnnotations;
using Wing.ServiceProvider;
using Wing.ServiceProvider.Config;

namespace Wing.ServiceCenter.Entity
{
    [Table(Name = "SC_Service")]
    [Index("IX_Name", "Name", false)]
    public class Service
    {
        [Column(IsPrimary = true, StringLength = 50)]
        public string Id { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        [Column(StringLength = 200)]
        public string Name { get; set; }

        [Column(StringLength = 20)]
        public string Scheme { get; set; }

        [Column(StringLength = 50)]
        public string Host { get; set; }

        public int Port { get; set; }

        /// <summary>
        /// 服务类别 Http/Grpc
        /// </summary>
        public ServiceOptions ServiceType { get; set; }

        /// <summary>
        /// 健康检查地址
        /// </summary>
        [Column(StringLength = 200)]
        public string HealthCheckUrl { get; set; }

        /// <summary>
        /// 健康检查超时 单位秒
        /// </summary>
        public int? HealthCheckTimeout { get; set; }

        /// <summary>
        /// 健康检查间隔时间 单位秒
        /// </summary>
        public int? HealthCheckInterval { get; set; }

        public LoadBalancerOptions LoadBalancer { get; set; }

        public int? Weight { get; set; }

        public HealthStatus Status { get; set; }

        [Column(StringLength = 200)]
        public string ConfigKey { get; set; }

        [Column(StringLength = 500)]
        public string Tag { get; set; }

        [Column(StringLength = 100)]
        public string Developer { get; set; }

        [Column(StringLength = 500)]
        public string Remark { get; set; }
    }
}
