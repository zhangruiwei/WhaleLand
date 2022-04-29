namespace WhaleLand.DynamicRoute
{
    public interface IServiceDiscoveryProvider
    {
        void Register();

        void Deregister();

        void Heartbeat();

        string ServiceId { get; }
    }
}
