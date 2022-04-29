using System.Threading;
using System.Threading.Tasks;

namespace WhaleLand.Extensions.EventBus.Abstractions
{
    /// <summary>
    /// 事件处理程序
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface IEventHandler<in TEvent>
        where TEvent : class
    {
        Task<bool> Handle(TEvent @event, System.Collections.Generic.Dictionary<string, object> headers, CancellationToken cancellationToken);
    }

    /// <summary>
    /// 事件处理程序
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface IEventBatchHandler<in TEvent>
        where TEvent : class
    {
        Task<bool> Handle(TEvent[] @event, System.Collections.Generic.Dictionary<string, object>[] Headers, CancellationToken cancellationToken);
    }
}
