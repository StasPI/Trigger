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
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}