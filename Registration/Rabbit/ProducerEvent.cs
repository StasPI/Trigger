using Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Options;
using RabbitMQ;
using RabbitMQ.Client;

namespace Rabbit
{
    public class ProducerEvent : ProducerBase<EventMessageBody>
    {
        private readonly RabbitMQOptions _options;
        private readonly string _exchangeName;
        private readonly string _routingKeyName;
        private readonly string _appId;
        private readonly string _contentType;
        private readonly byte _deliveryMode;
        protected override string ExchangeName => _exchangeName;
        protected override string RoutingKeyName => _routingKeyName;
        protected override string AppId => _appId;
        protected override string ContentType => _contentType;
        protected override byte DeliveryMode => _deliveryMode;

        public ProducerEvent(ConnectionFactory connectionFactory, ILogger<RabbitMqClientBase> logger,
            ILogger<ProducerBase<EventMessageBody>> producerBaseLogger, IOptions<RabbitMQOptions> options)
            : base(connectionFactory, options, logger, producerBaseLogger)
        {
            _options = options.Value;
            _exchangeName = _options.ProducerEvent.ExchangeName;
            _routingKeyName = _options.ProducerEvent.RoutingKeyName;
            _appId = _options.AppId;
            _contentType = _options.ContentType;
            _deliveryMode = _options.DeliveryMode;
        }
    }
}