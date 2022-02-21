using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Manager
{
    public class CaseEvent : BaseModel
    {
        [Required]
        public string EventTypeName { get; set; }
    }
}
