using RabbitMQ;
using RabbitMQ.Client;
using Worker;

namespace WebApi.Worker.Producer
{
    public class EventProducer : ProducerBase<EventMessage>
    {
        public EventProducer(ConnectionFactory connectionFactory, ILogger<RabbitMqClientBase> logger, 
            ILogger<ProducerBase<EventMessage>> producerBaseLogger) :base(connectionFactory, logger, producerBaseLogger)
        {
        }

        protected override string ExchangeName => "CUSTOM_HOST.LoggerExchange";
        protected override string RoutingKeyName => "log.message";
        protected override string AppId => "LogProducer";
    }
}
