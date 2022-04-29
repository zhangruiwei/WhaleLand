using Microsoft.Extensions.DependencyInjection;

namespace WhaleLand.Extensions.UidGenerator
{
    public interface IWorkIdCreateStrategyBuilder
    {
        IServiceCollection Services { get; }

        int CenterId { get; set; }
    }
}
