using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Manager
{
    public class UseCases : BaseModel
    {
        public int UserId { get; set; }
        [Required]
        public string CaseName { get; set; }
        public bool SendToEvent { get; set; } = false;
        public bool SendToReaction { get; set; } = false;

        [Required]
        public virtual List<CaseEvent> CaseEvent { get; set; }
        [Required]
        public virtual List<CaseReaction> CaseReaction { get; set; }

    }
}
