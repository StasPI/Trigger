using Commands;
using Extensions;
using MediatR;
using Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Modules
{
    public class MediatrModule : Module
    {
        public override void Load(IServiceCollection services)
        {
            services.AddMediatR(typeof(DeleteUseCasesCommand).Assembly);
            services.AddMediatR(typeof(GetByIdUseCasesCommand).Assembly);
            services.AddMediatR(typeof(PostUseCasesCommand).Assembly);
            services.AddMediatR(typeof(PutUseCasesCommand).Assembly);
            services.AddMediatR(typeof(EventsMessage).Assembly);
            services.AddMediatR(typeof(ReactionsMessage).Assembly);
        }
    }
}
