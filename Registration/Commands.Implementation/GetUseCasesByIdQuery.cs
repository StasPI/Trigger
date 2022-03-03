using AutoMapper;
using Case.Implementation;
using Contracts.Manager;
using Entities.Manager;
using EntityFramework.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Commands.Implementation
{
    public class GetUseCasesByIdQuery : UseCasesDto, IRequest<UseCasesDto>
    {
        public class GetUseCasesByIdQueryHandler : IRequestHandler<GetUseCasesByIdQuery, UseCasesDto>
        {
            readonly IServiceScopeFactory _scopeFactory;
            private readonly IMapper _mapper;
            public GetUseCasesByIdQueryHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
            {
                _scopeFactory = scopeFactory;
                _mapper = mapper;
            }
            public async Task<UseCasesDto> Handle(GetUseCasesByIdQuery query, CancellationToken cancellationToken)
            {
                using IServiceScope scope = _scopeFactory.CreateScope();
                IDatabaseContext context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();

                UseCases useCases = await context.UseCases.Where(x => x.Id == query.Id).FirstAsync(cancellationToken);
                useCases.CaseEvent = await context.CaseEvents.Where(x => x.UseCasesID == useCases.Id).ToListAsync(cancellationToken);
                useCases.CaseReaction = await context.CaseReaction.Where(x => x.UseCasesID == useCases.Id).ToListAsync(cancellationToken);

                UseCasesDto useCasesDto = _mapper.Map<UseCasesDto>(useCases);

                await EventAsync(useCasesDto.CaseEvent, context, cancellationToken);
                await ReactionAsync(useCasesDto.CaseReaction, context, cancellationToken);

                return useCasesDto;
            }
            private async Task EventAsync(List<CaseEventDto> caseEvents, IDatabaseContext context, CancellationToken cancellationToken)
            {
                foreach (CaseEventDto caseEvent in caseEvents)
                {
                    await GetEvent.Get(context, _mapper, caseEvent, cancellationToken);
                }
            }
            private async Task ReactionAsync(List<CaseReactionDto> caseReactions, IDatabaseContext context, CancellationToken cancellationToken)
            {
                foreach (CaseReactionDto caseReaction in caseReactions)
                {
                    await GetReaction.Get(context, _mapper, caseReaction, cancellationToken);
                }
            }
        }
    }
}
