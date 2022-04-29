using System.Threading.Tasks;

namespace WhaleLand.Extensions.EventBus.RabbitMQ
{
    public interface IRabbitMQPersisterConnectionLoadBalancer
    {
        Task<IRabbitMQPersistentConnection> Lease();
    }
}
