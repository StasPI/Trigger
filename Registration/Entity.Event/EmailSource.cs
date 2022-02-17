using Abstraction;
using System.ComponentModel.DataAnnotations;

namespace Entity.Event
{
    public class EmailSource : IEntity<int>
    {
        public int Id { get; set; }
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