using AutoMapper;
using Contracts.Manager;
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
                using IServiceScope scope = _scopeFactory.CreateScope();
                IDatabaseContext context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();

                NotSentMessages notSentMessages = new(context, _mapper);
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

                await Task.Delay(10000, cancellationToken);
            }
        }
    }
}
