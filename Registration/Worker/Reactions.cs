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
    public class Reactions : UseCasesSendReactionDto, IReactions
    {
        private readonly ILogger<Reactions> _logger;
        private readonly IMapper _mapper;
        private readonly IDatabaseContext _context;

        public Reactions(ILogger<Reactions> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            IServiceScope scope = scopeFactory.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
        }

        public async Task<List<UseCasesSendReactionDto>> Get(int maxMessagesReactions, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ReactionWorker run at: {time}", DateTimeOffset.Now);

            List<UseCases> useCases = await _context.UseCases
                .Where(x => x.SendReaction == false)
                .Take(maxMessagesReactions)
                .ToListAsync(cancellationToken);

            List<UseCasesSendReactionDto> useCasesSendReactionDto = _mapper.Map<List<UseCasesSendReactionDto>>(useCases);

            Parallel.ForEach(useCasesSendReactionDto, async x => x.CaseReaction = await ConvertObject.ListStringToJsonObject(x.CaseReactionStr));

            return useCasesSendReactionDto;
        }
    }
}