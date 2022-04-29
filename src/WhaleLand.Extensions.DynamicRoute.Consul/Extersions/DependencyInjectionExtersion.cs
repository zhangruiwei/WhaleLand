﻿using Consul;
using Microsoft.Extensions.Configuration;
using System;
using WhaleLand.Core;
using WhaleLand.DynamicRoute;
using WhaleLand.Extensions.DynamicRoute.Consul;

namespace Microsoft.Extensions.DependencyInjection
{
    public class ConsulConfigBuilder
    {
        private ConsulConfig _config;

        public ConsulConfigBuilder()
        {
            _config = new ConsulConfig();
        }
        public ConsulConfigBuilder(ConsulConfig config)
        {
            _config = config;
        }


        public void AddTags(string Tag)
        {
            if (!string.IsNullOrEmpty(Tag))
            {
                _config.SERVICE_TAGS += $",{Tag}";
            }
        }

        public ConsulConfig Build()
        {
            return _config;
        }
    }

    public static partial class DependencyInjectionExtersion
    {
        public static IServiceCollection AddConsulDynamicRoute(this IServiceCollection services, ConsulConfig config, Action<ConsulConfigBuilder> setup = null)
        {
            ConsulConfigBuilder builder = new ConsulConfigBuilder(config);

            if (setup != null)
            {
                setup(builder);
            }

            config = builder.Build();

            services.AddSingleton<ConsulConfig>(a =>
            {
                return config;
            });
            services.AddSingleton<IConsulClient>(a =>
            {
                var _client = new ConsulClient(delegate (ConsulClientConfiguration obj)
                {
                    obj.Address = new Uri("http://" + config.SERVICE_REGISTRY_ADDRESS + ":" + config.SERVICE_REGISTRY_PORT);
                    obj.Datacenter = config.SERVICE_REGION;
                    obj.Token = config.SERVICE_REGISTRY_TOKEN;
                });

                return _client;

            });
            services.AddSingleton<IServiceLocator>(a =>
            {
                return new ConsulServiceLocator(config.SERVICE_REGISTRY_ADDRESS, config.SERVICE_REGISTRY_PORT, config.SERVICE_REGION, config.SERVICE_REGISTRY_TOKEN);
            });
            services.AddSingleton<IServiceDiscoveryProvider, ConsulServiceDiscoveryProvider>();
            services.AddHostedService<ConsulServiceRegisterHostedService>();

            return services;
        }

        public static IServiceCollection AddConsulDynamicRoute(this IServiceCollection services, IConfiguration configuration, Action<ConsulConfigBuilder> setup = null)
        {
            var config = configuration.Get<ConsulConfig>();
            return services.AddConsulDynamicRoute(config, setup);
        }


        public static IWhaleLandHostBuilder AddConsulDynamicRoute(this IWhaleLandHostBuilder hostBuilder, IConfiguration configuration)
        {
            hostBuilder.Services.AddConsulDynamicRoute(configuration);
            return hostBuilder;
        }

        public static IWhaleLandHostBuilder AddConsulDynamicRoute(this IWhaleLandHostBuilder hostBuilder, IConfiguration configuration, Action<ConsulConfigBuilder> setup = null)
        {
            hostBuilder.Services.AddConsulDynamicRoute(configuration, setup);
            return hostBuilder;
        }
    }

}
