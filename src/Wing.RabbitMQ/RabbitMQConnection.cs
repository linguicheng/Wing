using System;
using RabbitMQ.Client;

namespace Wing.RabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;

        public RabbitMQConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public virtual void Create(Action<IModel> channel)
        {
            using var conn = Connect();
            using var model = conn.CreateModel();
            channel(model);
        }

        protected bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen;
            }
        }

        protected IConnection Connect()
        {
            if (IsConnected)
            {
                return _connection;
            }

            _connection = _connectionFactory.CreateConnection();
            return _connection;
        }
    }
}
