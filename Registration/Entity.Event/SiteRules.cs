using Abstraction;
using System.ComponentModel.DataAnnotations;

namespace Entity.Event
{
    public class SiteRules : IEntity<int>
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
