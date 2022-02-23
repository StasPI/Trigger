using Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Entities.Manager
{
    public class CaseEvent : BaseModel
    {
        private string _source;
        private string _rule;

        [Required]
        public string Name { get; set; }
        public int SourceId { get; set; }
        public int RuleId { get; set; }

        //[NotMapped]
        public int UseCasesID { get; set; }

        [NotMapped]
        public JsonObject Source
        {
            get => JsonSerializer.Deserialize<JsonObject>(string.IsNullOrEmpty(_source) ? "{}" : _source);
            set => _source = value.ToString();
        }

        [NotMapped]
        public JsonObject Rule
        {
            get => JsonSerializer.Deserialize<JsonObject>(string.IsNullOrEmpty(_rule) ? "{}" : _rule);
            set => _rule = value.ToString();
        }

    }
}
