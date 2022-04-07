using AutoMapper;
using Dto.Registration;
using Entities.Registration;
using EntityFramework.Abstraction;
using Helps;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Messages
{
    public class EventsMessage : IRequest<Tuple<IDbContextTransaction, List<UseCasesSendEventDto>>>
    {
        public int maxMessagesEvents;
        public class EventsHandler : IRequestHandler<EventsMessage, Tuple<IDbContextTransaction, List<UseCasesSendEventDto>>>
        {
            private readonly ILogger<EventsHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IDatabaseContext _context;
            private List<UseCases> useCases;

            public EventsHandler(ILogger<EventsHandler> logger, IDatabaseContext context, IMapper mapper)
            {
                _logger = logger;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Tuple<IDbContextTransaction, List<UseCasesSendEventDto>>> Handle(EventsMessage query, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Events Generates a list of unsent events Time: {time}", DateTimeOffset.Now);
                IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                useCases = await _context.UseCases
                    .Where(x => (x.SendEvent == false))
                    .Take(query.maxMessagesEvents)
                    .ToListAsync(cancellationToken);

                List<UseCasesSendEventDto> useCasesSendEventDto = _mapper.Map<List<UseCasesSendEventDto>>(useCases);

                Parallel.ForEach(useCasesSendEventDto, async x => x.CaseEvent = await ConvertObject.StringToJsonObjectAsync(x.CaseEventStr));
                Parallel.ForEach(useCases, x => x.SendEvent = true);

                await _context.SaveChangesAsync(cancellationToken);

                return Tuple.Create(transaction, useCasesSendEventDto);
            }
        }
    }
}