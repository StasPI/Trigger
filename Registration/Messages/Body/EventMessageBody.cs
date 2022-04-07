using Dto.Registration;

namespace Messages
{
    public class EventMessageBody
    {
        public List<UseCasesSendEventDto> EventMessages { get; set; }
    }
}
