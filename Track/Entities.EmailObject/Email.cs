using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.EmailObject
{
    public class Email : BaseModel
    {
        [Required]
        public int CaseEventId { get; set; }
        [Required]
        public EmailSource EmailSource { get; set; }
        [Required]
        public List<EmailRule> EmailRule { get; set; }
    }
}