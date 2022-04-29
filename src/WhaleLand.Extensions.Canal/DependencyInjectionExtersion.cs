using Microsoft.Extensions.Configuration;
using WhaleLand.Core;
using WhaleLand.Extensions.Canal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtersion
    {
        public static IWhaleLandHostBuilder AddCanal(this IWhaleLandHostBuilder hostBuilder, IConfigurationSection configurationSection)
        {
            hostBuilder.Services.AddSingleton<CanalConfig>(sp =>
            {
                return configurationSection.Get<CanalConfig>();

            });
            hostBuilder.Services.AddHostedService<CanalClientHostedService>();
            return hostBuilder;


        }
    }
}
