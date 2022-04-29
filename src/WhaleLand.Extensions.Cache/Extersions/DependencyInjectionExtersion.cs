using CacheManager.Core;
using Microsoft.Extensions.Configuration;
using System;
using WhaleLand.Core;
using WhaleLand.Extensions.Cache;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtersion
    {
        static ICacheManager<T> GetCacheManager<T>(IServiceProvider sp, string configName)
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var cacheConfiguration = configuration.GetCacheConfiguration(configName).Builder.Build();
            var cacheManager = CacheManager.Core.CacheFactory.FromConfiguration<T>(configName, cacheConfiguration);
            return cacheManager;
        }

        public static IWhaleLandHostBuilder AddCache(this IWhaleLandHostBuilder hostBuilder, Action<IWhaleLandCacheConfig> setupOption = null)
        {
            var config = new WhaleLandCacheConfig();
            if (setupOption != null)
            {
                setupOption(config);
            }
            hostBuilder.Services.AddSingleton(typeof(IWhaleLandCacheConfig), config);
            hostBuilder.Services.AddSingleton(typeof(ICacheManager<int>), sp =>
            {
                return GetCacheManager<int>(sp, config.ConfigName);
            });
            hostBuilder.Services.AddSingleton(typeof(ICacheManager<long>), sp =>
            {
                return GetCacheManager<long>(sp, config.ConfigName);
            });
            hostBuilder.Services.AddSingleton(typeof(ICacheManager<string>), sp =>
            {
                return GetCacheManager<string>(sp, config.ConfigName);
            });
            hostBuilder.Services.AddSingleton(typeof(ICacheManager<bool>), sp =>
            {
                return GetCacheManager<bool>(sp, config.ConfigName);
            });
            hostBuilder.Services.AddSingleton(typeof(ICacheManager<object>), sp =>
            {
                return GetCacheManager<object>(sp, config.ConfigName);
            });

            hostBuilder.Services.AddSingleton(typeof(IWhaleLandCache<int>), sp =>
            {
                var cacheManager = sp.GetRequiredService<ICacheManager<int>>();
                return new WhaleLand.Extensions.Cache.WhaleLandCacheManager<int>(cacheManager, config.CacheRegion);
            });
            hostBuilder.Services.AddSingleton(typeof(IWhaleLandCache<long>), sp =>
            {
                var cacheManager = sp.GetRequiredService<ICacheManager<long>>();
                return new WhaleLand.Extensions.Cache.WhaleLandCacheManager<long>(cacheManager, config.CacheRegion);
            });
            hostBuilder.Services.AddSingleton(typeof(IWhaleLandCache<string>), sp =>
            {
                var cacheManager = sp.GetRequiredService<ICacheManager<string>>();
                return new WhaleLand.Extensions.Cache.WhaleLandCacheManager<string>(cacheManager, config.CacheRegion);
            });
            hostBuilder.Services.AddSingleton(typeof(IWhaleLandCache<bool>), sp =>
            {
                var cacheManager = sp.GetRequiredService<ICacheManager<bool>>();
                return new WhaleLand.Extensions.Cache.WhaleLandCacheManager<bool>(cacheManager, config.CacheRegion);
            });
            hostBuilder.Services.AddSingleton(typeof(IWhaleLandCache<object>), sp =>
            {
                var cacheManager = sp.GetRequiredService<ICacheManager<object>>();
                return new WhaleLand.Extensions.Cache.WhaleLandCacheManager<object>(cacheManager, config.CacheRegion);
            });
            return hostBuilder;
        }

        public static IWhaleLandHostBuilder AddCache(this IWhaleLandHostBuilder hostBuilder, Action<WhaleLand.Extensions.Cache.RedisConfigurationBuilder> configuration, string cacheRegion = "")
        {
            hostBuilder.Services.AddSingleton(typeof(IWhaleLandCache<int>), sp =>
            {
                return WhaleLand.Extensions.Cache.CacheFactory.Build<int>(configuration, cacheRegion);
            });
            hostBuilder.Services.AddSingleton(typeof(IWhaleLandCache<long>), sp =>
            {
                return WhaleLand.Extensions.Cache.CacheFactory.Build<long>(configuration, cacheRegion);
            });
            hostBuilder.Services.AddSingleton(typeof(IWhaleLandCache<string>), sp =>
            {
                return WhaleLand.Extensions.Cache.CacheFactory.Build<string>(configuration, cacheRegion);
            });
            hostBuilder.Services.AddSingleton(typeof(IWhaleLandCache<bool>), sp =>
            {
                return WhaleLand.Extensions.Cache.CacheFactory.Build<bool>(configuration, cacheRegion);
            });
            hostBuilder.Services.AddSingleton(typeof(IWhaleLandCache<object>), sp =>
            {
                return WhaleLand.Extensions.Cache.CacheFactory.Build<object>(configuration, cacheRegion);
            });
            return hostBuilder;

        }


    }
}


namespace WhaleLand.Extensions.Cache
{
    public class RedisConfigurationBuilder
    {
        private int ConnectionTimeout { get; set; } = 0;
        private bool AllowAdmin { get; set; } = true;
        private string Password { get; set; } = "";
        private string Host { get; set; } = "localhost";
        private int Port { get; set; } = 6378;
        private int Database { get; set; } = 0;

        public bool Ssl { get; set; } = false;

        public RedisConfigurationBuilder WithAllowAdmin()
        {
            this.AllowAdmin = true;
            return this;
        }

        public RedisConfigurationBuilder WithSsl()
        {
            this.Ssl = true;
            return this;
        }

        public RedisConfigurationBuilder WithDatabase(int database)
        {
            this.Database = database;
            return this;
        }
        public RedisConfigurationBuilder WithPassword(string password)
        {
            this.Password = password;
            return this;
        }
        public RedisConfigurationBuilder WithEndpoint(string host, int port)
        {
            this.Host = host;
            this.Port = port;
            return this;
        }

        public RedisConfigurationBuilder WithConnectionTimeout(int timeout)
        {
            this.ConnectionTimeout = timeout;
            return this;
        }

        public Action<CacheManager.Redis.RedisConfigurationBuilder> Build()
        {

            return (redis) =>
            {
                if (AllowAdmin)
                {
                    redis.WithAllowAdmin();
                }

                if (Ssl)
                {
                    redis.WithSsl(this.Host);
                }

                if (ConnectionTimeout > 0)
                {
                    redis.WithConnectionTimeout(ConnectionTimeout);
                }

                redis.WithDatabase(Database)
                .WithEndpoint(Host, Port)
                .WithPassword(Password);
            };
        }

    }

    public static class CacheFactory
    {
        public static IWhaleLandCache<T> Build<T>(Action<RedisConfigurationBuilder> configuration, string cacheRegion = "")
        {
            var _builder = new RedisConfigurationBuilder();
            configuration(_builder);

            var cacheManager = CacheManager.Core.CacheFactory.Build<T>("getStartedCache", settings =>
            {
                settings.WithJsonSerializer();
                settings.WithMicrosoftMemoryCacheHandle("handleName")
                .And
                .WithRedisConfiguration("redis", _builder.Build())
                .WithMaxRetries(100)
                .WithRetryTimeout(50)
                .WithRedisBackplane("redis")
                .WithRedisCacheHandle("redis", true);
            });

            WhaleLand.Extensions.Cache.IWhaleLandCache<T> cache = new WhaleLand.Extensions.Cache.WhaleLandCacheManager<T>(cacheManager, cacheRegion);
            return cache;
        }
    }
}
