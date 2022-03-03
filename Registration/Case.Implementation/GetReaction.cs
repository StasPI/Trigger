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
    public static class GetReaction
    {
        public static async Task Get(IDatabaseContext context, IMapper mapper, CaseReactionDto caseReactionDto, CancellationToken cancellationToken)
        {
            switch (caseReactionDto.Name)
            {
                case "Email":
                    caseReactionDto.Destination = await GetDestinationJsonObjectAsync<EmailDestination, EmailDestinationDto>(context, mapper, caseReactionDto.DestinationId, cancellationToken);
                    break;
            }
        }
        private static async Task<JsonObject> GetDestinationJsonObjectAsync<Table, TableDto>(IDatabaseContext context, IMapper mapper, int DestinationId, CancellationToken cancellationToken) where Table : class, IEntity
        {
            Table source = await context.Set<Table>().Where(x => x.Id == DestinationId).FirstAsync(cancellationToken);
            TableDto dto = mapper.Map<TableDto>(source);
            string sSource = JsonSerializer.Serialize(dto);
            return JsonSerializer.Deserialize<JsonObject>(sSource);
        }
    }
}
