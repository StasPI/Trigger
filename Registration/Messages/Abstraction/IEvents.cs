using Dto.Registration;

namespace Messages.Abstraction
{
    public interface IEvents
    {
        public Task<List<UseCasesSendEventDto>> GetMessageAsync(int maxMessagesEvents, CancellationToken cancellationToken);
        public Task CommitSendAsync(CancellationToken cancellationToken);
        public Task RollbackSendAsync(CancellationToken cancellationToken);
    }
}