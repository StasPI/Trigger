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
            private readonly IMapper _mapper;
            private readonly UseCasesDto _useCasesDto;
            public CreateUseCasesCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
            {
                _scopeFactory = scopeFactory;
                _mapper = mapper;
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
                    casesEventDto.Add(await CreateEvent.Create(context, commandCaseEvent, useCasesId, cancellationToken));
                }
                List<CaseEvent> caseEvent = _mapper.Map<List<CaseEvent>>(casesEventDto);
                await context.CaseEvents.AddRangeAsync(caseEvent, cancellationToken);
            }
            private async Task ReactionAsync(List<CaseReactionDto> commandCasesReaction, IDatabaseContext context, int useCasesId, CancellationToken cancellationToken)
            {
                List<CaseReactionDto> caseReactionDto = new();
                foreach (CaseReactionDto commandCaseReaction in commandCasesReaction)
                {
                    caseReactionDto.Add(await CreateReaction.Create(context, commandCaseReaction, useCasesId, cancellationToken));
                }
                List<CaseReaction> caseReaction = _mapper.Map<List<CaseReaction>>(caseReactionDto);
                await context.CaseReaction.AddRangeAsync(caseReaction, cancellationToken);
            }
        }
    }
}
