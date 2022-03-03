using Contracts.Manager;
using Entities.Reaction;
using EntityFramework.Abstraction;

namespace CreateCase.Implementation
{
    public static class CreateReaction
    {
        public static async Task<CaseReactionDto> Create(IDatabaseContext context, CaseReactionDto caseReactionDto, int useCasesId, CancellationToken cancellationToken)
        {
            caseReactionDto.UseCasesID = useCasesId;
            switch (caseReactionDto.Name)
            {
                case "Email":
                    caseReactionDto.DestinationId = (await context.SaveAsyncJsonObject<EmailDestination>(caseReactionDto.Destination, cancellationToken)).Id;
                    break;
            }
            return caseReactionDto;
        }
    }
}
