using MediatR;
using Messages;
using Microsoft.Extensions.Options;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WebApi.Options;

namespace WebApi.Consumer
{
    public class ConsumerRegistration : ConsumerBase, IHostedService
    {
        public ConsumerRegistration(IMediator mediator, ConnectionFactory connectionFactory, ILogger<ConsumerBase> consumerLogger, 
            ILogger<RabbitMqClientBase> logger, IOptions<ConsumerOptions> options) : base(mediator, connectionFactory, consumerLogger, logger)
        {
            try
            {
                var consumer = new AsyncEventingBasicConsumer(Channel);
                consumer.Received += OnEventReceived<EventMessage>;
                Channel.BasicConsume(queue: options.Value.Registration.QueueName, autoAck: false, consumer: consumer);
            }
            catch (Exception ex)
            {
                consumerLogger.LogCritical(ex, "Error while consuming message");
            }
        }

        public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}
