using AutoMapper;
using Dto.Registration;
using Entities.Registration;
using EntityFramework.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Commands.Implementation
{
    public class CreateUseCasesCommand : UseCasesPostDto, IRequest<int>
    {
        public class CreateUseCasesCommandHandler : IRequestHandler<CreateUseCasesCommand, int>
        {
            private readonly ILogger<CreateUseCasesCommandHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IDatabaseContext _context;
            private readonly List<string> _caseEvent;
            private readonly List<string> _caseReaction;
            private readonly UseCasesPostDto _useCasesPostDto;

            public CreateUseCasesCommandHandler(ILogger<CreateUseCasesCommandHandler> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
            {
                _logger = logger;
                IServiceScope scope = scopeFactory.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
                _mapper = mapper;
                _caseEvent = new();
                _caseReaction = new();
                _useCasesPostDto = new();
            }

            public async Task<int> Handle(CreateUseCasesCommand command, CancellationToken cancellationToken)
            {
                using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                try
                {
                    command.CaseEvent.ForEach(x => _caseEvent.Add(x.ToJsonString()));
                    command.CaseReaction.ForEach(x => _caseReaction.Add(x.ToJsonString()));

                    _useCasesPostDto.UserId = command.UserId;
                    _useCasesPostDto.CaseName = command.CaseName;
                    _useCasesPostDto.CaseEventStr = _caseEvent;
                    _useCasesPostDto.CaseReactionStr = _caseReaction;

                    UseCases useCases = _mapper.Map<UseCases>(_useCasesPostDto);

                    await _context.UseCases.AddAsync(useCases, cancellationToken);

                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    _logger.LogInformation("CreateUseCasesCommandHandler registration UseCase {id} : {time}", useCases.Id, DateTimeOffset.Now);
                    return useCases.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    _logger.LogInformation("CreateUseCasesCommandHandler Error: {ex} : {time}", ex,  DateTimeOffset.Now);
                    return -1;
                }
            }
        }
    }
}
