using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.EmailObject
{
    public class EmailSource : BaseModel
    {
        [Required]
        public int EmailId { get; set; }
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
