using AutoMapper;
using Contracts.Event;
using Contracts.Manager;
using Entities.Abstraction;
using Entities.Event;
using EntityFramework.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Case.Implementation
{
    public static class GetEvent
    {
        public static async Task Get(IDatabaseContext context, IMapper mapper, CaseEventDto caseEventDto, CancellationToken cancellationToken)
        {
            switch (caseEventDto.Name)
            {
                case "Email":
                    caseEventDto.Source = await GetSourceJsonObjectAsync<EmailSource, EmailSourceDto>(context, mapper, caseEventDto.SourceId, cancellationToken);
                    caseEventDto.Rule = await GetRuleJsonObjectAsync<EmailRule, EmailRuleDto>(context, mapper, caseEventDto.RuleId, cancellationToken);
                    break;
                case "Site":
                    caseEventDto.Source = await GetSourceJsonObjectAsync<SiteSource, SiteSourceDto>(context, mapper, caseEventDto.SourceId, cancellationToken);
                    caseEventDto.Rule = await GetRuleJsonObjectAsync<SiteRule, SiteRuleDto>(context, mapper, caseEventDto.RuleId, cancellationToken);
                    break;
            }
        }
        private static async Task<JsonObject> GetSourceJsonObjectAsync<Table, TableDto>(IDatabaseContext context, IMapper mapper, int SourceId, CancellationToken cancellationToken) where Table : class, IEntity
        {
            Table source = await context.Set<Table>().Where(x => x.Id == SourceId).FirstAsync(cancellationToken);
            TableDto dto = mapper.Map<TableDto>(source);
            string sSource = JsonSerializer.Serialize(dto);
            return JsonSerializer.Deserialize<JsonObject>(sSource);
        }
        private static async Task<JsonObject> GetRuleJsonObjectAsync<Table, TableDto>(IDatabaseContext context, IMapper mapper, int RuleId, CancellationToken cancellationToken) where Table : class, IEntity
        {
            Table rule = await context.Set<Table>().Where(x => x.Id == RuleId).FirstAsync(cancellationToken);
            TableDto ruleDto = mapper.Map<TableDto>(rule);
            string sRule = JsonSerializer.Serialize(ruleDto);
            return JsonSerializer.Deserialize<JsonObject>(sRule);
        }
    }
}
