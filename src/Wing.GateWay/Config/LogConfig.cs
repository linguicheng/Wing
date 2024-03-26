namespace Wing.Gateway.Config
{
    public class LogConfig
    {
        public bool IsEnabled { get; set; }

        public bool UseEventBus { get; set; }

        public Filter Filter { get; set; }
    }

    public class Filter
    {
        public List<string> ServiceName { get; set; }

        public List<string> RequestUrl { get; set; }

        public List<string> DownstreamUrl { get; set; }
    }
}
