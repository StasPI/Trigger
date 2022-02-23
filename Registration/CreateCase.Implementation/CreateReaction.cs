using Entities.Manager;
using Entities.Reaction;
using EntityFramework;
using System.Text.Json;

namespace CreateCase.Implementation
{
    public static class CreateReaction
    {
        private static int _destinationId;
        public static async Task Create(IDatabaseContext _context, CancellationToken cancellationToken, CaseReaction _caseReaction, int _useCasesId)
        {
            switch (_caseReaction.Name)
            {
                case "Email":
                    EmailDestination emailDestination = JsonSerializer.Deserialize<EmailDestination>(_caseReaction.Destination);
                    await _context.EmailDestination.AddAsync(emailDestination);
                    await _context.SaveChangesAsync(cancellationToken);
                    _destinationId = emailDestination.Id;
                    break;
            }

            CaseReaction cr = new CaseReaction();
            cr.Name = _caseReaction.Name;
            cr.UseCasesID = _useCasesId;
            cr.DestinationId = _destinationId;
            await _context.CaseReaction.AddAsync(cr);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
