using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using WhaleLand.Example.Events;
using WhaleLand.Extensions.EventBus.Abstractions;
using WhaleLand.Extensions.EventBus.RabbitMQ;

namespace WhaleLand.Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors()
           .AddMvc(a => a.EnableEndpointRouting = false)
           .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
           .AddControllersAsServices();  //全局配置Json序列化处理

            services.AddHealthChecks(checks =>
            {
                checks.WithDefaultCacheDuration(TimeSpan.FromSeconds(30));

                #region MySql

                var sqlConnectiongString = Configuration["mysql:Default"];
                //checks.AddMySqlCheck("mysql", sqlConnectiongString);

                #endregion

                #region Redis

                var redis_Host = Configuration["Redis:Host"];
                var redis_Port = Configuration["Redis:Port"];
                var redis_Password = Configuration["Redis:Password"];

                //checks.AddRedisCheck($"{redis_Host}:{redis_Port}", TimeSpan.FromSeconds(30), $"{redis_Host}:{redis_Port},password={redis_Password},allowAdmin=true,ssl=false,abortConnect=false,connectTimeout=5000");

                #endregion

                #region Rabbitmq

                //checks.AddRabbitMQCheck($"{Configuration["EventBus:HostName"]}", rabbitmq =>
                //{
                //    rabbitmq.WithEndPoint(Configuration["EventBus:HostName"] ?? "localhost", int.Parse(Configuration["EventBus:Port"] ?? "5672"));
                //    rabbitmq.WithAuth(Configuration["EventBus:UserName"] ?? "guest", Configuration["EventBus:Password"] ?? "guest");
                //    rabbitmq.WithExchange(Configuration["EventBus:VirtualHost"] ?? "/");
                //});

                #endregion

                #region Kafka

                //checks.AddKafkaCheck("kafka", new Confluent.Kafka.ProducerConfig()
                //{
                //    Acks = Confluent.Kafka.Acks.All,
                //    BootstrapServers = Configuration["Kafka:Sender:bootstrap.servers"]
                //});

                #endregion
            });

            services.AddWhaleLand(whaleland =>
            {
                var redis_host = Configuration["Redis:Host"];
                var redis_port = int.Parse(Configuration["Redis:Port"]);
                var redis_password = Configuration["Redis:Password"];

                whaleland.AddCache(config =>
                {
                    config.WithDatabase(0);
                    config.WithEndpoint(redis_host, redis_port);
                    config.WithPassword(redis_password);

                }, "WhaleLand.Cache")
                .AddRedis((option) =>
                {
                    option.WithDb(0);
                    option.WithKeyPrefix("WhaleLand.Redis");
                    option.WithPassword(redis_password);
                    option.WithReadServerList($"{redis_host}:{redis_port}");
                    option.WithWriteServerList($"{redis_host}:{redis_port}");
                    option.WithSsl(false);
                })
                .AddDistributedLock((option) =>
                {
                    option.WithDb(0);
                    option.WithKeyPrefix("WhaleLand.Redis");
                    option.WithServerList($"{redis_host}:{redis_port}");
                    option.WithPassword(redis_password);
                    option.WithSsl(false);
                })
                .AddSnowflakeUniqueIdGenerator((option) =>
                {
                    option.CenterId = int.Parse(Configuration["CenterId"]);
                    option.AddStaticWorkIdCreateStrategy(int.Parse(Configuration["WorkId"]));
                    //option.AddConsulWorkIdCreateStrategy("ZT.FLMS.ORDERSERVICE");
                })
                .AddEventBus((builder) =>
                {
                    var sqlserverServer = Configuration["SQLServer:Server"];
                    var sqlserverDatabase = Configuration["SQLServer:Database"];
                    var sqlserverUserId = Configuration["SQLServer:UserId"];
                    var sqlserverPassword = Configuration["SQLServer:Password"];
                    var sqlserverConnectionString = $"Server={sqlserverServer};Database={sqlserverDatabase};User Id={sqlserverUserId};Password={sqlserverPassword};MultipleActiveResultSets=true";

                    builder
                    .AddMySqlEventLogging(config => config.WithEndpoint(Configuration["Mysql:ConnectionString"]))
                    //.AddSqlServerEventLogging(config => config.WithEndpoint(sqlserverConnectionString))
                    .AddRabbitmq(factory =>
                    {
                        factory.WithEndPoint(Configuration["EventBus:HostName"] ?? "localhost", int.Parse(Configuration["EventBus:Port"] ?? "5672"));
                        factory.WithAuth(Configuration["EventBus:UserName"] ?? "guest", Configuration["EventBus:Password"] ?? "guest");
                        factory.WithExchange(Configuration["EventBus:VirtualHost"] ?? "/");
                        factory.WithReceiver(PreFetch: 10, ReceiverMaxConnections: 1, ReveiverMaxDegreeOfParallelism: 1);
                        factory.WithSender(10);
                    });
                });

                whaleland.AddQuartz(Configuration.GetSection("Quartz"));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            var logger = app.ApplicationServices.GetRequiredService<ILogger<IEventLogger>>();

            app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(_ => true));
            app.UseMvc();

            app.UseWhaleLand(whale =>
            {
                whale.UseEventBus(sp =>
                {
                    sp.UseSubscriber(eventbus =>
                    {
                        //eventbus.Register<TestEvent, TestEventHandler>("TestEventHandler", "TestEventHandler");

                        //订阅信息
                        eventbus.Subscribe((Messages) =>
                        {
                            foreach (var message in Messages)
                            {
                                logger.LogDebug($"ACK: queue {message.QueueName} route={message.RouteKey} messageId:{message.MessageId}");
                            }
                        }, async (obj) =>
                         {
                             foreach (var message in obj.Messages)
                             {
                                 logger.LogError($"NAck: queue {message.QueueName} route={message.RouteKey} messageId:{message.MessageId}");
                             }

                             //消息消费失败执行以下代码
                             if (obj.Exception != null)
                             {
                                 logger.LogError(obj.Exception, obj.Exception.Message);
                             }

                             var events = obj.Messages.Select(message => message.WaitAndRetry(a => 5, 3)).ToList();

                             var ret = !(await eventBus.PublishAsync(events));

                             return ret;
                         });
                    });
                });
            });
        }
    }
}
