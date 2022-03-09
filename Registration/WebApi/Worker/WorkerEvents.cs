using Dto.Registration;
using Microsoft.Extensions.Options;
using WebApi.Worker.Options;
using Worker.Abstraction;

namespace WebApi.Worker
{
    public class WorkerEvents : BackgroundService
    {
        private readonly ILogger<WorkerEvents> _logger;
        private readonly IEvents _events;
        private readonly WorkerOptions _options;

        public WorkerEvents(ILogger<WorkerEvents> logger, IEvents events, IOptions<WorkerOptions> options)
        {
            _logger = logger;
            _events = events;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("WorkerEvents run at: {time}", DateTimeOffset.Now);
                List<UseCasesSendEventDto> useCasesSendEventDto = await _events.Get(_options.Events.MaxMessages, cancellationToken);
            }
        }
    }
}
