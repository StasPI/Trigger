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
        [JsonIgnore]
        public List<string>? CaseEventStr { get; set; }
        public List<JsonObject> CaseEvent { get; set; }
        [JsonIgnore]
        public List<string>? CaseReactionStr { get; set; }
        public List<JsonObject> CaseReaction { get; set; }
    }
}
