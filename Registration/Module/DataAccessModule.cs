using EntityFramework;
using EntityFramework.Abstraction;
using Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules
{
    public class DataAccessModule : Module
    {
        public override void Load(IServiceCollection services)
        {
            services.AddDbContext<IDatabaseContext, DatabaseContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("Postgres"),
                    assembly => assembly.MigrationsAssembly(typeof(DataAccessModule).Assembly.GetName().Name)
                )
            );
        }
    }
}
