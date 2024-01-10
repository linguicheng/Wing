namespace Wing.Gateway.Config
{
    public class HeadersTransform
    {
        public Request Request { get; set; }

        public Response Response { get; set; }
    }

    public class Request
    {
        public List<Header> Global { get; set; }

        public List<DownstreamService> Services { get; set; }
    }

    public class Response
    {
        public List<Header> Global { get; set; }

        public List<DownstreamService> Services { get; set; }
    }

    public class DownstreamService
    {
        public string ServiceName { get; set; }

        public List<Header> Headers { get; set; }
    }

    public class Header
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
