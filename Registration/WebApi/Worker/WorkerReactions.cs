using Microsoft.Extensions.Options;
using RabbitMQ.Abstraction;
using WebApi.Worker.Options;
using Messages;
using Microsoft.EntityFrameworkCore.Storage;
using Dto.Registration;
using MediatR;

namespace WebApi.Worker
{
    public class WorkerReactions : BackgroundService
    {
        private readonly ILogger<WorkerReactions> _logger;
        private readonly WorkerOptions _options;
        private readonly IRabbitMqProducer<ReactionMessageBody> _producer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Tuple<IDbContextTransaction, List<UseCasesSendReactionDto>> _tuple;

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
            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("WorkerReactions run at Time: {time}", DateTimeOffset.Now);

                    ReactionsMessage reactionsMessage = new() { maxMessagesReactions = _options.Reactions.MaxMessages };

                    _tuple = await mediator.Send(reactionsMessage, cancellationToken);

                    ReactionMessageBody reactionMessageBody = new()
                    {
                        ReactionMessages = _tuple.Item2
                    };

                    if (reactionMessageBody.ReactionMessages.Count > 0) _producer.Publish(reactionMessageBody);

                    await _tuple.Item1.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("WorkerReactions Time: {time} | Exception: {ex}", DateTimeOffset.Now, ex);
                    await _tuple.Item1.RollbackAsync(cancellationToken);
                }
                finally
                {
                    await Task.Delay(_options.Reactions.DelayMs, cancellationToken);
                }
            }
            await Task.CompletedTask;
        }
    }
}
