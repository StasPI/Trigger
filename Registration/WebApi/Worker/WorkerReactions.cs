using Microsoft.Extensions.Options;
using RabbitMQ.Abstraction;
using WebApi.Worker.Options;
using Messages;
using Messages.Abstraction;

namespace WebApi.Worker
{
    public class WorkerReactions : BackgroundService
    {
        private readonly ILogger<WorkerReactions> _logger;
        private readonly IReactions _reactions;
        private readonly WorkerOptions _options;
        private readonly IRabbitMqProducer<ReactionMessage> _producer;

        public WorkerReactions(IRabbitMqProducer<ReactionMessage> producer, ILogger<WorkerReactions> logger, IReactions reactions, 
            IOptions<WorkerOptions> options)
        {
            _producer = producer;
            _logger = logger;
            _reactions = reactions;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("WorkerReactions run at Time: {time}", DateTimeOffset.Now);
                    ReactionMessage reactionMessage = new() { ReactionMessages = await _reactions.GetMessageAsync(_options.Reactions.MaxMessages, cancellationToken) };

                    if (reactionMessage.ReactionMessages.Count > 0) _producer.Publish(reactionMessage);

                    await _reactions.CommitSendAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("WorkerReactions Time: {time} | Exception: {ex}", DateTimeOffset.Now, ex);
                    await _reactions.RollbackSendAsync(cancellationToken);
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
