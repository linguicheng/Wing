namespace Wing.ServiceProvider.Config
{
    public class LoadBalancer
    {
        public LoadBalancerOptions Option { get; set; }

        public int? Weight { get; set; }
    }
}
