using AutoMapper;
using Entities.Registration;
using EntityFramework.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Commands.Implementation
{
    public class DeleteUseCasesCommand : IRequest<int>
    {
        public int Id { get; set; }
        public class DeleteUseCasesCommandHandler : IRequestHandler<DeleteUseCasesCommand, int>
        {
            private readonly ILogger<DeleteUseCasesCommandHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IDatabaseContext _context;

            public DeleteUseCasesCommandHandler(ILogger<DeleteUseCasesCommandHandler> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
            {
                _logger = logger;
                _mapper = mapper;
                IServiceScope scope = scopeFactory.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
            }
            public async Task<int> Handle(DeleteUseCasesCommand command, CancellationToken cancellationToken)
            {
                UseCases useCases = await _context.UseCases.Where(x => (x.Id == command.Id) & (x.DateDeleted == null)).FirstAsync(cancellationToken);
                useCases.DateDeleted = DateTime.UtcNow;
                useCases.Active = false;
                useCases.SendEvent = false;
                useCases.SendReaction = false;

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("DeleteUseCasesCommandHandler delete UseCase {id} : {time}", useCases.Id, DateTimeOffset.Now);

                return useCases.Id;
            }
        }
    }
}
