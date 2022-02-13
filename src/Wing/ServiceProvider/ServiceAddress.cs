using System.Collections.Generic;
using Wing.ServiceProvider.Config;

namespace Wing.ServiceProvider
{
    public class ServiceAddress
    {
        public ServiceAddress(string host, int port, string scheme)
        {
            Host = host;
            Port = port;
            Sheme = scheme;
        }

        public string Sheme { get; }

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
