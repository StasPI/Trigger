using Abstraction;
using System.ComponentModel.DataAnnotations;

namespace Entities.Base
{
    public class BaseModel : IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateCreate { get; init; } = DateTime.UtcNow;
        public DateTime? DateDelete { get; set; }
    }
}