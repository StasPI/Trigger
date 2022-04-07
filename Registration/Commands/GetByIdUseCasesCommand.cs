using AutoMapper;
using Dto.Registration;
using Entities.Registration;
using EntityFramework.Abstraction;
using Helps;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json.Nodes;

namespace Commands
{
    public class GetByIdUseCasesCommand : UseCasesGetDto, IRequest<UseCasesGetDto>
    {
        public class GetByIdUseCasesCommandHandler : IRequestHandler<GetByIdUseCasesCommand, UseCasesGetDto>
        {
            private readonly ILogger<GetByIdUseCasesCommandHandler> _logger;
            private readonly IMapper _mapper;
            private readonly IDatabaseContext _context;
            private JsonObject _caseEvent;
            private JsonObject _caseReaction;

            public GetByIdUseCasesCommandHandler(ILogger<GetByIdUseCasesCommandHandler> logger, IMapper mapper, IDatabaseContext context)
            {
                _logger = logger;
                _context = context;
                _mapper = mapper;
            }

            public async Task<UseCasesGetDto> Handle(GetByIdUseCasesCommand query, CancellationToken cancellationToken)
            {
                try
                {
                    _logger.LogInformation("GetByIdUseCasesCommandHandler Get UseCase: {id} | Time: {time}", query.Id, DateTimeOffset.Now);
                    UseCases useCases = await _context.UseCases.Where(x => (x.Id == query.Id) & (x.DateDeleted == null)).FirstAsync(cancellationToken);

                    UseCasesGetDto useCasesGetDto = _mapper.Map<UseCasesGetDto>(useCases);

                    List<Task> tasks = new()
                    {
                        Task.Run(async () => _caseEvent = await ConvertObject.StringToJsonObjectAsync(useCasesGetDto.CaseEventStr)),
                        Task.Run(async () => _caseReaction = await ConvertObject.StringToJsonObjectAsync(useCasesGetDto.CaseReactionStr))
                    };
                    Task.WhenAll(tasks).Wait(cancellationToken);

                    useCasesGetDto.CaseEvent = _caseEvent;
                    useCasesGetDto.CaseReaction = _caseReaction;

                    return useCasesGetDto;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("GetByIdUseCasesCommandHandler Exception UseCase: {id} | Time: {time} | Error: {ex} ", query.Id, DateTimeOffset.Now, ex);
                    return new UseCasesGetDto();
                }
            }
        }
    }
}
