﻿using AutoMapper;
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
    public class GetEvent
    {
        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;

        public GetEvent(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task Get(CaseEventDto caseEventDto, CancellationToken cancellationToken)
        {
            
            switch (caseEventDto.Name)
            {
                case "Email":
                    caseEventDto.Source = await GetSourceJsonObjectAsync<EmailSource, EmailSourceDto>(caseEventDto, cancellationToken);
                    caseEventDto.Rule = await GetRuleJsonObjectAsync<EmailRule, EmailRuleDto>(caseEventDto, cancellationToken);
                    break;
                case "Site":
                    caseEventDto.Source = await GetSourceJsonObjectAsync<SiteSource, SiteSourceDto>(caseEventDto, cancellationToken);
                    caseEventDto.Rule = await GetRuleJsonObjectAsync<SiteRule, SiteRuleDto>(caseEventDto, cancellationToken);
                    break;
            }
        }

        private async Task<JsonObject> GetSourceJsonObjectAsync<Table, TableDto>(CaseEventDto caseEventDto, CancellationToken cancellationToken) where Table : class, IEntity
        {
            Table source = await _context.Set<Table>().Where(x => x.Id == caseEventDto.SourceId).FirstAsync(cancellationToken);
            TableDto dto = _mapper.Map<TableDto>(source);
            string sSource = JsonSerializer.Serialize(dto);
            return JsonSerializer.Deserialize<JsonObject>(sSource);
        }

        private async Task<JsonObject> GetRuleJsonObjectAsync<Table, TableDto>(CaseEventDto caseEventDto, CancellationToken cancellationToken) where Table : class, IEntity
        {
            Table rule = await _context.Set<Table>().Where(x => x.Id == caseEventDto.RuleId).FirstAsync(cancellationToken);
            TableDto ruleDto = _mapper.Map<TableDto>(rule);
            string sRule = JsonSerializer.Serialize(ruleDto);
            return JsonSerializer.Deserialize<JsonObject>(sRule);
        }
    }
}