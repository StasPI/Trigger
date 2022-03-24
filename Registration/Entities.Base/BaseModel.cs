using Entities.Base.Abstraction;
using System.ComponentModel.DataAnnotations;

namespace Entities.Base
{
    public class BaseModel : IEntity
    {
        [Key]
        public int Id { get; set; }
        public bool Active { get; set; } = false;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}