using Abstraction;

namespace Entities.Manager
{
    public class ReactionType : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
