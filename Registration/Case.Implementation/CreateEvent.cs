using AutoMapper;
using Case.Abstraction;
using Contracts.Manager;
using Entities.Event;
using Entities.Manager;
using EntityFramework.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace CreateCase.Implementation
{
    public class CreateEvent : CaseEventDto, ICreateEvent
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        public CreateEvent(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public async Task<CaseEventDto> Create(CaseEventDto caseEventDto, int useCasesId, CancellationToken cancellationToken)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            IDatabaseContext context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
            caseEventDto.UseCasesID = useCasesId;
            switch (caseEventDto.Name)
            {
                case "Email":
                    caseEventDto.SourceId = (await context.SaveAsyncJsonObject<EmailSource>(caseEventDto.Source, cancellationToken)).Id;
                    caseEventDto.RuleId = (await context.SaveAsyncJsonObject<EmailRule>(caseEventDto.Rule, cancellationToken)).Id;
                    break;
                case "Site":
                    caseEventDto.SourceId = (await context.SaveAsyncJsonObject<SiteSource>(caseEventDto.Source, cancellationToken)).Id;
                    caseEventDto.RuleId = (await context.SaveAsyncJsonObject<SiteRule>(caseEventDto.Rule, cancellationToken)).Id;
                    break;
            }
            return caseEventDto;
            //CaseEvent caseEvent = _mapper.Map<CaseEvent>(caseEventDto);
            //await context.CaseEvents.AddAsync(caseEvent, cancellationToken);
            //await context.SaveChangesAsync(cancellationToken);
        }
    }
}