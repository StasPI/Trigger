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
    public class Events
    {
        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;
        public Events(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CaseEventDto> CreateCaseEventAsync(CaseEventDto caseEventDto, CancellationToken cancellationToken)
        {
            string name = caseEventDto.Name;
            JsonObject source = caseEventDto.Source;
            JsonObject rule = caseEventDto.Rule;
            switch (name)
            {
                case "Email":
                    caseEventDto.SourceId = (await _context.SaveAsyncJsonObject<EmailSource>(source, cancellationToken)).Id;
                    caseEventDto.RuleId = (await _context.SaveAsyncJsonObject<EmailRule>(rule, cancellationToken)).Id;
                    break;
                case "Site":
                    caseEventDto.SourceId = (await _context.SaveAsyncJsonObject<SiteSource>(source, cancellationToken)).Id;
                    caseEventDto.RuleId = (await _context.SaveAsyncJsonObject<SiteRule>(rule, cancellationToken)).Id;
                    break;
            }
            return caseEventDto;
        }
        public async Task FillCaseEventAsync(CaseEventDto caseEventDto, CancellationToken cancellationToken)
        {
            string name = caseEventDto.Name;
            int sourceId = caseEventDto.SourceId;
            int ruleId = caseEventDto.RuleId;
            switch (name)
            {
                case "Email":
                    caseEventDto.Source = await GetSourceJsonObjectAsync<EmailSource, EmailSourceDto>(sourceId, cancellationToken);
                    caseEventDto.Rule = await GetRuleJsonObjectAsync<EmailRule, EmailRuleDto>(ruleId, cancellationToken);
                    break;
                case "Site":
                    caseEventDto.Source = await GetSourceJsonObjectAsync<SiteSource, SiteSourceDto>(sourceId, cancellationToken);
                    caseEventDto.Rule = await GetRuleJsonObjectAsync<SiteRule, SiteRuleDto>(ruleId, cancellationToken);
                    break;
            }
        }
        private async Task<JsonObject> GetSourceJsonObjectAsync<Table, TableDto>(int SourceId, CancellationToken cancellationToken) 
            where Table : class, IEntity
        {
            Table source = await _context.Set<Table>().Where(x => x.Id == SourceId).FirstAsync(cancellationToken);
            TableDto dto = _mapper.Map<TableDto>(source);
            string sSource = JsonSerializer.Serialize(dto);
            return JsonSerializer.Deserialize<JsonObject>(sSource);
        }
        private async Task<JsonObject> GetRuleJsonObjectAsync<Table, TableDto>(int RuleId, CancellationToken cancellationToken) 
            where Table : class, IEntity
        {
            Table rule = await _context.Set<Table>().Where(x => x.Id == RuleId).FirstAsync(cancellationToken);
            TableDto ruleDto = _mapper.Map<TableDto>(rule);
            string sRule = JsonSerializer.Serialize(ruleDto);
            return JsonSerializer.Deserialize<JsonObject>(sRule);
        }
    }
}
