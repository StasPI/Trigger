using System.ComponentModel.DataAnnotations;

namespace Contracts.Event
{
    public class EmailSourceDto
    {
        [Required]
        public string Protocol { get; set; }
        [Required]
        public string Host { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}