using Microsoft.Extensions.DependencyInjection;

namespace WhaleLand.Extensions.EventBus
{
    internal class WhaleLandEventBusHostBuilder : IWhaleLandEventBusHostBuilder
    {
        private IServiceCollection _services;

        public WhaleLandEventBusHostBuilder(IServiceCollection Services)
        {
            this._services = Services;
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
