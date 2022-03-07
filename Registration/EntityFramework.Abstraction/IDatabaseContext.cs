using Entities.Registration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json.Nodes;

namespace EntityFramework.Abstraction
{
    public interface IDatabaseContext : IDisposable
    {
        DbSet<UseCases> UseCases { get; set; }

        //repo
        //Task AddAsync<T>(T newItem) where T : class;
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        //Task<T> SaveAsyncJsonObject<T>(JsonObject jsonObject, CancellationToken cancellationToken) where T : class;
        //public DbSet<T> Set<T>() where T : class;
    }
}