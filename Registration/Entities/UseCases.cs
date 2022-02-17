using Abstraction;
using System.ComponentModel.DataAnnotations;

namespace Entities.Manager
{
    public class UseCases : IEntity<int>
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        public virtual List<Cases> Cases { get; set; }

        [Required]
        public string CaseName { get; set; }
    }
}
