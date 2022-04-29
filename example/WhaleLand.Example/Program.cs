using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using WhaleLand.HealthChecks;

namespace WhaleLand.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Environment.SetEnvironmentVariable("Environment", env);

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseShutdownTimeout(TimeSpan.FromSeconds(30))
            .UseStartup<Startup>()
            .UseHealthChecks("/healthcheck")
            //.UseMetrics((builderContext, metricsBuilder) =>
            //{
            //    metricsBuilder.ToPrometheus();
            //    metricsBuilder.ToInfluxDb(builderContext.Configuration.GetSection("AppMetrics:Influxdb"));
            //})
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFileEx("Config/appsettings.json");
                config.AddJsonFileEx("Config/redis.json");
                config.AddJsonFileEx("Config/quartz.json");
                config.AddJsonFileEx("Config/mysql.json");
                config.AddJsonFileEx("Config/canal.json");
                config.AddJsonFileEx("Config/httpclient.json");
                config.AddJsonFileEx("Config/kafka.json");
                config.AddJsonFileEx("Config/metrics.json");
                config.AddJsonFileEx("Config/rabbitmq.json");
                config.AddJsonFileEx("Config/service.json");
                config.AddJsonFileEx("Config/sqlserver.json");
                config.AddJsonFileEx("Config/tracing.json");
                config.AddEnvironmentVariables();
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .Build();
    }
}
