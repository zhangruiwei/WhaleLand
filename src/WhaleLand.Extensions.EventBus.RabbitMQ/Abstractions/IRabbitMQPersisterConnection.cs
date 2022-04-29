using RabbitMQ.Client;
using System;

namespace WhaleLand.Extensions.EventBus.RabbitMQ
{
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel GetConsumer();

        IModel GetProducer();

    }
}
