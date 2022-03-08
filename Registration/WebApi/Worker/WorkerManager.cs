using Microsoft.Extensions.Options;
using Worker.Abstraction;

namespace WebApi.Worker
{
    public class WorkerManager : BackgroundService
    {
        private readonly ILogger<WorkerManager> _logger;
        private readonly IEventWorker _eventWorker;
        private readonly IReactionWorker _reactionWorker;
        private readonly WorkerManagerOptions _workerOptions;
        public WorkerManager(ILogger<WorkerManager> logger, IOptions<WorkerManagerOptions> workerOptions,
            IEventWorker eventWorker, IReactionWorker reactionWorker)
        {
            _workerOptions = workerOptions.Value;
            _logger = logger;
            _eventWorker = eventWorker;
            _reactionWorker = reactionWorker;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("WorkerManager run worker pull at: {time}", DateTimeOffset.Now);

                List<Task> tasks = new()
                {
                    Task.Run(async () => await _eventWorker.Run(cancellationToken), cancellationToken),
                    Task.Run(async () => await _reactionWorker.Run(cancellationToken), cancellationToken)
                };
                Task.WhenAll(tasks).Wait(cancellationToken);

                _logger.LogInformation("WorkerManager finish worker pull at: {time}", DateTimeOffset.Now);
                await Task.Delay(_workerOptions.DelayMs, cancellationToken);
            }
        }
    }
}
