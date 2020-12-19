namespace Wing.RabbitMQ
{
    public class Config
    {
        public string HostName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string VirtualHost { get; set; }

        public int Port { get; set; }

        public int? MessageTTL { get; set; }

        public string ExchangeName { get; set; }

        public ushort PrefetchCount { get; set; }
    }
}
