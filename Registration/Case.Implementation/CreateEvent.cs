using AutoMapper;
using Contracts.Manager;
using Entities.Event;
using Entities.Manager;
using EntityFramework.Abstraction;

namespace CreateCase.Implementation
{
    public class CreateEvent : CaseEventDto
    {
        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;
        public CreateEvent(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task Create(CaseEventDto caseEventDto, int useCasesId, CancellationToken cancellationToken)
        {
            caseEventDto.UseCasesID = useCasesId;
            switch (caseEventDto.Name)
            {
                case "Email":
                    caseEventDto.SourceId = (await _context.SaveAsyncJsonObject<EmailSource>(caseEventDto.Source, cancellationToken)).Id;
                    caseEventDto.RuleId = (await _context.SaveAsyncJsonObject<EmailRule>(caseEventDto.Rule, cancellationToken)).Id;
                    break;
                case "Site":
                    caseEventDto.SourceId = (await _context.SaveAsyncJsonObject<SiteSource>(caseEventDto.Source, cancellationToken)).Id;
                    caseEventDto.RuleId = (await _context.SaveAsyncJsonObject<SiteRule>(caseEventDto.Rule, cancellationToken)).Id;
                    break;
            }

            CaseEvent caseEvent = _mapper.Map<CaseEvent>(caseEventDto);
            await _context.CaseEvents.AddAsync(caseEvent, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}