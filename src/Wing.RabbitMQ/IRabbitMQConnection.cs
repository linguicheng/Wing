using System;
using RabbitMQ.Client;

namespace Wing.RabbitMQ
{
    public interface IRabbitMQConnection
    {
        void Create(Action<IModel> channel);
    }
}
