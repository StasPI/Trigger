using Entities.Base;
using Entities.EmailObject;
using Entities.SiteObject;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Manager
{
    public class CaseEvent : BaseModel
    {
        [Required]
        public int UseCasesID { get; set; }
        [NotMapped]
        public override bool Active { get; set; }
        [NotMapped]
        public override int Status { get; set; }
        public List<Email>? Email { get; set; }
        public List<Site>? Site { get; set; }
    }
}
