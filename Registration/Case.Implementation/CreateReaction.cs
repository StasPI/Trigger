using AutoMapper;
using Contracts.Manager;
using Entities.Manager;
using Entities.Reaction;
using EntityFramework.Abstraction;

namespace CreateCase.Implementation
{
    public class CreateReaction : CaseReactionDto
    {
        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;

        public CreateReaction(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task Create(CaseReactionDto caseReactionDto, int useCasesId, CancellationToken cancellationToken)
        {
            switch (caseReactionDto.Name)
            {
                case "Email":
                    caseReactionDto.DestinationId = (await _context.SaveAsyncJsonObject<EmailDestination>(caseReactionDto.Destination, cancellationToken)).Id;
                    break;
            }
            caseReactionDto.UseCasesID = useCasesId;
            CaseReaction caseReaction = _mapper.Map<CaseReaction>(caseReactionDto);

            await _context.CaseReaction.AddAsync(caseReaction);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
