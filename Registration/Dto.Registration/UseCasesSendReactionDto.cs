using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Dto.Registration
{
    public class UseCasesSendReactionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
        [JsonIgnore]
        public List<string>? CaseReactionStr { get; set; }
        public List<JsonObject> CaseReaction { get; set; }
    }
}
