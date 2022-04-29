using System;
using System.Collections.Generic;

namespace WhaleLand.Extensions.EventBus.Kafka
{
    public interface IKafkaPersisterConnectionLoadBalancerFactory
    {
        IRabbitMQPersisterConnectionLoadBalancer Get(Func<List<IKafkaPersistentConnection>> func, string Type);
    }
}
