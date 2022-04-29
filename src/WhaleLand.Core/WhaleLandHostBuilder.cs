using Microsoft.Extensions.DependencyInjection;

namespace WhaleLand.Core
{
    public class WhaleLandHostBuilder : IWhaleLandHostBuilder
    {
        private readonly IServiceCollection _services;

        public WhaleLandHostBuilder(IServiceCollection services)
        {
            this._services = services;
        }

        public IServiceCollection Services
        {
            get
            {
                return _services;
            }
        }
    }
}
