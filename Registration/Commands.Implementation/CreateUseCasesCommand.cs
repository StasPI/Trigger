using AutoMapper;
using Contracts.Implementation;
using CreateCase.Implementation;
using Entities.Manager;
using EntityFramework.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;

namespace Commands.Implementation
{
    public class CreateUseCasesCommand : UseCasesDto, IRequest<int>
    {
        public class CreateUseCasesCommandHandler : IRequestHandler<CreateUseCasesCommand, int>
        {
            private readonly IDatabaseContext _context;
            private readonly IMapper _mapper;
            public CreateUseCasesCommandHandler(IDatabaseContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<int> Handle(CreateUseCasesCommand command, CancellationToken cancellationToken)
            {
                using (IDbContextTransaction dbContextTransaction = await _context.Database.BeginTransactionAsync(cancellationToken))
                {
                    UseCasesDto useCasesDto = new UseCasesDto();
                    useCasesDto.UserId = command.UserId;
                    useCasesDto.CaseName = command.CaseName;

                    UseCases useCases = _mapper.Map<UseCases>(useCasesDto);

                    await _context.UseCases.AddAsync(useCases);
                    await _context.SaveChangesAsync(cancellationToken);

                    //await Task.WhenAll(
                    //    EventAsync(command.CaseEvent, cancellationToken, useCases.Id),
                    //    RuleAsync(command.CaseReaction, cancellationToken, useCases.Id));
                    await EventAsync(command.CaseEvent, cancellationToken, useCases.Id);
                    await RuleAsync(command.CaseReaction, cancellationToken, useCases.Id);

                    await dbContextTransaction.CommitAsync(cancellationToken);
                    return useCases.Id;
                }
            }

            private async Task EventAsync(List<CaseEvent> command, CancellationToken cancellationToken,int useCasesId)
            {
                foreach (var caseEvent in command)
                {
                    await CreateEvent.Create(_context, cancellationToken, caseEvent, useCasesId);
                }
            }

            private async Task RuleAsync(List<CaseReaction> command, CancellationToken cancellationToken, int useCasesId)
            {
                foreach (var caseReaction in command)
                {
                    await CreateReaction.Create(_context, cancellationToken, caseReaction, useCasesId);
                }
            }
        }
    }
}
