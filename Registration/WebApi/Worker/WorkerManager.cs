using AutoMapper;
using Contracts.Manager;
using EntityFramework.Abstraction;
using Microsoft.Extensions.Options;
using Worker.Implementation;

namespace WebApi.Worker
{
    public class WorkerManager : BackgroundService
    {
        private readonly ILogger<WorkerManager> _logger;
        private readonly IMapper _mapper;
        private readonly IDatabaseContext _context;
        private readonly WorkerOptions _workerOptions;
        public WorkerManager(ILogger<WorkerManager> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            IServiceScope scope = scopeFactory.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
            _workerOptions = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<WorkerOptions>>().Value;
            _logger = logger;
            _mapper = mapper;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                NotSentMessages notSentMessages = new(_workerOptions, _context, _mapper);
                List<UseCasesDto> casesEvent = await notSentMessages.GetEventAsync(cancellationToken);
                List<UseCasesDto> casesReaction = await notSentMessages.GetReactionAsync(cancellationToken);

                if (casesEvent != null)
                {
                    Console.WriteLine("casesEvent");
                }
                if (casesReaction != null)
                {
                    Console.WriteLine("casesReaction");
                }

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await Task.Delay(_workerOptions.DelayMs, cancellationToken);
            }
        }
    }
}
