using Confluent.Kafka;
using System;

namespace WhaleLand.Extensions.EventBus.Kafka
{
    public interface IKafkaPersistentConnection
        : IDisposable
    {

        IProducer<string, string> GetProducer();

        IConsumer<string, string> GetConsumer();

    }
}
