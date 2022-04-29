using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WhaleLand.Extensions.EventBus.Models;

namespace WhaleLand.Extensions.EventBus.Abstractions
{
    public interface IEventBus
    {
        Task PublishNonConfirmAsync(List<Models.EventLogEntry> Events, CancellationToken cancellationToken = default(CancellationToken));


        Task<bool> PublishAsync(List<Models.EventLogEntry> Events, CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// 订阅消息（同一类消息可以重复订阅）
        /// </summary>
        /// <typeparam name="TD"></typeparam>
        /// <typeparam name="TH"></typeparam>
        /// <param name="QueueName">队列名称</param>
        /// <param name="EventTypeName">事件类型名称</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IEventBus Register<TD, TH>(string QueueName = "", string EventTypeName = "", CancellationToken cancellationToken = default(CancellationToken))
          where TD : class
          where TH : IEventHandler<TD>;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TD"></typeparam>
        /// <typeparam name="TH"></typeparam>
        /// <param name="QueueName">队列名称</param>
        /// <param name="EventTypeName">事件类型名称</param>
        /// <param name="BatchSize">批量获取消息大小</param>
        /// <returns></returns>
        IEventBus RegisterBatch<TD, TH>(string QueueName = "", string EventTypeName = "", int BatchSize = 50, CancellationToken cancellationToken = default(CancellationToken))
               where TD : class
                 where TH : IEventBatchHandler<TD>;

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="ackHandler"></param>
        /// <param name="nackHandler"></param>
        /// <returns></returns>
        IEventBus Subscribe(
        Action<EventResponse[]> ackHandler,
        Func<(EventResponse[] Messages, Exception Exception), Task<bool>> nackHandler);

    }
}
