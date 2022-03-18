using Entities.Registration;
using EntityFramework.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Commands
{
	public class DeleteUseCasesCommand : IRequest<int>
    {
        public int Id { get; set; }
        public class DeleteUseCasesCommandHandler : IRequestHandler<DeleteUseCasesCommand, int>
        {
            private readonly ILogger<DeleteUseCasesCommandHandler> _logger;
            private readonly IDatabaseContext _context;

            public DeleteUseCasesCommandHandler(ILogger<DeleteUseCasesCommandHandler> logger, IServiceScopeFactory scopeFactory)
            {
                _logger = logger;
                IServiceScope scope = scopeFactory.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
            }
            public async Task<int> Handle(DeleteUseCasesCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    _logger.LogInformation("DeleteUseCasesCommandHandler Delete UseCase: {id} | Time: {time}", command.Id, DateTimeOffset.Now);
                    UseCases useCases = await _context.UseCases.Where(x => (x.Id == command.Id) & (x.DateDeleted == null)).FirstAsync(cancellationToken);

                    useCases.DateDeleted = DateTime.UtcNow;
                    useCases.Active = false;
                    useCases.SendEvent = false;
                    useCases.SendReaction = false;

                    await _context.SaveChangesAsync(cancellationToken);

                    return useCases.Id;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("DeleteUseCasesCommandHandler Exception UseCase: {id} | Time: {time} | Error: {ex} ", command.Id, DateTimeOffset.Now, ex);
                    return -1;
                }
            }
        }
    }
}
