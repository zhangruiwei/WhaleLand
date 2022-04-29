using System.Threading;
using System.Threading.Tasks;

namespace WhaleLand.Extensions.HealthChecks
{
    public interface IHealthCheck
    {
        ValueTask<IHealthCheckResult> CheckAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
