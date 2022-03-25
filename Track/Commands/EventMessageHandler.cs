using AutoMapper;
using Entities.Manager;
using EntityFramework.Abstraction;
using MediatR;
using Messages;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Commands
{
    public class EventMessageHandler : IRequestHandler<EventMessage>
    {
        private readonly ILogger<EventMessageHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDatabaseContext _context;

        public EventMessageHandler(ILogger<EventMessageHandler> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _logger = logger;
            IServiceScope scope = scopeFactory.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
            _mapper = mapper;
        }

        public async Task<Unit> Handle(EventMessage request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("EventMessageHandler Received message: {Message}", request.EventMessages);
            using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            List<UseCases> useCases = _mapper.Map<List<UseCases>>(request.EventMessages);

            await _context.UseCases.AddRangeAsync(useCases, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return await Task.FromResult(Unit.Value);
        }
    }
}