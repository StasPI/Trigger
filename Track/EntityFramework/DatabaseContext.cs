using Entities.Base.Abstraction;
using Entities.Event;
using Entities.Manager;
using EntityFramework.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DbSet<UseCases> UseCases { get; set; }
        public DbSet<CaseEvent> CaseEvent { get; set; }
        public DbSet<EmailRule> EmailRule { get; set; }
        public DbSet<EmailSource> EmailSource { get; set; }
        public DbSet<SiteRule> SiteRule { get; set; }
        public DbSet<SiteSource> SiteSource { get; set; }

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

        //public async Task AddAsync<T>(T Item) where T : class
        //{
        //    await base.Set<T>().AddAsync(Item);
        //}

        //public async Task<T> SaveAsyncJsonObject<T>(JsonObject jsonObject, CancellationToken cancellationToken = default) where T : class
        //{
        //    T gClass = JsonSerializer.Deserialize<T>(jsonObject);
        //    await AddAsync<T>(gClass);
        //    await SaveChangesAsync(cancellationToken);
        //    return gClass;
        //}

        //public DbSet<T> Set<T>() where T : class
        //{
        //    return base.Set<T>();
        //}
    }
}