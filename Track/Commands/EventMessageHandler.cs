using MediatR;
using Messages;
using Microsoft.Extensions.Logging;

namespace Commands
{
    public class EventMessageHandler : IRequestHandler<EventMessage>
    {
        private readonly ILogger<EventMessageHandler> _logger;

        public EventMessageHandler(ILogger<EventMessageHandler> logger) => _logger = logger;

        public Task<Unit> Handle(EventMessage request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("---- Received message: {Message} ----", request.EventMessages);
            return Task.FromResult(Unit.Value);
        }
    }
}