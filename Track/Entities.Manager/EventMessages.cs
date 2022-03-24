using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Manager
{
    public class EventMessages : BaseModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public CaseEvent CaseEvent { get; set; }
    }
}
