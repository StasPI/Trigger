using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Event
{
    public class SiteSource : BaseModel
    {
        [Required]
        public string Url { get; set; }
    }
}
