using System.ComponentModel.DataAnnotations;

namespace Contracts.Event
{
    public class SiteRuleDto
    {
        [Required]
        public string Text { get; set; }
    }
}
