using AutoMapper;
using Dto.Registration;
using Entities.Registration;
using EntityFramework.Abstraction;
using Helps;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Messages
{
    public class ReactionsMessage : IRequest<TransitBody<UseCasesSendReactionDto>>
    {
        public int maxMessagesReactions;

        public class ReactionsHandler : IRequestHandler<ReactionsMessage, TransitBody<UseCasesSendReactionDto>>
        {
            private readonly ILogger<ReactionsHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IDatabaseContext _context;
            private TransitBody<UseCasesSendReactionDto> _transitBody;

            public ReactionsHandler(ILogger<ReactionsHandler> logger, IDatabaseContext context, IMapper mapper)
            {
                _logger = logger;
                _mapper = mapper;
                _context = context;
                _transitBody = new();
            }

            public async Task<TransitBody<UseCasesSendReactionDto>> Handle(ReactionsMessage query, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Reactions Generates a list of unsent reactions Time: {time}", DateTimeOffset.Now);
                _transitBody.Transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                List<UseCases> useCases = await _context.UseCases
                    .Where(x => (x.SendReaction == false) && (x.SendEvent == true))
                    .Take(query.maxMessagesReactions)
                    .ToListAsync(cancellationToken);

                _transitBody.Messages = _mapper.Map<List<UseCasesSendReactionDto>>(useCases);

                Parallel.ForEach(_transitBody.Messages, async x => x.CaseReaction = await ConvertObject.StringToJsonObjectAsync(x.CaseReactionStr));
                Parallel.ForEach(useCases, x => x.SendReaction = true);

                await _context.SaveChangesAsync(cancellationToken);

                return _transitBody;
            }
        }
    }
}
