using AutoMapper;
using Dto.Registration;
using Entities.Registration;
using EntityFramework.Abstraction;
using Helps;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Commands.Implementation
{
    public class PostUseCasesCommand : UseCasesPostDto, IRequest<int>
    {
        public class PostUseCasesCommandHandler : IRequestHandler<PostUseCasesCommand, int>
        {
            private readonly ILogger<PostUseCasesCommandHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IDatabaseContext _context;
            private List<string> _caseEvent;
            private List<string> _caseReaction;
            private readonly UseCasesPostDto _useCasesPostDto;

            public PostUseCasesCommandHandler(ILogger<PostUseCasesCommandHandler> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
            {
                _logger = logger;
                IServiceScope scope = scopeFactory.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
                _mapper = mapper;
                _caseEvent = new();
                _caseReaction = new();
                _useCasesPostDto = new();
            }

            public async Task<int> Handle(PostUseCasesCommand command, CancellationToken cancellationToken)
            {
                using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                try
                {
                    List<Task> tasks = new()
                    {
                        Task.Run(async () => _caseEvent = await ConvertObject.ListJsonObjectToListString(command.CaseEvent)),
                        Task.Run(async () => _caseReaction = await ConvertObject.ListJsonObjectToListString(command.CaseReaction))
                    };
                    Task.WhenAll(tasks).Wait(cancellationToken);

                    _useCasesPostDto.UserId = command.UserId;
                    _useCasesPostDto.CaseName = command.CaseName;
                    _useCasesPostDto.CaseEventStr = _caseEvent;
                    _useCasesPostDto.CaseReactionStr = _caseReaction;
                    _useCasesPostDto.Active = command.Active;

                    UseCases useCases = _mapper.Map<UseCases>(_useCasesPostDto);

                    await _context.UseCases.AddAsync(useCases, cancellationToken);

                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    _logger.LogInformation("PostUseCasesCommandHandler registration UseCase {id} : {time}", useCases.Id, DateTimeOffset.Now);
                    return useCases.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    _logger.LogInformation("PostUseCasesCommandHandler Error: {ex} : {time}", ex,  DateTimeOffset.Now);
                    return -1;
                }
            }
        }
    }
}
