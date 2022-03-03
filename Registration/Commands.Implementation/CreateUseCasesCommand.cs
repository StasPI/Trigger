using AutoMapper;
using Case.Abstraction;
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
            private readonly IMapper _mapper;
            private readonly ICreateEvent _createEvent;
            private readonly UseCasesDto _useCasesDto;
            public CreateUseCasesCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper, ICreateEvent createEvent)
            {
                _scopeFactory = scopeFactory;
                _mapper = mapper;
                _createEvent = createEvent;
                _useCasesDto = new UseCasesDto();
            }
            public async Task<int> Handle(CreateUseCasesCommand command, CancellationToken cancellationToken)
            {
                using IServiceScope scope = _scopeFactory.CreateScope();
                IDatabaseContext context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
                using IDbContextTransaction dbContextTransaction = await context.Database.BeginTransactionAsync(cancellationToken);

                _useCasesDto.UserId = command.UserId;
                _useCasesDto.CaseName = command.CaseName;

                UseCases useCases = _mapper.Map<UseCases>(_useCasesDto);

                await context.UseCases.AddAsync(useCases, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                await EventAsync(command.CaseEvent, context, useCases.Id, cancellationToken);
                await ReactionAsync(command.CaseReaction, context, useCases.Id, cancellationToken);

                await context.SaveChangesAsync(cancellationToken);
                await dbContextTransaction.CommitAsync(cancellationToken);
                return useCases.Id;
            }
            private async Task EventAsync(List<CaseEventDto> commandCasesEvent, IDatabaseContext context, int useCasesId, CancellationToken cancellationToken)
            {
                List<CaseEventDto> casesEventDto = new();
                foreach (CaseEventDto commandCaseEvent in commandCasesEvent)
                {
                    casesEventDto.Add(await _createEvent.Create(commandCaseEvent, useCasesId, cancellationToken));
                }
                List<CaseEvent> caseEvent = _mapper.Map<List<CaseEvent>>(casesEventDto);
                await context.CaseEvents.AddRangeAsync(caseEvent, cancellationToken);
            }
            private async Task ReactionAsync(List<CaseReactionDto> command, IDatabaseContext context, int useCasesId, CancellationToken cancellationToken)
            {
                CreateReaction createReaction = new(context, _mapper);
                foreach (CaseReactionDto caseReaction in command)
                {
                    await createReaction.Create(caseReaction, useCasesId, cancellationToken);
                }
            }
        }
    }
}
