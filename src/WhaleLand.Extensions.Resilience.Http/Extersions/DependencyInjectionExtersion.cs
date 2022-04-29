using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using WhaleLand.Core;
using WhaleLand.Extensions.Resilience.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtersion
    {
        public static IWhaleLandHostBuilder AddResilientHttpClient(this IWhaleLandHostBuilder hostBuilder, Action<string, ResilientHttpClientConfigOption> func = null)
        {
            hostBuilder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            hostBuilder.Services.AddSingleton<IHttpClientFactory, ResilientHttpClientFactory>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ResilientHttpClient>>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var serviceLocator = sp.GetService<WhaleLand.DynamicRoute.IServiceLocator>();

                return new ResilientHttpClientFactory(logger,
                    httpContextAccessor,
                    serviceLocator,
                    func);
            });
            hostBuilder.Services.AddSingleton<IHttpClient>(sp => sp.GetService<IHttpClientFactory>().CreateResilientHttpClient());
            return hostBuilder;

        }

        public static IWhaleLandHostBuilder AddResilientHttpClient(this IWhaleLandHostBuilder hostBuilder, Action<string, ResilientHttpClientConfigOption> func = null, HttpMessageHandler httpMessageHandler = null)
        {
            hostBuilder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            hostBuilder.Services.AddSingleton<IHttpClientFactory, ResilientHttpClientFactory>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ResilientHttpClient>>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var serviceLocator = sp.GetService<WhaleLand.DynamicRoute.IServiceLocator>();

                return new ResilientHttpClientFactory(logger,
                    httpContextAccessor,
                    serviceLocator,
                    func);
            });
            hostBuilder.Services.AddSingleton<IHttpClient>(sp => sp.GetService<IHttpClientFactory>().CreateResilientHttpClient(httpMessageHandler));
            return hostBuilder;

        }

        public static IWhaleLandHostBuilder AddStandardHttpClient(this IWhaleLandHostBuilder hostBuilder)
        {
            hostBuilder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            hostBuilder.Services.AddSingleton<IHttpClientFactory, StandardHttpClientFactory>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<StandardHttpClient>>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var serviceLocator = sp.GetService<WhaleLand.DynamicRoute.IServiceLocator>();
                return new StandardHttpClientFactory(logger, httpContextAccessor, serviceLocator);
            });
            hostBuilder.Services.AddSingleton<IHttpClient>(sp => sp.GetService<IHttpClientFactory>().CreateResilientHttpClient());
            return hostBuilder;

        }

        public static IWhaleLandHostBuilder AddStandardHttpClient(this IWhaleLandHostBuilder hostBuilder, HttpMessageHandler httpMessageHandler)
        {
            hostBuilder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            hostBuilder.Services.AddSingleton<IHttpClientFactory, StandardHttpClientFactory>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<StandardHttpClient>>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var serviceLocator = sp.GetService<WhaleLand.DynamicRoute.IServiceLocator>();
                return new StandardHttpClientFactory(logger, httpContextAccessor, serviceLocator);
            });
            hostBuilder.Services.AddSingleton<IHttpClient>(sp => sp.GetService<IHttpClientFactory>().CreateResilientHttpClient(httpMessageHandler));
            return hostBuilder;

        }
    }
}