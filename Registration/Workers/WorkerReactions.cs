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
    public class WorkerReactions : BackgroundService
    {
        private readonly ILogger<WorkerReactions> _logger;
        private readonly WorkerOptions _options;
        private readonly IRabbitMqProducer<ReactionMessageBody> _producer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private TransitBody<ReactionMessageBody> _transitBody;

        public WorkerReactions(IRabbitMqProducer<ReactionMessageBody> producer, ILogger<WorkerReactions> logger,
            IOptions<WorkerOptions> options, IServiceScopeFactory serviceScopeFactory)
        {
            _producer = producer;
            _logger = logger;
            _options = options.Value;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("WorkerReactions run at Time: {time}", DateTimeOffset.Now);

                    IMediator mediator = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IMediator>();

                    ReactionsMessage reactionsMessage = new() { maxMessagesReactions = _options.Reactions.MaxMessages };

                    _transitBody = await mediator.Send(reactionsMessage, cancellationToken);

                    if (_transitBody.Messages.ReactionMessages.Count > 0) _producer.Publish(_transitBody.Messages);
                    await _transitBody.Transaction.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("WorkerReactions Time: {time} | Exception: {ex}", DateTimeOffset.Now, ex);
                    await _transitBody.Transaction.RollbackAsync(cancellationToken);
                }
                finally
                {
                    await _transitBody.Transaction.DisposeAsync();
                    await Task.Delay(_options.Reactions.DelayMs, cancellationToken);
                }
            }
            await Task.CompletedTask;
        }
    }
}
