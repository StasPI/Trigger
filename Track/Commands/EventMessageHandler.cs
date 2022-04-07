using AutoMapper;
using Entities.Manager;
using EntityFramework.Abstraction;
using MediatR;
using Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Commands
{
    public class EventMessageHandler : IRequestHandler<EventMessage>
    {
        private readonly ILogger<EventMessageHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IServiceScope _scope;

        public EventMessageHandler(ILogger<EventMessageHandler> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _logger = logger;
            _scope = scopeFactory.CreateScope();
            _mapper = mapper;
        }

        public async Task<Unit> Handle(EventMessage request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("EventMessageHandler Received message: {Message}", request.EventMessages);
            using IDatabaseContext context = _scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            foreach(var uc in request.EventMessages)
            {
                UseCases useCases = await context.UseCases.Where(x => (x.Id == uc.Id)).FirstOrDefaultAsync(cancellationToken);
                if (useCases != null)
                {
                    //var a = _mapper.Map<UseCases>(uc);
                    //context.UseCases.Update(_mapper.Map<UseCases>(uc));
                    useCases.UserId = uc.UserId;
                    useCases.Active = uc.Active;
                    useCases.CaseEvent = uc.CaseEvent;
                    useCases.CaseEvent.Email = uc.CaseEvent.Email;
                    useCases.CaseEvent.Site = uc.CaseEvent.Site;
                    //useCases = _mapper.Map<UseCases>(uc);
                }
                else
                {
                    await context.UseCases.AddAsync(_mapper.Map<UseCases>(uc), cancellationToken);
                }
            }

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return await Task.FromResult(Unit.Value);
        }
    }
}