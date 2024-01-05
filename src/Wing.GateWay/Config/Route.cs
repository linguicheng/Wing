namespace Wing.Gateway.Config
{
    public class Route
    {
        public Upstream Upstream { get; set; }

        public List<Downstream> Downstreams { get; set; }

        public bool? UseJWTAuth { get; set; }

        public string AuthKey { get; set; }
    }

    public class Upstream
    {
        public string Url { get; set; }

        public string Method { get; set; }
    }

    public class Downstream
    {
        public string ServiceName { get; set; }

        public string Url { get; set; }

        public string Method { get; set; }

        public string Key { get; set; }
    }
}
