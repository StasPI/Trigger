using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.SiteObject
{
    public class Site : BaseModel
    {
        [Required]
        public int CaseEventId { get; set; }
        [Required]
        public SiteSource SiteSource { get; set; }
        [Required]
        public List<SiteRule> SiteRule { get; set; }
    }
}