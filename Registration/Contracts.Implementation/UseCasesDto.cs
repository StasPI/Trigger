using Entities.Manager;

namespace Contracts.Implementation
{
    public class UseCasesDto
    {
        public int UserId { get; set; }
        public string CaseName { get; set; }
        public List<CaseEvent> CaseEvent { get; set; }
        public List<CaseReaction> CaseReaction { get; set; }
    }
}