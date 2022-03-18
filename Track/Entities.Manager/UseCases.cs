using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Manager
{
    public class UseCases : BaseModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public bool Active { get; set; }

        [Required]
        public virtual List<CaseEvent> CaseEvent { get; set; }

    }
}
