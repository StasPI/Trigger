using MediatR;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace RabbitMQ
{
    public abstract class ConsumerBase : RabbitMqClientBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ConsumerBase> _logger;

        public ConsumerBase(IMediator mediator,ConnectionFactory connectionFactory,ILogger<ConsumerBase> consumerLogger,
            ILogger<RabbitMqClientBase> logger) : base(connectionFactory, logger)
        {
            _mediator = mediator;
            _logger = consumerLogger;
        }

        protected virtual async Task OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                var message = JsonSerializer.Deserialize<T>(body);

                await _mediator.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error while retrieving message from queue.");
            }
            finally
            {
                Channel.BasicAck(@event.DeliveryTag, false);
            }
        }
    }
}
