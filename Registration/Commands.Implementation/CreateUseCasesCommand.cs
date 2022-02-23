using CreateCase.Implementation;
using Entities.Manager;
using EntityFramework;
using MediatR;

namespace Commands.Implementation
{
    public class CreateUseCasesCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public string CaseName { get; set; }
        public List<CaseEvent> CaseEvent { get; set; }
        public List<CaseReaction> CaseReaction { get; set; }

        public class CreateUseCasesCommandHandler : IRequestHandler<CreateUseCasesCommand, int>
        {
            private readonly IDatabaseContext _context;
            public CreateUseCasesCommandHandler(IDatabaseContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(CreateUseCasesCommand command, CancellationToken cancellationToken)
            {
                using (var dbContextTransaction = await _context.Database.BeginTransactionAsync(cancellationToken))
                {
                    UseCases useCases = new UseCases();
                    useCases.UserId = command.UserId;
                    useCases.CaseName = command.CaseName;
                    await _context.UseCases.AddAsync(useCases);
                    await _context.SaveChangesAsync(cancellationToken);

                    foreach (var caseEvent in command.CaseEvent)
                    {
                        await CreateEvent.Create(_context, cancellationToken, caseEvent, useCases.Id);
                    }

                    foreach (var caseReaction in command.CaseReaction)
                    {
                        await CreateReaction.Create(_context, cancellationToken, caseReaction, useCases.Id);
                    }

                    await dbContextTransaction.CommitAsync(cancellationToken);
                    return useCases.Id;
                }
            }
        }
    }
}
