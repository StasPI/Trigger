using AutoMapper;
using Case.Implementation;
using Contracts.Manager;
using Entities.Manager;
using EntityFramework.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commands.Implementation
{
    public class GetUseCasesByIdQuery : UseCasesDto, IRequest<UseCasesDto>
    {
        public int Id { get; set; }
        public class GetUseCasesByIdQueryHandler : IRequestHandler<GetUseCasesByIdQuery, UseCasesDto>
        {
            private readonly IDatabaseContext _context;
            private readonly IMapper _mapper;
            private readonly GetEvent _getEvent;
            private readonly GetReaction _getReaction;
            public GetUseCasesByIdQueryHandler(IDatabaseContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
                _getEvent = new GetEvent(_context, _mapper);
                _getReaction = new GetReaction(_context, _mapper);
            }
            public async Task<UseCasesDto> Handle(GetUseCasesByIdQuery query, CancellationToken cancellationToken)
            {
                UseCases useCases = await _context.UseCases.Where(x => x.Id == query.Id).FirstAsync(cancellationToken);
                useCases.CaseEvent = await _context.CaseEvents.Where(x => x.UseCasesID == useCases.Id).ToListAsync(cancellationToken);
                useCases.CaseReaction = await _context.CaseReaction.Where(x => x.UseCasesID == useCases.Id).ToListAsync(cancellationToken);

                UseCasesDto useCasesDto = _mapper.Map<UseCasesDto>(useCases);

                await EventAsync(useCasesDto.CaseEvent, cancellationToken);
                await ReactionAsync(useCasesDto.CaseReaction, cancellationToken);

                //if (useCases == null) return null;

                return useCasesDto;
            }

            private async Task EventAsync(List<CaseEventDto> caseEvents, CancellationToken cancellationToken)
            {
                foreach (CaseEventDto caseEvent in caseEvents)
                {
                    await _getEvent.Get(caseEvent, cancellationToken);
                }
            }

            private async Task ReactionAsync(List<CaseReactionDto> caseReactions, CancellationToken cancellationToken)
            {
                foreach (CaseReactionDto caseReaction in caseReactions)
                {
                    await _getReaction.Get(caseReaction, cancellationToken);
                }
            }
        }
    }
}
