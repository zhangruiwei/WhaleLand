using System;
using WhaleLand.Extensions.EventBus.MySqlLogging;
using WhaleLand.Extensions.EventBus;
using WhaleLand.Extensions.EventBus.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtersion
    {
        public static IWhaleLandEventBusHostBuilder AddMySqlEventLogging(this IWhaleLandEventBusHostBuilder hostBuilder, Action<MySqlConfiguration> setupFactory)
        {
            #region 配置

            setupFactory = setupFactory ?? throw new ArgumentNullException(nameof(setupFactory));
            var configuration = new MySqlConfiguration();
            setupFactory(configuration);

            #endregion

            hostBuilder.Services.AddTransient<MySqlConfiguration>(a => configuration);
            hostBuilder.Services.AddTransient<IDbConnectionFactory>(a => new DbConnectionFactory(configuration.ConnectionString));
            hostBuilder.Services.AddTransient<IEventLogger, MySqlEventLogger>();
            return hostBuilder;
        }
    }
}
