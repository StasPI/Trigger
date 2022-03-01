using AutoMapper;
using EntityFramework.Abstraction;
using Worker.Implementation;

namespace WebApi.Worker
{
    public class WorkerManager : BackgroundService
    {
        readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<WorkerManager> _logger;
        private readonly IMapper _mapper;

        public WorkerManager(ILogger<WorkerManager> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                IDatabaseContext context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
                NotSentMessages worker = new(context);

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
