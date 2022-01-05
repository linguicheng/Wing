using System;

namespace Wing.ServiceProvider.Config
{
    public class Provider
    {
        public string Url { get; set; }

        public ServiceData Service { get; set; }

        public int? Interval { get; set; }

        public string DataCenter { get; set; } = "dc1";

        public string Token { get; set; }

        public string Key { get; set; }

        public int WaitTime { get; set; } = 3;
    }
}
