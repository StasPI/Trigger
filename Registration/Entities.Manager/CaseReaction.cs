using Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Entities.Manager
{
    public class CaseReaction : BaseModel
    {
        private string _destination;
        [Required]
        public string Name { get; set; }
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
