using Entities.Base.Abstraction;
using Entities.EmailObject;
using Entities.Manager;
using Entities.SiteObject;
using EntityFramework.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DbSet<UseCases> UseCases { get; set; }
        public DbSet<CaseEvent> CaseEvent { get; set; }
        public DbSet<Site> Site { get; set; }
        public DbSet<SiteRule> SiteRule { get; set; }
        public DbSet<SiteSource> SiteSource { get; set; }
        public DbSet<Email> Email { get; set; }
        public DbSet<EmailRule> EmailRule { get; set; }
        public DbSet<EmailSource> EmailSource { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureDeleted();
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