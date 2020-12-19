using System.Collections.Generic;
using Wing.Configuration;

namespace Wing.ServiceProvider
{
    public class ServiceAddress
    {
        public ServiceAddress(string host, int port, IEnumerable<string> tags)
        {
            ServiceTool.GetServiceTagConfig(tags, ServiceTag.SCHEME, scheme => Sheme = scheme);
            Host = host;
            Port = port;
        }

        public string Sheme { get; private set; }

        public string Host { get; }

        public int Port { get; }

        public static bool operator ==(ServiceAddress left, ServiceAddress right)
        {
            return left.Host == right.Host && left.Port == right.Port;
        }

        public static bool operator !=(ServiceAddress left, ServiceAddress right)
        {
            return left.Host != right.Host || left.Port != right.Port;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Sheme}://{Host}:{Port}";
        }
    }
}
