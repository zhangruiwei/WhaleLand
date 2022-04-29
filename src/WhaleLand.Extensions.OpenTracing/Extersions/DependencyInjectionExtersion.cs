using System;
using WhaleLand.Core;

namespace WhaleLand.Extensions.OpenTracing.Extersions
{
    public static class DependencyInjectionExtersion
    {
        public static IWhaleLandHostBuilder AddOpenTracing(this IWhaleLandHostBuilder hostBuilder, Action<IWhaleLandOpenTracingBuilder> action)
        {
            var builder = new WhaleLandOpenTracingBuilder(hostBuilder.Services);
            action(builder);
            return hostBuilder;
        }
    }
}
