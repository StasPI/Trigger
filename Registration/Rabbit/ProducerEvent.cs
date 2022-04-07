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
        private readonly ProducerOptions _options;
        private readonly string _exchangeName;
        private readonly string _routingKeyName;
        private readonly string _appId;
        protected override string ExchangeName => _exchangeName;
        protected override string RoutingKeyName => _routingKeyName;
        protected override string AppId => _appId;
        public ProducerEvent(ConnectionFactory connectionFactory, ILogger<RabbitMqClientBase> logger,
            ILogger<ProducerBase<EventMessageBody>> producerBaseLogger, IOptions<ProducerOptions> options)
            : base(connectionFactory, logger, producerBaseLogger)
        {
            _options = options.Value;
            _exchangeName = _options.Events.ExchangeName;
            _routingKeyName = _options.Events.RoutingKeyName;
            _appId = _options.AppId;
        }
    }
}