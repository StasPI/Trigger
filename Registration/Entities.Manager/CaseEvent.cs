using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Manager
{
    public class CaseEvent : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public int SourceId { get; set; }
        public int RuleId { get; set; }
        [Required]
        public int UseCasesID { get; set; }
    }
}
