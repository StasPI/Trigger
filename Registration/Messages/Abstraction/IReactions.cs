using Dto.Registration;

namespace Messages.Abstraction
{
    public interface IReactions
    {
        public Task<List<UseCasesSendReactionDto>> GetMessageAsync(int maxMessagesReactions, CancellationToken cancellationToken);
        public Task CommitSendAsync(CancellationToken cancellationToken);
        public Task RollbackSendAsync(CancellationToken cancellationToken);
    }
}
