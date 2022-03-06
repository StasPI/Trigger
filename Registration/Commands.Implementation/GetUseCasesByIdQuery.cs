using AutoMapper;
using Case.Implementation;
using Contracts.Manager;
using Entities.Manager;
using EntityFramework.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Commands.Implementation
{
    public class GetUseCasesByIdQuery : UseCasesDto, IRequest<UseCasesDto>
    {
        public class GetUseCasesByIdQueryHandler : IRequestHandler<GetUseCasesByIdQuery, UseCasesDto>
        {
            private readonly IMapper _mapper;
            private readonly IDatabaseContext _context;
            private readonly Events _events;
            private readonly Reactions _reactions;

            public GetUseCasesByIdQueryHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
            {
                IServiceScope scope = scopeFactory.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
                _mapper = mapper;
                _events = new(_context, _mapper);
                _reactions = new(_context, _mapper);
            }

            public async Task<UseCasesDto> Handle(GetUseCasesByIdQuery query, CancellationToken cancellationToken)
            {
                UseCases useCases = await _context.UseCases.Where(x => x.Id == query.Id).FirstAsync(cancellationToken);
                useCases.CaseEvent = await _context.CaseEvents.Where(x => x.UseCasesID == useCases.Id).ToListAsync(cancellationToken);
                useCases.CaseReaction = await _context.CaseReaction.Where(x => x.UseCasesID == useCases.Id).ToListAsync(cancellationToken);

                UseCasesDto useCasesDto = _mapper.Map<UseCasesDto>(useCases);

                await EventAsync(useCasesDto.CaseEvent, cancellationToken);
                await ReactionAsync(useCasesDto.CaseReaction, cancellationToken);

                return useCasesDto;
            }

            private async Task EventAsync(List<CaseEventDto> caseEvents, CancellationToken cancellationToken)
            {
                foreach (CaseEventDto caseEvent in caseEvents)
                {
                    await _events.FillCaseEventAsync(caseEvent, cancellationToken);
                }
            }

            private async Task ReactionAsync(List<CaseReactionDto> caseReactions, CancellationToken cancellationToken)
            {
                foreach (CaseReactionDto caseReaction in caseReactions)
                {
                    await _reactions.FillaseReactionAsync(caseReaction, cancellationToken);
                }
            }
        }
    }
}
