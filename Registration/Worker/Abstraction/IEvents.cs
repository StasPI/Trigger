using Dto.Registration;

namespace Worker.Abstraction
{
    public interface IEvents
    {
        public Task<List<UseCasesSendEventDto>> Get(int maxMessagesEvents, CancellationToken cancellationToken);
    }
}