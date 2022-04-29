using Microsoft.Extensions.DependencyInjection;

namespace WhaleLand.Core
{
    public interface IWhaleLandHostBuilder
    {
        IServiceCollection Services { get; }
    }
}
