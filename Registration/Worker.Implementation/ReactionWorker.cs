using AutoMapper;
using EntityFramework.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Worker.Abstraction;

namespace Worker.Implementation
{
    public class ReactionWorker : IReactionWorker
    {
        private readonly ILogger<ReactionWorker> _logger;
        private readonly IMapper _mapper;
        private readonly IDatabaseContext _databaseContext;

        public ReactionWorker(ILogger<ReactionWorker> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
        {

        }

        public Task Run(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
