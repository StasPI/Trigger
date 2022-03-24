using Entities.Base;
using Entities.EmailObject;
using Entities.SiteObject;

namespace Entities.Manager
{
    public class CaseEvent : BaseModel
    {
        public virtual List<Email>? Email { get; set; }
        public virtual List<Site>? Site { get; set; }
    }
}
