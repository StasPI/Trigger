using System.Text.Json.Nodes;

namespace Dto.Registration
{
    public class UseCasesPostDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CaseName { get; set; }
        public bool Active { get; set; }
        public string? CaseEventStr { get; set; }
        public JsonObject CaseEvent { get; set; }
        public string? CaseReactionStr { get; set; }
        public JsonObject CaseReaction { get; set; }
    }
}