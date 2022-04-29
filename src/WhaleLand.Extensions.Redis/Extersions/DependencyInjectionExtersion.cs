using WhaleLand.Extensions.Redis;
using System;
using WhaleLand.Extensions.Redis.StackExchange;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtersion
    {
        public static WhaleLand.Core.IWhaleLandHostBuilder AddRedis(this WhaleLand.Core.IWhaleLandHostBuilder hostBuilder, Action<RedisCacheConfig> action)
        {
            action = action ?? throw new ArgumentNullException(nameof(action));

            hostBuilder.Services.AddSingleton<ICacheManager>(CacheFactory.Build(action));

            return hostBuilder;

        }
    }
}


namespace WhaleLand.Extensions.Redis
{
    public static class CacheFactory
    {
        public static ICacheManager Build(Action<RedisCacheConfig> action)
        {
            var option = new RedisCacheConfig();
            action(option);

            var cacheManager = RedisCacheManage.Create(option);
            return cacheManager;
        }

        public static ICacheManager Build(RedisCacheConfig option)
        {
            var cacheManager = RedisCacheManage.Create(option);
            return cacheManager;
        }
    }
}
