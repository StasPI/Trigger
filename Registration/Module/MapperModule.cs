using AutoMapper;
using Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Modules
{
    public class MapperModule : Module
    {
        public override void Load(IServiceCollection services)
        {
            services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));
        }
        private static MapperConfiguration GetMapperConfiguration()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<Mapping.Registration.UseCasesMappingsProfile>();
            });
            configuration.AssertConfigurationIsValid();
            return configuration;
        }
    }
}
