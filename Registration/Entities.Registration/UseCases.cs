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
    }
}