using Abstraction;
using System.ComponentModel.DataAnnotations;

namespace Entity.Event
{
    public class SiteSource : IEntity<int>
    {
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
    }
}
