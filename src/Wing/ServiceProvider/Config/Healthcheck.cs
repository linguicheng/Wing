namespace Wing.ServiceProvider.Config
{
    public class Healthcheck
    {
        public string Url { get; set; }

        public int? Timeout { get; set; }

        public int? Interval { get; set; }

        public int? RemoveService { get; set; }

        public bool? GRPCUseTLS { get; set; }
    }
}
