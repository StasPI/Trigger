using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entity.Event
{
    public class SiteRules : BaseModel
    {
        [Required]
        public string Text { get; set; }
    }
}
