﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Options;
using RabbitMQ.Abstraction;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMQ
{
    public abstract class ProducerBase<T> : RabbitMqClientBase, IRabbitMqProducer<T>
    {
        private readonly ILogger<ProducerBase<T>> _logger;
        protected abstract string ExchangeName { get; }
        protected abstract string RoutingKeyName { get; }
        protected abstract string AppId { get; }
        protected abstract string ContentType { get; }
        protected abstract byte DeliveryMode { get; }

        protected ProducerBase(ConnectionFactory connectionFactory, IOptions<RabbitMQOptions> options, ILogger<RabbitMqClientBase> logger, ILogger<ProducerBase<T>> producerBaseLogger)
            : base(connectionFactory, options, logger) => _logger = producerBaseLogger;

        public virtual void Publish(T @event)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
                var properties = Channel.CreateBasicProperties();
                properties.AppId = AppId;
                properties.ContentType = ContentType;
                properties.DeliveryMode = DeliveryMode;
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                Channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKeyName, body: body, basicProperties: properties);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error while publishing");
                throw;
            }
        }
    }
}
