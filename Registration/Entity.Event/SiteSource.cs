using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entity.Event
{
    public class SiteSource : BaseModel
    {
        [Required]
        public string Url { get; set; }
    }
}
