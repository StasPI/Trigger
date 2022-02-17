using Abstraction;

namespace Entities.Manager
{
    public class Cases : IEntity<int>
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public virtual List<EventType> EventType { get; set; }
        public int EventSourceID { get; set; }
        public int EventRuleID { get; set; }

        public virtual List<ReactionType> ReactionType { get; set; }
        public int ReactionTargetID { get; set; }
    }
}
