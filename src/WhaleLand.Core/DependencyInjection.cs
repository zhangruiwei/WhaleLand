using System;
using WhaleLand.Core;

#if NETCORE
using Microsoft.AspNetCore.Builder;
#endif

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWhaleLand(this IServiceCollection services, Action<IWhaleLandHostBuilder> setup)
        {
            var builder = new WhaleLandHostBuilder(services);
            setup(builder);
            return services;
        }

#if NETCORE
        public static IWhaleLandApplicationBuilder UseWhaleLand(this IApplicationBuilder app, Action<IWhaleLandApplicationBuilder> setup)
        {
            var builder = new WhaleLandApplicationBuilder(app);
            setup(builder);
            return builder;
        }
#endif
    }
}
