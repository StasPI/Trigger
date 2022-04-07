using Microsoft.Extensions.Options;
using RabbitMQ.Abstraction;
using WebApi.Worker.Options;
using Messages;
using Messages.Abstraction;

namespace WebApi.Worker
{
    public class WorkerEvents : BackgroundService
    {
        private readonly ILogger<WorkerEvents> _logger;
        private readonly IEvents _events;
        private readonly WorkerOptions _options;
        private readonly IRabbitMqProducer<EventMessage> _producer;

        public WorkerEvents(IRabbitMqProducer<EventMessage> producer, ILogger<WorkerEvents> logger, IEvents events, 
            IOptions<WorkerOptions> options)
        {
            _producer = producer;
            _logger = logger;
            _events = events;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("WorkerEvents run at Time: {time}", DateTimeOffset.Now);
                    EventMessage eventMessage = new() { EventMessages = await _events.GetMessageAsync(_options.Events.MaxMessages, cancellationToken) };

                    if (eventMessage.EventMessages.Count > 0) _producer.Publish(eventMessage);

                    await _events.CommitSendAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("WorkerEvents Time: {time} | Exception: {ex}", DateTimeOffset.Now, ex);
                    await _events.RollbackSendAsync(cancellationToken);
                }
                finally
                {
                    await Task.Delay(_options.Events.DelayMs, cancellationToken);
                }
            }
            await Task.CompletedTask;
        }
    }
}
