using Entities.Manager;
using Entity.Event;
using Entity.Reaction;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        //manager
        public DbSet<UseCases> UseCases { get; set; }
        public DbSet<CaseEvent> CaseEvents { get; set; }
        public DbSet<CaseReaction> CaseReaction { get; set; }
        //event
        public DbSet<EmailRules> EmailRules { get; set; }
        public DbSet<EmailSource> EmailSource { get; set; }
        public DbSet<SiteRules> SiteRules { get; set; }
        public DbSet<SiteSource> SiteSource { get; set; }
        //reaction
        public DbSet<Email> Email { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
    }
}