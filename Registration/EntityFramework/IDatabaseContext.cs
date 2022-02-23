using Entities.Event;
using Entities.Manager;
using Entities.Reaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFramework
{
    public interface IDatabaseContext
    {
        //manager
        DbSet<UseCases> UseCases { get; set; }
        DbSet<CaseEvent> CaseEvents { get; set; }
        DbSet<CaseReaction> CaseReaction { get; set; }

        //event
        DbSet<EmailRules> EmailRules { get; set; }
        DbSet<EmailSource> EmailSource { get; set; }
        DbSet<SiteRules> SiteRules { get; set; }
        DbSet<SiteSource> SiteSource { get; set; }

        //reaction
        public DbSet<EmailDestination> EmailDestination { get; set; }

        //repo
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}