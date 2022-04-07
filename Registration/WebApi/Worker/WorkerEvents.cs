using Dto.Registration;
using MediatR;
using Messages;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using RabbitMQ.Abstraction;
using WebApi.Worker.Options;

namespace WebApi.Worker
{
    public class WorkerEvents : BackgroundService
    {
        private readonly ILogger<WorkerEvents> _logger;
        private readonly WorkerOptions _options;
        private readonly IRabbitMqProducer<EventMessageBody> _producer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Tuple<IDbContextTransaction, List<UseCasesSendEventDto>> _tuple; 

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

                    _tuple = await mediator.Send(eventsMessage, cancellationToken);

                    EventMessageBody eventMessageBody = new()
                    {
                        EventMessages = _tuple.Item2
                    };

                    if (eventMessageBody.EventMessages.Count > 0) _producer.Publish(eventMessageBody);

                    await _tuple.Item1.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("WorkerEvents Time: {time} | Exception: {ex}", DateTimeOffset.Now, ex);
                    await _tuple.Item1.RollbackAsync(cancellationToken);
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
