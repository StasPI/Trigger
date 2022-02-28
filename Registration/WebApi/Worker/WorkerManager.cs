using AutoMapper;
using EntityFramework.Abstraction;

namespace WebApi.Worker
{
    public class WorkerManager : BackgroundService
    {
        private readonly ILogger<WorkerManager> _logger;
        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;

        public WorkerManager(ILogger<WorkerManager> logger, IDatabaseContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
