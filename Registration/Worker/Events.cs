using AutoMapper;
using Dto.Registration;
using Entities.Registration;
using EntityFramework.Abstraction;
using Helps;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Worker.Abstraction;

namespace Worker
{
    public class Events : UseCasesSendEventDto, IEvents
    {
        private readonly ILogger<Events> _logger;
        private readonly IMapper _mapper;
        private readonly IDatabaseContext _context;

        public Events(ILogger<Events> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            IServiceScope scope = scopeFactory.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
        }

        public async Task<List<UseCasesSendEventDto>> Get(int maxMessagesEvents, CancellationToken cancellationToken)
        {
            _logger.LogInformation("EventWorker run at: {time}", DateTimeOffset.Now);

            List<UseCases> useCases = await _context.UseCases
                .Where(x => x.SendEvent == false)
                .Take(maxMessagesEvents)
                .ToListAsync(cancellationToken);

            List<UseCasesSendEventDto> useCasesSendEventDto = _mapper.Map<List<UseCasesSendEventDto>>(useCases);

            Parallel.ForEach(useCasesSendEventDto, async x => x.CaseEvent = await ConvertObject.ListStringToJsonObject(x.CaseEventStr));

            return useCasesSendEventDto;
        }
    }
}