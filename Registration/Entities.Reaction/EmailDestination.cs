using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Reaction
{
    public class EmailDestination : BaseModel
    {
        [Required]
        public string Address { get; set; }
        public string? Copy { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public string? Attachment { get; set; }
    }
}
