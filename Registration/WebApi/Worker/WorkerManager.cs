using AutoMapper;
using EntityFramework.Abstraction;

namespace WebApi.Worker
{
    public class WorkerManager : BackgroundService
    {
        IServiceScopeFactory _scopeFactory;
        private IDatabaseContext _context;
        private readonly ILogger<WorkerManager> _logger;
        private readonly IMapper _mapper;

        public WorkerManager(ILogger<WorkerManager> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    
                    _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
