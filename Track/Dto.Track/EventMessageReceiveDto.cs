using Entities.Manager;

namespace Dto.Track
{
    public class EventMessageReceiveDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
        public int Status { get; set; }
        public CaseEvent CaseEvent { get; set; }
    }
}