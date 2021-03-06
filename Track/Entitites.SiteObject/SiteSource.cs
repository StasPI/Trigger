using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.SiteObject
{
    public class SiteSource : BaseModel
    {
        [Required]
        public int SiteId { get; set; }
        [Required]
        public string Url { get; set; }
    }
}
