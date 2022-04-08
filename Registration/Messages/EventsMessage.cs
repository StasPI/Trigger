using AutoMapper;
using Dto.Registration;
using Entities.Registration;
using EntityFramework.Abstraction;
using Helps;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Messages
{
    public class EventsMessage : IRequest<TransitBody<EventMessageBody>>
    {
        public int maxMessagesEvents;
        public class EventsHandler : IRequestHandler<EventsMessage, TransitBody<EventMessageBody>>
        {
            private readonly ILogger<EventsHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IDatabaseContext _context;

            public EventsHandler(ILogger<EventsHandler> logger, IDatabaseContext context, IMapper mapper)
            {
                _logger = logger;
                _mapper = mapper;
                _context = context;
            }

            public async Task<TransitBody<EventMessageBody>> Handle(EventsMessage query, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Events Generates a list of unsent events Time: {time}", DateTimeOffset.Now);

                TransitBody<EventMessageBody> transitBody = new()
                {
                    Messages = new(),
                    Transaction = await _context.Database.BeginTransactionAsync(cancellationToken)
                };

                List<UseCases> useCases = await _context.UseCases
                    .Where(x => (x.SendEvent == false))
                    .Take(query.maxMessagesEvents)
                    .ToListAsync(cancellationToken);

                transitBody.Messages.EventMessages = _mapper.Map<List<UseCasesSendEventDto>>(useCases);

                Parallel.ForEach(transitBody.Messages.EventMessages, 
                    async x => x.CaseEvent = await ConvertObject.StringToJsonObjectAsync(x.CaseEventStr));

                Parallel.ForEach(useCases, x => x.SendEvent = true);

                await _context.SaveChangesAsync(cancellationToken);

                return transitBody;
            }
        }
    }
}