using Entities.Event;
using Entities.Manager;
using EntityFramework.Abstraction;
using System.Text.Json;

namespace CreateCase.Implementation
{
    public static class CreateEvent
    {
        private static int _sourceId;
        private static int _ruleId;
        public static async Task Create(IDatabaseContext _context, CancellationToken cancellationToken, CaseEvent _caseEvent, int _useCasesId)
        {
            switch (_caseEvent.Name)
            {
                case "Email":
                    EmailSource emailSource = JsonSerializer.Deserialize<EmailSource>(_caseEvent.Source);
                    EmailRules emailRules = JsonSerializer.Deserialize<EmailRules>(_caseEvent.Rule);
                    await _context.EmailSource.AddAsync(emailSource);
                    await _context.EmailRules.AddAsync(emailRules);
                    await _context.SaveChangesAsync(cancellationToken);
                    _sourceId = emailSource.Id;
                    _ruleId = emailRules.Id;
                    break;
                case "Site":
                    SiteSource siteSource = JsonSerializer.Deserialize<SiteSource>(_caseEvent.Source);
                    SiteRules siteRules = JsonSerializer.Deserialize<SiteRules>(_caseEvent.Rule);
                    await _context.SiteSource.AddAsync(siteSource);
                    await _context.SiteRules.AddAsync(siteRules);
                    await _context.SaveChangesAsync(cancellationToken);
                    _sourceId = siteSource.Id;
                    _ruleId = siteRules.Id;
                    break;
            }
            
            CaseEvent ce = new CaseEvent();
            ce.Name = _caseEvent.Name;
            ce.UseCasesID = _useCasesId;
            ce.SourceId = _sourceId;
            ce.RuleId = _ruleId;
            await _context.CaseEvents.AddAsync(ce);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}