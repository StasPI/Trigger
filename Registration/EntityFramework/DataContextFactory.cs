using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EntityFramework
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

            if (args.Length > 0)
            {
                var connectionString = args[0];
                optionsBuilder.UseNpgsql(connectionString);
            }
            else
            {
                var configuration = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json")
                  .AddJsonFile("appsettings.Development.json", true)
                  .Build();

                optionsBuilder.UseNpgsql(configuration.GetConnectionString("DbConnection"));
            }

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
