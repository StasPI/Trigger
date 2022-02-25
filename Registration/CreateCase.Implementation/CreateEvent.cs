using Entities.Event;
using Entities.Manager;
using EntityFramework.Abstraction;

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
                    _sourceId = (await _context.SaveAsyncJsonObject<EmailSource>(cancellationToken, _caseEvent.Source)).Id;
                    _ruleId = (await _context.SaveAsyncJsonObject<EmailRules>(cancellationToken, _caseEvent.Rule)).Id;
                    break;
                case "Site":
                    _sourceId = (await _context.SaveAsyncJsonObject<SiteSource>(cancellationToken, _caseEvent.Source)).Id;
                    _ruleId = (await _context.SaveAsyncJsonObject<SiteRules>(cancellationToken, _caseEvent.Rule)).Id;
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