using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Registration
{
    public class UseCases : BaseModel
    {
        public int UserId { get; set; }
        [Required]
        public string CaseName { get; set; } = "default";
        public bool Active { get; set; } = false;
        [Required]
        public List<string> CaseEvent { get; set; } = new List<string>();
        [Required]
        public List<string> CaseReaction { get; set; } = new List<string>();
        public bool SendEvent { get; set; } = false;
        public bool SendReaction { get; set; } = false;
    }
}