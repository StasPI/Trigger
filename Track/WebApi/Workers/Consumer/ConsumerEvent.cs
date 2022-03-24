using MediatR;
using Messages;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WebApi.Workers.Consumer
{
    public class ConsumerEvent : ConsumerBase, IHostedService
    {
        public ConsumerEvent(IMediator mediator, ConnectionFactory connectionFactory, ILogger<ConsumerBase> consumerLogger, 
            ILogger<RabbitMqClientBase> logger) : base(mediator, connectionFactory, consumerLogger, logger)
        {
            try
            {
                var consumer = new AsyncEventingBasicConsumer(Channel);
                consumer.Received += OnEventReceived<EventMessage>;
                Channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
            }
            catch (Exception ex)
            {
                consumerLogger.LogCritical(ex, "Error while consuming message");
            }
        }

        protected override string QueueName => "QueueEvent";

        public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}
