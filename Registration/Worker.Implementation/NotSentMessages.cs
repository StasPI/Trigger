using AutoMapper;
using Contracts.Manager;
using Entities.Manager;
using EntityFramework.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Worker.Implementation
{
    public class NotSentMessages
    {
        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;
        public NotSentMessages(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<UseCasesDto>> GetEventAsync(CancellationToken cancellationToken)
        {
            List<UseCases> useCases = await _context.UseCases.Where(x => x.SendToEvent == false).ToListAsync(cancellationToken);
            foreach(UseCases useCase in useCases)
            {
                useCase.CaseEvent = await _context.CaseEvents.Where(x => x.UseCasesID == useCase.Id).ToListAsync(cancellationToken);
            }
            List<UseCasesDto> useCasesDto = _mapper.Map<List<UseCasesDto>>(useCases);
            return useCasesDto;
        }
        public async Task<List<UseCasesDto>> GetReactionAsync(CancellationToken cancellationToken)
        {
            List<UseCases> useCases = await _context.UseCases.Where(x => x.SendToReaction == false).ToListAsync(cancellationToken);
            foreach (UseCases useCase in useCases)
            {
                useCase.CaseReaction = await _context.CaseReaction.Where(x => x.UseCasesID == useCase.Id).ToListAsync(cancellationToken);
            }
            List<UseCasesDto> useCasesDto = _mapper.Map<List<UseCasesDto>>(useCases);
            return useCasesDto;
        }
    }
}
