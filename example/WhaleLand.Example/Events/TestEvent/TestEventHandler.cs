using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WhaleLand.Extensions.EventBus.Abstractions;

namespace WhaleLand.Example.Events
{
    public class TestEventHandler : IEventHandler<TestEvent>, IEventBatchHandler<TestEvent>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="headers"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> Handle(TestEvent @event, Dictionary<string, object> headers, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="event"></param>
        /// <param name="Headers"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> Handle(TestEvent[] @event, Dictionary<string, object>[] Headers, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
