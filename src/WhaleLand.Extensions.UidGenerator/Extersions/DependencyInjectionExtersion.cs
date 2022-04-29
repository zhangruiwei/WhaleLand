using System;
using WhaleLand.Core;
using WhaleLand.Extensions.UidGenerator;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtersion
    {
        public static IWhaleLandHostBuilder AddSnowflakeUniqueIdGenerator(this IWhaleLandHostBuilder hostBuilder, Action<IWorkIdCreateStrategyBuilder> workIdCreateStrategyBuilder)
        {
            var builder = new WorkIdCreateStrategyBuilder(hostBuilder.Services);

            workIdCreateStrategyBuilder(builder);

            hostBuilder.Services.AddSingleton<IUniqueIdGenerator>(sp =>
            {
                var workIdCreateStrategy = sp.GetService<IWorkIdCreateStrategy>();
                var workId = workIdCreateStrategy.NextId().Result;
                return new SnowflakeUniqueIdGenerator(workId, builder.CenterId);
            });

#if NETCORE
            hostBuilder.Services.AddHostedService<InitWorkIdHostedService>();
#endif

            return hostBuilder;
        }

        public static IWorkIdCreateStrategyBuilder AddStaticWorkIdCreateStrategy(this IWorkIdCreateStrategyBuilder hostBuilder, int WorkId)
        {
            hostBuilder.Services.AddSingleton<IWorkIdCreateStrategy>(sp =>
            {
                var strategy = new StaticWorkIdCreateStrategy(WorkId);
                return strategy;
            });
            return hostBuilder;
        }

        public static IWorkIdCreateStrategyBuilder AddHostNameWorkIdCreateStrategy(this IWorkIdCreateStrategyBuilder hostBuilder)
        {
            hostBuilder.Services.AddSingleton<IWorkIdCreateStrategy>(sp =>
            {
                var strategy = new HostNameWorkIdCreateStrategy();
                return strategy;
            });
            return hostBuilder;
        }
    }
}
