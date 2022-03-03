using Contracts.Manager;
using Entities.Event;
using EntityFramework.Abstraction;

namespace CreateCase.Implementation
{
    public static class CreateEvent
    {
        public static async Task<CaseEventDto> Create(IDatabaseContext context, CaseEventDto caseEventDto, int useCasesId, CancellationToken cancellationToken)
        {
            caseEventDto.UseCasesID = useCasesId;
            switch (caseEventDto.Name)
            {
                case "Email":
                    caseEventDto.SourceId = (await context.SaveAsyncJsonObject<EmailSource>(caseEventDto.Source, cancellationToken)).Id;
                    caseEventDto.RuleId = (await context.SaveAsyncJsonObject<EmailRule>(caseEventDto.Rule, cancellationToken)).Id;
                    break;
                case "Site":
                    caseEventDto.SourceId = (await context.SaveAsyncJsonObject<SiteSource>(caseEventDto.Source, cancellationToken)).Id;
                    caseEventDto.RuleId = (await context.SaveAsyncJsonObject<SiteRule>(caseEventDto.Rule, cancellationToken)).Id;
                    break;
            }
            return caseEventDto;
        }
    }
}