using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Dto.Registration
{
    public class UseCasesSendEventDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
        [JsonIgnore]
        public List<string>? CaseEventStr { get; set; }
        public List<JsonObject> CaseEvent { get; set; }
    }
}
