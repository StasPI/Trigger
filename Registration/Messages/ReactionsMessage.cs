using AutoMapper;
using Dto.Registration;
using Entities.Registration;
using EntityFramework.Abstraction;
using Helps;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Messages
{
    public class ReactionsMessage : IRequest<Tuple<IDbContextTransaction, List<UseCasesSendReactionDto>>>
    {
        public int maxMessagesReactions;

        public class ReactionsHandler : IRequestHandler<ReactionsMessage, Tuple<IDbContextTransaction, List<UseCasesSendReactionDto>>>
        {
            private readonly ILogger<ReactionsHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IDatabaseContext _context;

            public ReactionsHandler(ILogger<ReactionsHandler> logger, IDatabaseContext context, IMapper mapper)
            {
                _logger = logger;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Tuple<IDbContextTransaction, List<UseCasesSendReactionDto>>> Handle(ReactionsMessage query, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Reactions Generates a list of unsent reactions Time: {time}", DateTimeOffset.Now);
                IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                List<UseCases> useCases = await _context.UseCases
                    .Where(x => (x.SendReaction == false) && (x.SendEvent == true))
                    .Take(query.maxMessagesReactions)
                    .ToListAsync(cancellationToken);

                List<UseCasesSendReactionDto> useCasesSendReactionDto = _mapper.Map<List<UseCasesSendReactionDto>>(useCases);

                Parallel.ForEach(useCasesSendReactionDto, async x => x.CaseReaction = await ConvertObject.StringToJsonObjectAsync(x.CaseReactionStr));
                Parallel.ForEach(useCases, x => x.SendReaction = true);

                await _context.SaveChangesAsync(cancellationToken);

                return Tuple.Create(transaction, useCasesSendReactionDto);
            }
        }
    }
}
