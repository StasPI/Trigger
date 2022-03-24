using Entities.EmailObject;
using Entities.Manager;
using Entities.SiteObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFramework.Abstraction
{
    public interface IDatabaseContext : IDisposable
    {
        DbSet<EventMessages> EventMessages { get; set; }
        DbSet<CaseEvent> CaseEvent { get; set; }
        DbSet<Site> Site { get; set; }
        DbSet<SiteRule> SiteRule { get; set; }
        DbSet<SiteSource> SiteSource { get; set; }
        DbSet<Email> Email { get; set; }
        DbSet<EmailRule> EmailRule { get; set; }
        DbSet<EmailSource> EmailSource { get; set; }

        //repo
        //Task AddAsync<T>(T newItem) where T : class;
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        //Task<T> SaveAsyncJsonObject<T>(JsonObject jsonObject, CancellationToken cancellationToken) where T : class;
        //public DbSet<T> Set<T>() where T : class;
    }
}