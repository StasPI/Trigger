using System.Text.Json.Nodes;

namespace Dto.Registration
{
    public class UseCasesPostDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CaseName { get; set; }
        public List<string>? CaseEventStr { get; set; }
        public List<JsonObject> CaseEvent { get; set; }
        public List<string>? CaseReactionStr { get; set; }
        public List<JsonObject> CaseReaction { get; set; }
    }
}