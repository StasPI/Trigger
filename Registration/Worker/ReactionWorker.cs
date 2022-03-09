using AutoMapper;
using Dto.Registration;
using Entities.Registration;
using EntityFramework.Abstraction;
using Helps;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Worker.Abstraction;

namespace Worker
{
    public class ReactionWorker : UseCasesSendReactionDto, IReactionWorker
    {
        private readonly ILogger<ReactionWorker> _logger;
        private readonly IMapper _mapper;
        private readonly IDatabaseContext _context;
        private readonly ReactionWorkerOptions _workerOptions;

        public ReactionWorker(ILogger<ReactionWorker> logger, IOptions<ReactionWorkerOptions> workerOptions, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            IServiceScope scope = scopeFactory.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
            _workerOptions = workerOptions.Value;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ReactionWorker run at: {time}", DateTimeOffset.Now);

            List<UseCases> useCases = await _context.UseCases
                .Where(x => x.SendReaction == false)
                .Take(_workerOptions.MaxNumberOfMessages)
                .ToListAsync(cancellationToken);

            List<UseCasesSendReactionDto> useCasesSendReactionDto = _mapper.Map<List<UseCasesSendReactionDto>>(useCases);

            Parallel.ForEach(useCasesSendReactionDto, async x => x.CaseReaction = await ConvertObject.ListStringToJsonObject(x.CaseReactionStr));

            Console.WriteLine("send");
        }
    }
}