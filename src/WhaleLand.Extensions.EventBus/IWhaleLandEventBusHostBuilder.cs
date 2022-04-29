using Microsoft.Extensions.DependencyInjection;

namespace WhaleLand.Extensions.EventBus
{
    public interface IWhaleLandEventBusHostBuilder
    {
        IServiceCollection Services { get; }
    }
}
