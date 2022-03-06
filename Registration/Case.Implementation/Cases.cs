using AutoMapper;
using Contracts.Manager;
using EntityFramework.Abstraction;

namespace Case.Implementation
{
    public class Cases
    {
        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly Events _events;
        private readonly Reactions _reactions;

        public Cases(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _events = new(_context, _mapper);
            _reactions = new(_context, _mapper);
        }

        public async Task FillEventsInUseCasesAsync(List<UseCasesDto> useCasesDto, CancellationToken cancellationToken)
        {
            foreach (UseCasesDto useCase in useCasesDto)
            {
                foreach (CaseEventDto caseEventDto in useCase.CaseEvent)
                {
                    await _events.FillCaseEventAsync(caseEventDto, cancellationToken);
                }
            }
        }

        public async Task FillRulesInUseCasesAsync(List<UseCasesDto> useCasesDto, CancellationToken cancellationToken)
        {
            foreach (UseCasesDto useCase in useCasesDto)
            {
                foreach (CaseReactionDto caseReactionDto in useCase.CaseReaction)
                {
                    await _reactions.FillaseReactionAsync(caseReactionDto, cancellationToken);
                }
            }
        }
    }
}
