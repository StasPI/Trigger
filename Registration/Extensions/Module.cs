using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions
{
    public abstract class Module
    {
        public IConfiguration Configuration { get; set; }
        public abstract void Load(IServiceCollection services);
    }
}