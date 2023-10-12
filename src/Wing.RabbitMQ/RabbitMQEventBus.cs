using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Wing.Converter;
using Wing.EventBus;

namespace Wing.RabbitMQ
{
    public class RabbitMQEventBus : IEventBus
    {
        private readonly IRabbitMQConnection _connection;
        private readonly IRabbitMQConnection _conSumerConnection;
        private readonly IJson _json;
        private readonly Config _config;
        private readonly ILogger<RabbitMQEventBus> _logger;

        public RabbitMQEventBus(IRabbitMQConnection connection, IRabbitMQConnection conSumerConnection, IJson json, ILogger<RabbitMQEventBus> logger, Config config)
        {
            _connection = connection;
            _conSumerConnection = conSumerConnection;
            _json = json;
            _config = config;
            _logger = logger;
        }

        public void Publish(EventMessage message, Action<bool> confirm)
        {
            _connection.Create(channel =>
            {
                channel.BasicNacks += (sender, e) =>
                {
                    confirm(false);
                };

                channel.BasicAcks += (sender, e) =>
                {
                    confirm(true);
                };
                channel.ExchangeDeclare(exchange: _config.ExchangeName, type: ExchangeType.Direct);
                var props = channel.CreateBasicProperties();
                props.DeliveryMode = 2;
                var msg = _json.Serialize(message);
                var routingKey = message.GetType().FullName;
                _logger.LogDebug("消息发布：内容【{0}】，exchange={1}，routingKey={2}", msg, _config.ExchangeName, routingKey);
                var body = Encoding.UTF8.GetBytes(msg);
                channel.ConfirmSelect();
                channel.BasicPublish(exchange: _config.ExchangeName, routingKey: routingKey, basicProperties: props, body: body);
            });
        }

        public void Publish(EventMessage message)
        {
            _connection.Create(channel =>
            {
                channel.ExchangeDeclare(exchange: _config.ExchangeName, type: ExchangeType.Direct);
                var props = channel.CreateBasicProperties();
                props.DeliveryMode = 2;
                var msg = _json.Serialize(message);
                var routingKey = message.GetType().FullName;
                _logger.LogDebug("消息发布：内容【{0}】，exchange={1}，routingKey={2}", msg, _config.ExchangeName, routingKey);
                var body = Encoding.UTF8.GetBytes(msg);
                channel.ConfirmSelect();
                channel.BasicPublish(exchange: _config.ExchangeName, routingKey: routingKey, basicProperties: props, body: body);
            });
        }

        public void Subscribe<TEventMessage, TConsumer>()
            where TEventMessage : EventMessage
            where TConsumer : ISubscribe<TEventMessage>, new()
        {
            var queueName = typeof(TConsumer).FullName;
            var routingKey = typeof(TEventMessage).FullName;
            var subscribeAttribute = typeof(TConsumer).GetCustomAttribute<SubscribeAttribute>();
            if (subscribeAttribute == null)
            {
                subscribeAttribute = new SubscribeAttribute(QueueMode.Normal);
            }

            var dlxExchange = $"{_config.ExchangeName}.dlx";
            var arguments = new Dictionary<string, object>();
            var exchangeName = string.Empty;
            foreach (var mode in subscribeAttribute.QueueModes)
            {
                _conSumerConnection.Create(channel =>
                {
                    if (mode == QueueMode.Normal)
                    {
                        arguments.Add("x-dead-letter-exchange", dlxExchange);
                        arguments.Add("x-message-ttl", _config.MessageTTL);
                        arguments.Add("x-dead-letter-routing-key", routingKey);
                        exchangeName = _config.ExchangeName;
                    }
                    else if (mode == QueueMode.DLX)
                    {
                        exchangeName = dlxExchange;
                        queueName += ".dlx";
                        arguments = null;
                    }

                    channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: arguments);
                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);
                    channel.BasicQos(0, _config.PrefetchCount, false);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        var eventMessage = _json.Deserialize<TEventMessage>(message);
                        var result = false;
                        result = await new TConsumer().Consume(eventMessage);
                        try
                        {
                            if (result)
                            {
                                channel.BasicAck(ea.DeliveryTag, false);
                            }
                            else
                            {
                                channel.BasicNack(ea.DeliveryTag, false, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "消息订阅：内容【{0}】，exchange={1}，routingKey={2}", message, exchangeName, routingKey);
                        }

                        _logger.LogDebug("消息订阅：内容【{0}】，exchange={1}，routingKey={2}，消息处理结果：{3}", message, exchangeName, routingKey, result ? "Success" : "Fail");
                    };
                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                });
            }
        }
    }
}
