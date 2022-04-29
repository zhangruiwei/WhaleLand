using System.Threading.Tasks;

namespace WhaleLand.Extensions.EventBus.Kafka
{
    public interface IRabbitMQPersisterConnectionLoadBalancer
    {
        Task<IKafkaPersistentConnection> Lease();

    }
}
