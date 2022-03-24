using Dto.Track;
using MediatR;

namespace Messages
{
    public class EventMessage : IRequest<Unit>
    {
        public List<EventMessageReceiveDto> EventMessages { get; set; }
    }
}