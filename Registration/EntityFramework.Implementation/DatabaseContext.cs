using Abstraction;
using Entities.Event;
using Entities.Manager;
using Entities.Reaction;
using EntityFramework.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Implementation
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
        public DbSet<EmailDestination> EmailDestination { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var modified = ChangeTracker.Entries()
                .Where(t => t.State == EntityState.Modified)
                .Select(t => t.Entity);

            foreach (var a in modified)
            {
                ((IEntity)a).DateUpdated = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}