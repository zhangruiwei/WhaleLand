using System;
using WhaleLand.Extensions.EventBus;
using WhaleLand.Extensions.EventBus.Abstractions;
using WhaleLand.Extensions.EventBus.SqlServerLogging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtersion
    {
        public static IWhaleLandEventBusHostBuilder AddSqlServerEventLogging(this IWhaleLandEventBusHostBuilder hostBuilder, Action<SqlServerConfiguration> setupFactory)
        {
            #region 配置
            setupFactory = setupFactory ?? throw new ArgumentNullException(nameof(setupFactory));
            var configuration = new SqlServerConfiguration();
            setupFactory(configuration);
            #endregion

            hostBuilder.Services.AddTransient<SqlServerConfiguration>(a => configuration);
            hostBuilder.Services.AddTransient<IDbConnectionFactory>(a => new DbConnectionFactory(configuration.ConnectionString));
            hostBuilder.Services.AddTransient<IEventLogger, SqlServerEventLogger>();
            return hostBuilder;
        }
    }
}