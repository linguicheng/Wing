namespace Wing.Gateway.Config
{
    public class HeadersTransform
    {
        public Request Request { get; set; }

        public Response Response { get; set; }
    }

    public class Request
    {
        public Dictionary<string, string> Global { get; set; }

        public List<DownstreamService> Services { get; set; }
    }

    public class Response
    {
        public Dictionary<string, string> Global { get; set; }

        public List<DownstreamService> Services { get; set; }
    }

    public class DownstreamService
    {
        public string ServiceName { get; set; }

        public Dictionary<string, string> Headers { get; set; }
    }
}
