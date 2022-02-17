using Abstraction;
using System.ComponentModel.DataAnnotations;

namespace Entity.Reaction
{
    public class Email : IEntity<int>
    {
        public int Id { get; set; }
        [Required]
        public string Address { get; set; }
        public string Copy { get; set; }
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public string Attachment { get; set; }
    }
}
