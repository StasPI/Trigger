using AutoMapper;
using Contracts.Manager;
using Contracts.Reaction;
using Entities.Abstraction;
using Entities.Reaction;
using EntityFramework.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Case.Implementation
{
    public class GetReaction
    {
        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;

        public GetReaction(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Get(CaseReactionDto caseReactionDto, CancellationToken cancellationToken)
        {
            switch (caseReactionDto.Name)
            {
                case "Email":
                    caseReactionDto.Destination = await GetDestinationJsonObjectAsync<EmailDestination, EmailDestinationDto>(caseReactionDto.DestinationId, cancellationToken);
                    break;
            }
        }

        private async Task<JsonObject> GetDestinationJsonObjectAsync<Table, TableDto>(int DestinationId, CancellationToken cancellationToken) where Table : class, IEntity
        {
            Table source = await _context.Set<Table>().Where(x => x.Id == DestinationId).FirstAsync(cancellationToken);
            TableDto dto = _mapper.Map<TableDto>(source);
            string sSource = JsonSerializer.Serialize(dto);
            return JsonSerializer.Deserialize<JsonObject>(sSource);
        }
    }
}
