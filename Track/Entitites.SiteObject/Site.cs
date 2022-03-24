using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.SiteObject
{
    public class Site : BaseModel
    {
        [Required]
        public SiteSource SiteSource { get; set; }
        [Required]
        public virtual List<SiteRule> SiteRule { get; set; }
    }
}