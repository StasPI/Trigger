using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.SiteObject
{
    public class SiteRule : BaseModel
    {
        [Required]
        public int SiteId { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
