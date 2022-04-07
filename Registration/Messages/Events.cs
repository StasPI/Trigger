using AutoMapper;
using Dto.Registration;
using Entities.Registration;
using EntityFramework.Abstraction;
using Helps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Messages.Abstraction;

namespace Messages
{
    public class Events : UseCasesSendEventDto, IEvents
    {
        private readonly ILogger<Events> _logger;
        private readonly IMapper _mapper;
        private IDbContextTransaction transaction;
        private List<UseCases> useCases;
        private readonly IServiceScope _scope;

        public Events(ILogger<Events> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _scope = scopeFactory.CreateScope();
        }

        public async Task<List<UseCasesSendEventDto>> GetMessageAsync(int maxMessagesEvents, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Events Generates a list of unsent events Time: {time}", DateTimeOffset.Now);
            IDatabaseContext context = _scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
            transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            useCases = await context.UseCases
                .Where(x => (x.SendEvent == false))
                .Take(maxMessagesEvents)
                .ToListAsync(cancellationToken);

            List<UseCasesSendEventDto> useCasesSendEventDto = _mapper.Map<List<UseCasesSendEventDto>>(useCases);

            Parallel.ForEach(useCasesSendEventDto, async x => x.CaseEvent = await ConvertObject.StringToJsonObjectAsync(x.CaseEventStr));
            Parallel.ForEach(useCases, x => x.SendEvent = true);

            await context.SaveChangesAsync(cancellationToken);
            return useCasesSendEventDto;
        }

        public async Task CommitSendAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Event transaction commit Time: {time}", DateTimeOffset.Now);
            await transaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackSendAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Event transaction rollback Time: {time}", DateTimeOffset.Now);
            await transaction.RollbackAsync(cancellationToken);
        }
    }
}