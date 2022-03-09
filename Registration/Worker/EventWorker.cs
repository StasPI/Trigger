using AutoMapper;
using Dto.Registration;
using Entities.Registration;
using EntityFramework.Abstraction;
using Helps;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Worker.Abstraction;

namespace Worker
{
    public class EventWorker : UseCasesSendEventDto, IEventWorker
    {
        private readonly ILogger<EventWorker> _logger;
        private readonly IMapper _mapper;
        private readonly IDatabaseContext _context;
        private readonly EventWorkerOptions _workerOptions;

        public EventWorker(ILogger<EventWorker> logger, IOptions<EventWorkerOptions> workerOptions, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            IServiceScope scope = scopeFactory.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
            _workerOptions = workerOptions.Value;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EventWorker run at: {time}", DateTimeOffset.Now);

            List<UseCases> useCases = await _context.UseCases
                .Where(x => x.SendEvent == false)
                .Take(_workerOptions.MaxNumberOfMessages)
                .ToListAsync(cancellationToken);

            List<UseCasesSendEventDto> useCasesSendEventDto = _mapper.Map<List<UseCasesSendEventDto>>(useCases);

            Parallel.ForEach(useCasesSendEventDto, async x => x.CaseEvent = await ConvertObject.ListStringToJsonObject(x.CaseEventStr));

            Console.WriteLine("send");
        }
    }
}