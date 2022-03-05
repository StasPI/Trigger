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
    public class Reactions
    {
        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper; 
        public Reactions(IDatabaseContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<CaseReactionDto> CreateCaseReactionAsync(CaseReactionDto caseReactionDto, CancellationToken cancellationToken)
        {
            string name = caseReactionDto.Name;
            JsonObject destination = caseReactionDto.Destination;
            switch (name)
            {
                case "Email":
                    caseReactionDto.DestinationId = (await _context.SaveAsyncJsonObject<EmailDestination>(destination, cancellationToken)).Id;
                    break;
            }
            return caseReactionDto;
        }
        public async Task FillaseReactionAsync(CaseReactionDto caseReactionDto, CancellationToken cancellationToken)
        {
            string name = caseReactionDto.Name;
            int destinationId = caseReactionDto.DestinationId;
            switch (name)
            {
                case "Email":
                    caseReactionDto.Destination = await GetDestinationJsonObjectAsync<EmailDestination, EmailDestinationDto>(destinationId, cancellationToken);
                    break;
            }
        }
        private async Task<JsonObject> GetDestinationJsonObjectAsync<Table, TableDto>(int DestinationId, CancellationToken cancellationToken) 
            where Table : class, IEntity
        {
            Table source = await _context.Set<Table>().Where(x => x.Id == DestinationId).FirstAsync(cancellationToken);
            TableDto dto = _mapper.Map<TableDto>(source);
            string sSource = JsonSerializer.Serialize(dto);
            return JsonSerializer.Deserialize<JsonObject>(sSource);
        }
    }
}
