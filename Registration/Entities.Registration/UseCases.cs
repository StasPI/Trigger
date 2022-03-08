using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Registration
{
    public class UseCases : BaseModel
    {
        public int UserId { get; set; }
        [Required]
        public string CaseName { get; set; }
        [Required]
        public List<string> CaseEvent { get; set; }
        [Required]
        public List<string> CaseReaction { get; set; }
        public bool SendEvent { get; set; } = false;
        public bool SendReaction { get; set; } = false;
    }
}