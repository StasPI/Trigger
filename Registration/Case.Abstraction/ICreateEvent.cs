using Contracts.Manager;

namespace Case.Abstraction
{
    public interface ICreateEvent
    {
        Task<CaseEventDto> Create(CaseEventDto caseEventDto, int useCasesId, CancellationToken cancellationToken);
    }
}