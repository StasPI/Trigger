using AutoMapper;
using Case.Implementation;
using Contracts.Manager;
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
            private readonly IMapper _mapper;
            private readonly UseCasesDto _useCasesDto;
            private readonly IDatabaseContext _context;
            public CreateUseCasesCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
            {
                IServiceScope scope = scopeFactory.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
                _mapper = mapper;
                _useCasesDto = new UseCasesDto();
            }
            public async Task<int> Handle(CreateUseCasesCommand command, CancellationToken cancellationToken)
            {
                using IDbContextTransaction dbContextTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                _useCasesDto.UserId = command.UserId;
                _useCasesDto.CaseName = command.CaseName;

                UseCases useCases = _mapper.Map<UseCases>(_useCasesDto);

                await _context.UseCases.AddAsync(useCases, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await EventAsync(command.CaseEvent, useCases.Id, cancellationToken);
                await ReactionAsync(command.CaseReaction, useCases.Id, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
                await dbContextTransaction.CommitAsync(cancellationToken);

                return useCases.Id;
            }
            private async Task EventAsync(List<CaseEventDto> commandCasesEvent, int useCasesId, CancellationToken cancellationToken)
            {
                List<CaseEventDto> casesEventDto = new();
                Events events = new(_context, _mapper);
                foreach (CaseEventDto commandCaseEvent in commandCasesEvent)
                {
                    commandCaseEvent.UseCasesID = useCasesId;
                    casesEventDto.Add(await events.CreateCaseEventAsync(commandCaseEvent, cancellationToken));
                }
                List<CaseEvent> caseEvent = _mapper.Map<List<CaseEvent>>(casesEventDto);
                await _context.CaseEvents.AddRangeAsync(caseEvent, cancellationToken);
            }
            private async Task ReactionAsync(List<CaseReactionDto> commandCasesReaction, int useCasesId, CancellationToken cancellationToken)
            {
                List<CaseReactionDto> caseReactionDto = new();
                Reactions reactions = new(_context, _mapper);
                foreach (CaseReactionDto commandCaseReaction in commandCasesReaction)
                {
                    commandCaseReaction.UseCasesID = useCasesId;
                    caseReactionDto.Add(await reactions.CreateCaseReactionAsync(commandCaseReaction, cancellationToken));
                }
                List<CaseReaction> caseReaction = _mapper.Map<List<CaseReaction>>(caseReactionDto);
                await _context.CaseReaction.AddRangeAsync(caseReaction, cancellationToken);
            }
        }
    }
}
