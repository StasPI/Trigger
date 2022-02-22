using Entities.Manager;
using EntityFramework;
using MediatR;

namespace Implementation
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
                    useCases.CaseEvent = command.CaseEvent;

                    foreach (var caseEvent in useCases.CaseEvent)
                    {
                        if (caseEvent.EventTypeName == "Email")
                        {

                        }
                    }
                    useCases.CaseReaction = command.CaseReaction;
                    _context.UseCases.Add(useCases);
                    await _context.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync(cancellationToken);
                    return useCases.Id;
                }
            }
        }
    }
}
