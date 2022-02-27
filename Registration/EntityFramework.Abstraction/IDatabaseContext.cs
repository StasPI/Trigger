using Entities.Event;
using Entities.Manager;
using Entities.Reaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json.Nodes;

namespace EntityFramework.Abstraction
{
    public interface IDatabaseContext : IDisposable
    {
        //manager
        DbSet<UseCases> UseCases { get; set; }
        DbSet<CaseEvent> CaseEvents { get; set; }
        DbSet<CaseReaction> CaseReaction { get; set; }

        //event
        DbSet<EmailRule> EmailRule { get; set; }
        DbSet<EmailSource> EmailSource { get; set; }
        DbSet<SiteRule> SiteRule { get; set; }
        DbSet<SiteSource> SiteSource { get; set; }

        //reaction
        public DbSet<EmailDestination> EmailDestination { get; set; }

        //repo
        Task AddAsync<T>(T newItem) where T : class;
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        IDatabaseContext NewContext();
        Task<T> SaveAsyncJsonObject<T>(JsonObject jsonObject, CancellationToken cancellationToken) where T : class;
        //Task<DbSet<T>> Set<T>() where T : class;
        public DbSet<T> Set<T>() where T : class;
    }
}