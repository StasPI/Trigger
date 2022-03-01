using AutoMapper;
using Contracts.Manager;
using CreateCase.Implementation;
using Entities.Manager;
using EntityFramework.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Commands.Implementation
{
    public class CreateUseCasesCommand : UseCasesDto, IRequest<int>
    {
        public class CreateUseCasesCommandHandler : IRequestHandler<CreateUseCasesCommand, int>
        {
            readonly IServiceScopeFactory _scopeFactory;
            private IDatabaseContext _context;
            private IMapper _mapper;
            private CreateEvent _createEvent;
            private CreateReaction _createReaction;
            private UseCasesDto _useCasesDto;

            public CreateUseCasesCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
            {
                _scopeFactory = scopeFactory;
                _mapper = mapper;
                _useCasesDto = new UseCasesDto();
            }

            public async Task<int> Handle(CreateUseCasesCommand command, CancellationToken cancellationToken)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
                    _createEvent = new CreateEvent(_context, _mapper);
                    _createReaction = new CreateReaction(_context, _mapper);
                    using IDbContextTransaction dbContextTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
                    _useCasesDto.UserId = command.UserId;
                    _useCasesDto.CaseName = command.CaseName;

                    UseCases useCases = _mapper.Map<UseCases>(_useCasesDto);

                    await _context.UseCases.AddAsync(useCases);
                    await _context.SaveChangesAsync(cancellationToken);

                    await EventAsync(command.CaseEvent, useCases.Id, cancellationToken);
                    await RuleAsync(command.CaseReaction, useCases.Id, cancellationToken);

                    await dbContextTransaction.CommitAsync(cancellationToken);
                    return useCases.Id;
                }
            }

            private async Task EventAsync(List<CaseEventDto> command,int useCasesId, CancellationToken cancellationToken)
            {
                foreach (var caseEvent in command)
                {
                    await _createEvent.Create(caseEvent, useCasesId, cancellationToken);
                }
            }

            private async Task RuleAsync(List<CaseReactionDto> command, int useCasesId, CancellationToken cancellationToken)
            {
                foreach (var caseReaction in command)
                {
                    await _createReaction.Create(caseReaction, useCasesId, cancellationToken);
                }
            }
        }
    }
}
