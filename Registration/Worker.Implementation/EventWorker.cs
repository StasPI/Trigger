using AutoMapper;
using Entities.Registration;
using EntityFramework.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Worker.Abstraction;

namespace Worker.Implementation
{
    public class EventWorker : IEventWorker
    {
        private readonly ILogger<EventWorker> _logger;
        private readonly IMapper _mapper;
        private readonly IDatabaseContext _context;
        private readonly EventWorkerOptions _workerOptions;

        public EventWorker(ILogger<EventWorker> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            IServiceScope scope = scopeFactory.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
            _workerOptions = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<EventWorkerOptions>>().Value;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EventWorker run at: {time}", DateTimeOffset.Now);

            List<UseCases> useCases = await _context.UseCases
                .Where(x => x.SendEvent == false)
                .Take(_workerOptions.MaxNumberOfMessages)
                .ToListAsync(cancellationToken);
        }
    }
}