using System.ComponentModel.DataAnnotations;

namespace Contracts.Event
{
    public class SiteSourceDto
    {
        [Required]
        public string Url { get; set; }
    }
}
