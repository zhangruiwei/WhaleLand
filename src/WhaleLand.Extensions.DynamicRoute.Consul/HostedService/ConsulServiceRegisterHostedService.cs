using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using WhaleLand.DynamicRoute;

namespace WhaleLand.Extensions.DynamicRoute.Consul
{
    public class ConsulServiceRegisterHostedService : Microsoft.Extensions.Hosting.IHostedService
    {
        private readonly ConsulConfig _serviceConfig;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceDiscoveryProvider _serviceDiscoveryProvider;
        private readonly System.Timers.Timer _timer;
        public ConsulServiceRegisterHostedService(
            IHostApplicationLifetime lifetime,
            IServiceProvider serviceProvider,
            IServiceDiscoveryProvider serviceDiscoveryProvider,
            ConsulConfig serviceConfig)
        {
            _lifetime = lifetime;
            _serviceProvider = serviceProvider;
            _cancellationTokenSource = new CancellationTokenSource();
            _serviceConfig = serviceConfig;
            _serviceDiscoveryProvider = serviceDiscoveryProvider;
            var interval = int.Parse(_serviceConfig.SERVICE_CHECK_INTERVAL.TrimEnd('s'));


            _timer = new System.Timers.Timer((double)(interval * 1000));

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(1);

            _lifetime.ApplicationStarted.Register(delegate
            {
                _serviceDiscoveryProvider.Register();

                _timer.Elapsed += delegate
                {
                    _serviceDiscoveryProvider.Heartbeat();
                };
                _timer.Start();
            });
            _lifetime.ApplicationStopping.Register(delegate
            {
                _timer.Stop();
                _serviceDiscoveryProvider.Deregister();

            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();

            return Task.CompletedTask;
        }
    }
}
