using Entities.Abstraction;
using Entities.Event;
using Entities.Manager;
using Entities.Reaction;
using EntityFramework.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Nodes;

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

        private readonly DbContextOptions<DatabaseContext> _options;
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            _options = options;
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

        public async Task AddAsync<T>(T Item) where T : class
        {
            await base.Set<T>().AddAsync(Item);
        }

        public IDatabaseContext NewContext()
        {
            IDatabaseContext dbContext = new DatabaseContext(_options);

            return dbContext;
        }

        public async Task<T> SaveAsyncJsonObject<T>(CancellationToken cancellationToken, JsonObject jsonObject) where T : class
        {
            T gClass = JsonSerializer.Deserialize<T>(jsonObject);
            await AddAsync<T>(gClass);
            await SaveChangesAsync(cancellationToken);
            return gClass;
        }
    }
}