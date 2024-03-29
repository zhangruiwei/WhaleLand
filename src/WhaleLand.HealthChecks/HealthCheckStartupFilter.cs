﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

namespace WhaleLand.HealthChecks
{
    public class HealthCheckStartupFilter : IStartupFilter
    {
        private string _path;
        private int? _port;
        private TimeSpan _timeout;

        public HealthCheckStartupFilter(int port, TimeSpan timeout)
        {
            _port = port;
            _timeout = timeout;
        }

        public HealthCheckStartupFilter(string path, TimeSpan timeout)
        {
            _path = path;
            _timeout = timeout;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                if (_port.HasValue)
                {
                    app.UseMiddleware<HealthCheckMiddleware>(_port, _timeout);
                }
                else
                {
                    app.UseMiddleware<HealthCheckMiddleware>(_path, _timeout);
                }

                next(app);
            };
        }
    }
}
