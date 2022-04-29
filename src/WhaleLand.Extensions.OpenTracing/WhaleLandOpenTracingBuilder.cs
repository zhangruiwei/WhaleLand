using Microsoft.Extensions.DependencyInjection;

namespace WhaleLand.Extensions.OpenTracing
{
    internal class WhaleLandOpenTracingBuilder : IWhaleLandOpenTracingBuilder
    {
        private IServiceCollection _services;

        public WhaleLandOpenTracingBuilder(IServiceCollection Services)
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
