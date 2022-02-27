using System.ComponentModel.DataAnnotations;

namespace Contracts.Reaction
{
    public class EmailDestinationDto
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