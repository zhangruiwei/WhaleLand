using System;
using WhaleLand.Core;
using WhaleLand.Extensions.Idempotency;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtersion
    {
        /// <summary>
        /// 缓存实现幂等
        /// </summary>
        /// <param name="services"></param>
        public static IWhaleLandHostBuilder AddIdempotency(this IWhaleLandHostBuilder hostBuilder, Action<IIdempotencyOption> setupOption = null)
        {
            var option = new IdempotencyOption();

            if (setupOption != null)
            {
                setupOption(option);
            }

            hostBuilder.Services.AddSingleton<IIdempotencyOption>(option);
            hostBuilder.Services.AddSingleton<IRequestManager, CacheRequestManager>();
            return hostBuilder;
        }
    }
}
