using System;
using System.Collections.Generic;

namespace WhaleLand.Extensions.EventBus.RabbitMQ
{
    public interface IRabbitMQPersisterConnectionLoadBalancerFactory
    {
        IRabbitMQPersisterConnectionLoadBalancer Get(Func<List<IRabbitMQPersistentConnection>> func, string Type);
    }
}
