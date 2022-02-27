using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Contracts.Manager
{
    public class CaseReactionDto
    {
        private string _destination;
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        public int DestinationId { get; set; }
        public int UseCasesID { get; set; }

        [NotMapped]
        public JsonObject Destination
        {
            get => JsonSerializer.Deserialize<JsonObject>(string.IsNullOrEmpty(_destination) ? "{}" : _destination);
            set => _destination = value.ToString();
        }
    }
}
