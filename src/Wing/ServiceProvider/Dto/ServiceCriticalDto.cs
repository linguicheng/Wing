namespace Wing.ServiceProvider.Dto
{
    /// <summary>
    /// 服务死亡率排行
    /// </summary>
    public class ServiceCriticalDto
    {
        public string ServiceName { get; set; }

        public int Total { get; set; }

        public int CriticalTotal { get; set; }

        public double CriticalLv { get; set; }
    }
}
