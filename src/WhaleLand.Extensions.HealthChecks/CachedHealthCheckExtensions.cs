using System;
using System.Threading;
using System.Threading.Tasks;

namespace WhaleLand.Extensions.HealthChecks
{
    public static class CachedHealthCheckExtensions
    {
        public static ValueTask<IHealthCheckResult> RunAsync(this CachedHealthCheck check, IServiceProvider serviceProvider)
        {
            Guard.ArgumentNotNull(nameof(check), check);

            return check.RunAsync(serviceProvider, CancellationToken.None);
        }
    }
}
