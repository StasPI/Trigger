using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Dto.Registration
{
    public class UseCasesGetDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CaseName { get; set; }
        public bool Active { get; set; }
        [JsonIgnore]
        public string? CaseEventStr { get; set; }
        public JsonObject CaseEvent { get; set; }
        [JsonIgnore]
        public string? CaseReactionStr { get; set; }
        public JsonObject CaseReaction { get; set; }
    }
}
