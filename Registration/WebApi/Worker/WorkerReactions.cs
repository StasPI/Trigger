using Dto.Registration;
using Microsoft.Extensions.Options;
using WebApi.Worker.Options;
using Worker.Abstraction;

namespace WebApi.Worker
{
    public class WorkerReactions : BackgroundService
    {
        private readonly ILogger<WorkerReactions> _logger;
        private readonly IReactions _reactions;
        private readonly WorkerOptions _options;

        public WorkerReactions(ILogger<WorkerReactions> logger, IEvents events, IOptions<WorkerOptions> options, IReactions reactions)
        {
            _logger = logger;
            _reactions = reactions;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("WorkerReactions run at: {time}", DateTimeOffset.Now);
                List<UseCasesSendReactionDto> useCasesSendEventDto = await _reactions.Get(_options.Reactions.MaxMessages, cancellationToken);
            }
        }
    }
}
