using Microsoft.Extensions.DependencyInjection;

namespace WhaleLand.Extensions.OpenTracing
{
    public interface IWhaleLandOpenTracingBuilder
    {
        IServiceCollection Services { get; }
    }
}
