using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WhaleLand.Core;
using WhaleLand.Extensions.EventBus;
using WhaleLand.Extensions.EventBus.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtersion
    {
        public static IWhaleLandHostBuilder AddEventBus(this IWhaleLandHostBuilder hostBuilder, Action<IWhaleLandEventBusHostBuilder> setup)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(a => a.GetTypes().Where(type => Array.Exists(type.GetInterfaces(), t => t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(IEventHandler<>) || t.GetGenericTypeDefinition() == typeof(IEventBatchHandler<>)))))
                       .ToArray();

            foreach (var type in types)
            {
                hostBuilder.Services.AddSingleton(type);
            }

            var builder = new WhaleLandEventBusHostBuilder(hostBuilder.Services);
            setup(builder);

            return hostBuilder;
        }


        public static IWhaleLandHostBuilder AddEventBus(this IWhaleLandHostBuilder hostBuilder, Action<IWhaleLandEventBusHostBuilder> setup, Func<System.Reflection.Assembly[]> assemblies)
        {
            var types = assemblies()
                       .SelectMany(a => a.GetTypes().Where(type => Array.Exists(type.GetInterfaces(), t => t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(IEventHandler<>) || t.GetGenericTypeDefinition() == typeof(IEventBatchHandler<>)))))
                       .ToArray();

            foreach (var type in types)
            {
                hostBuilder.Services.AddSingleton(type);
            }

            var builder = new WhaleLandEventBusHostBuilder(hostBuilder.Services);
            setup(builder);

            return hostBuilder;
        }

#if NETCORE
        public static IWhaleLandApplicationBuilder UseEventBus(this IWhaleLandApplicationBuilder hummingbirdApplicationBuilder, Action<IServiceProvider> setupSubscriberHandler)
        {

            setupSubscriberHandler(hummingbirdApplicationBuilder.app.ApplicationServices);

            return hummingbirdApplicationBuilder;
        }
#endif

        /// <summary>
        /// 使用消息总线订阅者
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="setupSubscriberHandler"></param>
        /// <returns></returns>
        public static IServiceProvider UseSubscriber(this IServiceProvider serviceProvider, Action<IEventBus> setupSubscriberHandler = null)
        {
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();
            var logger = serviceProvider.GetRequiredService<ILogger<IEventLogger>>();

            //订阅消息
            eventBus.Subscribe((Messages) =>
            {
                foreach (var message in Messages)
                {
                    logger.LogDebug($"ACK: queue {message.QueueName} route={message.RouteKey} messageId:{message.MessageId}");
                }

            }, async (obj) =>
            {
                await Task.Delay(1);
                foreach (var message in obj.Messages)
                {
                    logger.LogError($"NAck: queue {message.QueueName} route={message.RouteKey} messageId:{message.MessageId}");
                }

                //消息消费失败执行以下代码
                if (obj.Exception != null)
                {
                    logger.LogError(obj.Exception, obj.Exception.Message);
                }

                return true;
            });

            setupSubscriberHandler?.Invoke(eventBus);

            return serviceProvider;
        }

        public static IEventBus Register<TD, TH>(this IEventBus eventBus, string QueueName = "", string EventTypeName = "")
                     where TD : class
                     where TH : IEventHandler<TD>
        {
            return eventBus.Register<TD, TH>(EventTypeName);
        }

        public static IEventBus Register<TD, TH>(this IEventBus eventBus, string QueueName = "", string EventTypeName = "", int BatchSize = 10)
             where TD : class
             where TH : IEventBatchHandler<TD>
        {
            return eventBus.RegisterBatch<TD, TH>(QueueName, EventTypeName, BatchSize);
        }
    }
}
