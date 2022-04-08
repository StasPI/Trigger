using Dto.Registration;

namespace Messages
{
    public class ReactionMessageBody
    {
        public List<UseCasesSendReactionDto>? ReactionMessages { get; set; }
    }
}