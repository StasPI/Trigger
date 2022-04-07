using Entities.Registration;
using EntityFramework.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DbSet<UseCases> UseCases { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}