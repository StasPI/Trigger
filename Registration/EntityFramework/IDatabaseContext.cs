using Entities.Manager;
using Entity.Event;
using Entity.Reaction;
using Microsoft.EntityFrameworkCore;

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
        DbSet<Email> Email { get; set; }
        //??
        Task<int> SaveChanges();
    }
}