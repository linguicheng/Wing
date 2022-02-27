using System;

namespace Wing.Models.GateWay
{
    public class Log
    {
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public string DownstreamUrl { get; set; }
        public string RequestUrl { get; set; }
        public string ClientIp { get; set; }
        public DateTime? RequestTime { get; set; }
        public string RequestType { get; set; }
        public string RequestValue { get; set; }
        public DateTime? ResponseTime { get; set; }
        public string ResponseValue { get; set; }
        public int StatusCode { get; set; }
        public string Policy { get; set; }
    }
}
