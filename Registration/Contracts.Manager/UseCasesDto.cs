namespace Contracts.Manager
{
    public class UseCasesDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CaseName { get; set; }
        public bool SendToEvent { get; set; }
        public bool SendToReaction { get; set; }
        public List<CaseEventDto> CaseEvent { get; set; }
        public List<CaseReactionDto> CaseReaction { get; set; }

    }
}
