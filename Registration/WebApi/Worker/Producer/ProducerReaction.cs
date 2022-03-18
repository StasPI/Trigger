using Microsoft.Extensions.Options;
using RabbitMQ;
using RabbitMQ.Client;
using WebApi.Worker.Options;
using Worker;

namespace WebApi.Worker.Producer
{
    public class ProducerReaction : ProducerBase<ReactionMessage>
    {
        private readonly ProducerOptions _options;
        private readonly string _exchangeName;
        private readonly string _routingKeyName;
        private readonly string _appId;
        protected override string ExchangeName => _exchangeName;
        protected override string RoutingKeyName => _routingKeyName;
        protected override string AppId => _appId;
        public ProducerReaction(ConnectionFactory connectionFactory, ILogger<RabbitMqClientBase> logger,
            ILogger<ProducerBase<ReactionMessage>> producerBaseLogger, IOptions<ProducerOptions> options)
            : base(connectionFactory, logger, producerBaseLogger)
        {
            _options = options.Value;
            _exchangeName = _options.Reactions.ExchangeName;
            _routingKeyName = _options.Reactions.RoutingKeyName;
            _appId = _options.AppId;
        }
    }
}
