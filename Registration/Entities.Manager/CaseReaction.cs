using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Manager
{
    public class CaseReaction : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public int DestinationId { get; set; }
        [Required]
        public int UseCasesID { get; set; }
    }
}
