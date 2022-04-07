using AutoMapper;
using Dto.Registration;
using Entities.Registration;
using EntityFramework.Abstraction;
using Helps;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Commands
{
    public class PostUseCasesCommand : UseCasesPostDto, IRequest<int>
    {
        public class PostUseCasesCommandHandler : IRequestHandler<PostUseCasesCommand, int>
        {
            private readonly ILogger<PostUseCasesCommandHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IDatabaseContext _context;
            private string _caseEvent;
            private string _caseReaction;

            public PostUseCasesCommandHandler(ILogger<PostUseCasesCommandHandler> logger, IMapper mapper, IDatabaseContext context)
            {
                _logger = logger;
                _mapper = mapper;
                _context = context;
            }

            public async Task<int> Handle(PostUseCasesCommand command, CancellationToken cancellationToken)
            {
                using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                try
                {
                    _logger.LogInformation("PostUseCasesCommandHandler Post Time: {time}", DateTimeOffset.Now);
                    List<Task> tasks = new()
                    {
                        Task.Run(async () => _caseEvent = await ConvertObject.JsonObjectToStringAsync(command.CaseEvent)),
                        Task.Run(async () => _caseReaction = await ConvertObject.JsonObjectToStringAsync(command.CaseReaction))
                    };
                    Task.WhenAll(tasks).Wait(cancellationToken);

                    UseCasesPostDto useCasesPostDto = new()
                    {
                        UserId = command.UserId,
                        CaseName = command.CaseName,
                        CaseEventStr = _caseEvent,
                        CaseReactionStr = _caseReaction,
                        Active = command.Active
                    };

                    UseCases useCases = _mapper.Map<UseCases>(useCasesPostDto);

                    await _context.UseCases.AddAsync(useCases, cancellationToken);

                    await _context.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation("PostUseCasesCommandHandler Post UseCase: {id} | Time: {time}", useCases.Id, DateTimeOffset.Now);

                    await transaction.CommitAsync(cancellationToken);

                    return useCases.Id;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("PostUseCasesCommandHandler Exception Time: {time} | Error: {ex}", DateTimeOffset.Now, ex);
                    await transaction.RollbackAsync(cancellationToken);
                    return -1;
                }
            }
        }
    }
}
