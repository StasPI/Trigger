using AutoMapper;
using Dto.Registration;
using Entities.Registration;
using EntityFramework.Abstraction;
using Helps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        private IDbContextTransaction transaction;
        private List<UseCases> useCases;

        public Events(ILogger<Events> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            IServiceScope scope = scopeFactory.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
        }

        public async Task<List<UseCasesSendEventDto>> GetMessageAsync(int maxMessagesEvents, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Generates a list of unsent events: {time}", DateTimeOffset.Now);
            transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            useCases = await _context.UseCases
                .Where(x => x.SendEvent == false)
                .Take(maxMessagesEvents)
                .ToListAsync(cancellationToken);

            List<UseCasesSendEventDto> useCasesSendEventDto = _mapper.Map<List<UseCasesSendEventDto>>(useCases);

            Parallel.ForEach(useCasesSendEventDto, async x => x.CaseEvent = await ConvertObject.ListStringToJsonObject(x.CaseEventStr));
            Parallel.ForEach(useCases, x => x.SendEvent = true);

            await _context.SaveChangesAsync(cancellationToken);

            return useCasesSendEventDto;
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Event transaction commit: {time}", DateTimeOffset.Now);
            await transaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Event transaction rollback: {time}", DateTimeOffset.Now);
            await transaction.RollbackAsync(cancellationToken);
        }
    }
}