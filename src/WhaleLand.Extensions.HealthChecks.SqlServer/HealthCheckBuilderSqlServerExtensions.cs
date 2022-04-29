using System;
using System.Data;
using System.Data.SqlClient;

namespace WhaleLand.Extensions.HealthChecks
{
    public static class HealthCheckBuilderSqlServerExtensions
    {
        public static HealthCheckBuilder AddSqlCheck(this HealthCheckBuilder builder, string name, string connectionString)
        {
            Guard.ArgumentNotNull(nameof(builder), builder);

            return AddSqlCheck(builder, name, connectionString, builder.DefaultCacheDuration);
        }

        public static HealthCheckBuilder AddSqlCheck(this HealthCheckBuilder builder, string name, string connectionString, TimeSpan cacheDuration)
        {
            builder.AddCheck($"SqlServerCheck({name})", async () =>
            {
                try
                {
                    //TODO: There is probably a much better way to do this.
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = "SELECT 1";
                            var result = (int)await command.ExecuteScalarAsync().ConfigureAwait(false);
                            if (result == 1)
                            {
                                return HealthCheckResult.Healthy($"Healthy");
                            }

                            return HealthCheckResult.Unhealthy($"Unhealthy");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy($"{ex.GetType().FullName}");
                }
            }, cacheDuration);

            return builder;
        }
    }
}
