using Contracts.Manager;
using Entities.Manager;
using EntityFramework.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Worker.Implementation
{
    public class NotSentMessages
    {
        public readonly IDatabaseContext _context;
        public NotSentMessages(IDatabaseContext context)
        {
            _context = context;
        }
        public async Task<List<UseCasesDto>> GetEventAsync(CancellationToken cancellationToken)
        {
            UseCases useCases = await _context.UseCases.Where(x => x.SendToEvent == false).FirstAsync(cancellationToken);
            return true;
        }
        public async Task<List<UseCasesDto>> GetReactionAsync(CancellationToken cancellationToken)
        {
            UseCases useCases = await _context.UseCases.Where(x => x.SendToReaction == false).FirstAsync(cancellationToken);
            return true;
        }
    }
}
