using Abstraction;

namespace Entity.Event
{
    public class EmailRules : IEntity<int>
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Copy { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Attachment { get; set; }
        public bool Outgoing { get; set; }
        public bool Access { get; set; }
    }
}
