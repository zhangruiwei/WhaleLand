namespace WhaleLand.Example.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MySql.Data.MySqlClient;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using WhaleLand.Example.Events;
    using WhaleLand.Extensions.EventBus.Abstractions;
    using WhaleLand.Extensions.EventBus.Models;

    [Route("api/[controller]")]
    public class MQPublisherController : Controller
    {
        private readonly IEventLogger _eventLogger;
        private readonly IEventBus _eventBus;

        public MQPublisherController(
            IEventLogger eventLogger,
            IEventBus eventBus)
        {
            _eventLogger = eventLogger;
            _eventBus = eventBus;
        }

        [HttpGet]
        [Route("Publish")]
        public async Task<string> Publish()
        {
            var events = new List<EventLogEntry>
            {
                new EventLogEntry ("TestEventHandler",new TestEvent
                {
                     EventName = "1",
                     EventType = 1
                }),
                new EventLogEntry ("TestEventHandler",new TestEvent
                {
                     EventName = "2",
                     EventType = 2
                }),
                new EventLogEntry ("TestEventHandler",new TestEvent
                {
                     EventName = "2",
                     EventType = 2
                })
            };

            var ret = await _eventBus.PublishAsync(events);

            return ret.ToString();
        }


        [HttpGet]
        [Route("Save")]
        public async Task Save()
        {
            var connectionString = "Server=localshot;Port=3306;Database=; User=root;Password=123456;pooling=True;minpoolsize=1;maxpoolsize=100;connectiontimeout=180";

            using (var sqlConnection = new MySqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var sqlTran = await sqlConnection.BeginTransactionAsync();

                var events = new List<EventLogEntry>() {
                   new EventLogEntry("TestEventHandler",new TestEvent() {
                      EventName = "4",
                      EventType = 4
                   }),
                   new EventLogEntry("TestEventHandler",new {
                      EventName = "5",
                      EventType = 5
                   }),
            };

                //保存消息至业务数据库，保证写消息和业务操作在一个事务
                await _eventLogger.SaveEventAsync(events, sqlTran);

                return;
            }
        }

        [HttpGet]
        [Route("ReadPush")]
        public async Task ReadPush()
        {
            var unPublishedEventList = _eventLogger.GetUnPublishedEventList(1000);

            //通过消息总线发布消息
            var ret = await _eventBus.PublishAsync(unPublishedEventList);

            if (ret)
            {
                await _eventLogger.MarkEventAsPublishedAsync(unPublishedEventList.Select(a => a.EventId).ToList(), CancellationToken.None);
            }
            else
            {
                await _eventLogger.MarkEventAsPublishedFailedAsync(unPublishedEventList.Select(a => a.EventId).ToList(), CancellationToken.None);
            }
        }
    }
}
