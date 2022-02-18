using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entity.Reaction
{
    public class Email : BaseModel
    {
        [Required]
        public string Address { get; set; }
        public string Copy { get; set; }
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public string Attachment { get; set; }
    }
}
