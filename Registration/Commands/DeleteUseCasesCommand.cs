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
            private readonly IServiceScope _scope;

            public DeleteUseCasesCommandHandler(ILogger<DeleteUseCasesCommandHandler> logger, IServiceScopeFactory scopeFactory)
            {
                _logger = logger;
                _scope = scopeFactory.CreateScope();
            }
            public async Task<int> Handle(DeleteUseCasesCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    IDatabaseContext context = _scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
                    _logger.LogInformation("DeleteUseCasesCommandHandler Delete UseCase: {id} | Time: {time}", command.Id, DateTimeOffset.Now);
                    UseCases useCases = await context.UseCases.Where(x => (x.Id == command.Id) & (x.DateDeleted == null)).FirstAsync(cancellationToken);

                    useCases.DateDeleted = DateTime.UtcNow;
                    useCases.DateUpdated = DateTime.UtcNow;
                    useCases.Active = false;
                    useCases.SendEvent = false;
                    useCases.SendReaction = false;

                    await context.SaveChangesAsync(cancellationToken);

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
