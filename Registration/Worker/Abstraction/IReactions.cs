using Dto.Registration;

namespace Worker.Abstraction
{
    public interface IReactions
    {
        public Task<List<UseCasesSendReactionDto>> GetMessageAsync(int maxMessagesReactions, CancellationToken cancellationToken);
        public Task CommitAsync(CancellationToken cancellationToken);
        public Task RollbackAsync(CancellationToken cancellationToken);
    }
}
