using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using WhaleLand.Extensions.EventBus.Models;

namespace WhaleLand.Extensions.EventBus.Abstractions
{
    /// <summary>
    /// 事件日志
    /// </summary>
    public interface IEventLogger
    {
        Task<List<EventLogEntry>> SaveEventAsync(List<EventLogEntry> events, IDbTransaction transaction);

        /// <summary>
        /// 事件已经发布成功
        /// </summary>
        /// <param name="events"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task MarkEventAsPublishedAsync(List<long> events, CancellationToken cancellationToken);

        /// <summary>
        /// 事件发布失败
        /// </summary>
        /// <param name="events"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task MarkEventAsPublishedFailedAsync(List<long> events, CancellationToken cancellationToken);


        List<EventLogEntry> GetUnPublishedEventList(int Take);
    }
}
