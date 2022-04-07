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
    public class Reactions : UseCasesSendReactionDto, IReactions
    {
        private readonly ILogger<Reactions> _logger;
        private readonly IMapper _mapper;
        private readonly IServiceScope _scope;
        private IDbContextTransaction transaction;

        public Reactions(ILogger<Reactions> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _scope = scopeFactory.CreateScope();
        }

        public async Task<List<UseCasesSendReactionDto>> GetMessageAsync(int maxMessagesReactions, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reactions Generates a list of unsent reactions Time: {time}", DateTimeOffset.Now);
            IDatabaseContext context = _scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
            transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            List<UseCases> useCases = await context.UseCases
                .Where(x => (x.SendReaction == false) && (x.SendEvent == true))
                .Take(maxMessagesReactions)
                .ToListAsync(cancellationToken);

            List<UseCasesSendReactionDto> useCasesSendReactionDto = _mapper.Map<List<UseCasesSendReactionDto>>(useCases);

            Parallel.ForEach(useCasesSendReactionDto, async x => x.CaseReaction = await ConvertObject.StringToJsonObjectAsync(x.CaseReactionStr));
            Parallel.ForEach(useCases, x => x.SendReaction = true);

            await context.SaveChangesAsync(cancellationToken);

            return useCasesSendReactionDto;
        }
        public async Task CommitSendAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reaction transaction commit Time: {time}", DateTimeOffset.Now);
            await transaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackSendAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reaction transaction rollback Time: {time}", DateTimeOffset.Now);
            await transaction.RollbackAsync(cancellationToken);
        }
    }
}