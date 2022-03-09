using Dto.Registration;

namespace Worker.Abstraction
{
    public interface IReactions
    {
        public Task<List<UseCasesSendReactionDto>> Get(int maxMessagesReactions, CancellationToken cancellationToken);
    }
}
