using AutoMapper;
using Contracts.Manager;
using EntityFramework.Abstraction;

namespace Case.Implementation
{
    public static class Cases
    {
        public static async Task FillEventsInUseCasesAsync(IDatabaseContext context, IMapper mapper, List<UseCasesDto> useCasesDto, CancellationToken cancellationToken)
        {
            foreach (UseCasesDto useCase in useCasesDto)
            {
                foreach (CaseEventDto caseEventDto in useCase.CaseEvent)
                {
                    await Events.FillCaseEventAsync(context, mapper, caseEventDto, cancellationToken);
                }
            }
        }
        public static async Task FillRulesInUseCasesAsync(IDatabaseContext context, IMapper mapper, List<UseCasesDto> useCasesDto, CancellationToken cancellationToken)
        {
            foreach (UseCasesDto useCase in useCasesDto)
            {
                foreach (CaseReactionDto caseReactionDto in useCase.CaseReaction)
                {
                    await Reactions.FillaseReactionAsync(context, mapper, caseReactionDto, cancellationToken);
                }
            }
        }
    }
}
