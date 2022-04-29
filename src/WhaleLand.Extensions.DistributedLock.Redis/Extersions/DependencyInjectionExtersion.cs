using System;
using WhaleLand.Core;
using WhaleLand.Extensions.DistributedLock;
using WhaleLand.Extensions.DistributedLock.Redis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtersion
    {
        public static IWhaleLandHostBuilder AddDistributedLock(this IWhaleLandHostBuilder hostBuilder, Action<Config> action)
        {
            action = action ?? throw new ArgumentNullException(nameof(action));
            var redisDistributedLock = DistributedLockFactory.Build(action);
            hostBuilder.Services.AddSingleton<IDistributedLock>(redisDistributedLock);
            return hostBuilder;
        }
    }
}


namespace WhaleLand.Extensions.DistributedLock.Redis
{
    public static class DistributedLockFactory
    {
        public static IDistributedLock Build(Action<Config> configSetup)
        {
            var config = new Config();
            configSetup(config);

            return new RedisDistributedLock(WhaleLand.Extensions.Redis.CacheFactory.Build(option =>
            {
                option.WithDb(config.DBNum);
                option.WithKeyPrefix(config.KeyPrefix);
                option.WithWriteServerList(config.WriteServerList);
                option.WithReadServerList(config.WriteServerList);
                option.WithPassword(config.Password);
                option.WithSsl(config.Ssl);
            }));
        }
    }
}


