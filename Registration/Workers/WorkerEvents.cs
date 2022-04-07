using Dto.Registration;
using MediatR;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Options;
using RabbitMQ.Abstraction;

namespace Workers
{
    public class WorkerEvents : BackgroundService
    {
        private readonly ILogger<WorkerEvents> _logger;
        private readonly WorkerOptions _options;
        private readonly IRabbitMqProducer<EventMessageBody> _producer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private TransitBody<UseCasesSendEventDto> _transitBody;

        public WorkerEvents(IRabbitMqProducer<EventMessageBody> producer, ILogger<WorkerEvents> logger,
            IOptions<WorkerOptions> options, IServiceScopeFactory serviceScopeFactory)
        {
            _producer = producer;
            _logger = logger;
            _options = options.Value;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("WorkerEvents run at Time: {time}", DateTimeOffset.Now);

                    EventsMessage eventsMessage = new() { maxMessagesEvents = _options.Events.MaxMessages };

                    _transitBody = await mediator.Send(eventsMessage, cancellationToken);

                    EventMessageBody eventMessageBody = new()
                    {
                        EventMessages = _transitBody.Messages
                    };

                    if (eventMessageBody.EventMessages.Count > 0) _producer.Publish(eventMessageBody);

                    await _transitBody.Transaction.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("WorkerEvents Time: {time} | Exception: {ex}", DateTimeOffset.Now, ex);
                    await _transitBody.Transaction.RollbackAsync(cancellationToken);
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