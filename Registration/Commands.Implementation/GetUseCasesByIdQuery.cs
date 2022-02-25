using AutoMapper;
using Contracts.Implementation;
using Entities.Manager;
using EntityFramework.Abstraction;
using MediatR;

namespace Commands.Implementation
{
    public class GetUseCasesByIdQuery : UseCasesDto, IRequest<UseCasesDto>
    {
        public int Id { get; set; }
        public class GetUseCasesByIdQueryHandler : IRequestHandler<GetUseCasesByIdQuery, UseCasesDto>
        {
            private readonly IDatabaseContext _context;
            private readonly IMapper _mapper;
            public GetUseCasesByIdQueryHandler(IDatabaseContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<UseCasesDto> Handle(GetUseCasesByIdQuery query, CancellationToken cancellationToken)
            {
                UseCases useCases = _context.UseCases.Where(a => a.Id == query.Id).FirstOrDefault();
                if (useCases == null) return null;

                UseCasesDto useCasesDto = _mapper.Map<UseCasesDto>(useCases);

                return useCasesDto;
            }
        }
    }
}
