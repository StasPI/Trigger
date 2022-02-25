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
        DbSet<EmailRules> EmailRules { get; set; }
        DbSet<EmailSource> EmailSource { get; set; }
        DbSet<SiteRules> SiteRules { get; set; }
        DbSet<SiteSource> SiteSource { get; set; }

        //reaction
        public DbSet<EmailDestination> EmailDestination { get; set; }

        //repo
        Task AddAsync<T>(T newItem) where T : class;
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        IDatabaseContext NewContext();
        Task<T> SaveAsyncJsonObject<T>(CancellationToken cancellationToken, JsonObject jsonObject) where T : class;
    }
}