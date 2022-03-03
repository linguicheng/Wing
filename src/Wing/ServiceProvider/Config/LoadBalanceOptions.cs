namespace Wing.ServiceProvider.Config
{
    public enum LoadBalancerOptions
    {
        /// <summary>
        /// 轮询
        /// </summary>
        RoundRobin,
        /// <summary>
        /// 加权轮询
        /// </summary>
        WeightRoundRobin,
        /// <summary>
        /// 最小连接数
        /// </summary>
        LeastConnection
    }
}
