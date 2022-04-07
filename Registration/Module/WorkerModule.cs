using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Options;
using Workers;

namespace Modules
{
    public class WorkerModule : Module
    {
        public override void Load(IServiceCollection services)
        {
            services.AddHostedService<WorkerEvents>();
            services.AddHostedService<WorkerReactions>();

            services.Configure<WorkerOptions>(Configuration.GetSection(WorkerOptions.Name));
        }
    }
}
