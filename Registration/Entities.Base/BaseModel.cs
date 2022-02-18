using Abstraction;

namespace Entities.Base
{
    public class BaseModel : IEntity<int>
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateDelete { get; set; }
    }
}