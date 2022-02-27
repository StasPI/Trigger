namespace Contracts.Manager
{
    public class UseCasesDto
    {
        public int UserId { get; set; }
        public string CaseName { get; set; }
        public List<CaseEventDto> CaseEvent { get; set; }
        public List<CaseReactionDto> CaseReaction { get; set; }
    }
}
