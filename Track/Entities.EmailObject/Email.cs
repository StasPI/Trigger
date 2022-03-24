using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.EmailObject
{
    public class Email : BaseModel
    {
        [Required]
        public EmailSource EmailSource { get; set; }
        [Required]
        public virtual List<EmailRule> EmailRule { get; set; }
    }
}