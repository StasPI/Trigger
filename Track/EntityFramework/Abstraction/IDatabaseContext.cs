using Entities.Event;
using Entities.Manager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFramework.Abstraction
{
    public interface IDatabaseContext : IDisposable
    {
        DbSet<UseCases> UseCases { get; set; }
        DbSet<CaseEvent> CaseEvent { get; set; }
        DbSet<EmailRule> EmailRule { get; set; }
        DbSet<EmailSource> EmailSource { get; set; }
        DbSet<SiteRule> SiteRule { get; set; }
        DbSet<SiteSource> SiteSource { get; set; }

        //repo
        //Task AddAsync<T>(T newItem) where T : class;
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        //Task<T> SaveAsyncJsonObject<T>(JsonObject jsonObject, CancellationToken cancellationToken) where T : class;
        //public DbSet<T> Set<T>() where T : class;
    }
}