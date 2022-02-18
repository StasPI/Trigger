using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Manager
{
    public class UseCases : BaseModel
    {
        public int UserId { get; set; }

        [Required]
        public virtual List<Cases> Cases { get; set; }

        [Required]
        public string CaseName { get; set; }
    }
}
