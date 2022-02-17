using Abstraction;

namespace Entities.Manager
{
    public class EventType : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
