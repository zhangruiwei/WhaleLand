using System.Collections.Generic;

namespace WhaleLand.Extensions.HealthChecks
{
    public class HealthCheckResults
    {
        public IList<IHealthCheckResult> CheckResults { get; } = new List<IHealthCheckResult>();
    }
}
