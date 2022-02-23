using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Event
{
    public class SiteRules : BaseModel
    {
        [Required]
        public string Text { get; set; }
    }
}
