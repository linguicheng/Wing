using System;
using RabbitMQ.Client;

namespace Wing.RabbitMQ
{
    public class ConsumerConnection : RabbitMQConnection
    {
        public ConsumerConnection(IConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        public override void Create(Action<IModel> channel)
        {
            var conn = Connect();
            var model = conn.CreateModel();
            channel(model);
            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                model.Close();
                conn.Close();
            };
        }
    }
}
