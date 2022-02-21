using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Manager
{
    public class CaseReaction : BaseModel
    {
        [Required]
        public string ReactionTypeName { get; set; }
    }
}
