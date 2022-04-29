﻿using CanalSharp.Client;
using CanalSharp.Client.Impl;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WhaleLand.Extensions.Canal
{
    internal class CanalClientHostedService : IHostedService
    {
        private readonly CanalConfig _cannalConfig;
        private readonly ILogger<CanalClientHostedService> _logger;
        private readonly IList<ICanalConnector> _canalConnectors;

        public CanalClientHostedService(
            CanalConfig cannalConfig,
            ILogger<CanalClientHostedService> logger)
        {
            _cannalConfig = cannalConfig;
            _logger = logger;
            _canalConnectors = new List<ICanalConnector>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(1);
            try
            {
                foreach (var subscribeInfo in _cannalConfig.Subscribes)
                {

                    if (string.IsNullOrEmpty(subscribeInfo.Connector))
                    {
                        subscribeInfo.Format = typeof(Connectors.ConsoleConnector).FullName;
                    }

                    if (string.IsNullOrEmpty(subscribeInfo.Format))
                    {
                        subscribeInfo.Format = typeof(Formatters.MaxwellJson.Formatter).FullName;
                    }

                    var connector = System.Activator.CreateInstance(Type.GetType(subscribeInfo.Connector)) as IConnector;

                    var formater = System.Activator.CreateInstance(Type.GetType(subscribeInfo.Format)) as IFormater;

                    //创建一个简单 CanalClient 连接对象（此对象不支持集群）传入参数分别为 canal 地址、端口、destination、用户名、密码
                    var canalConnector = CanalConnectors.NewSingleConnector(
                        subscribeInfo.ConnectionInfo.Address,
                        subscribeInfo.ConnectionInfo.Port,
                        subscribeInfo.ConnectionInfo.Destination,
                        subscribeInfo.ConnectionInfo.UserName,
                        subscribeInfo.ConnectionInfo.Passsword);

                    //连接 Canal
                    canalConnector.Connect();

                    canalConnector.Subscribe(subscribeInfo.Filter);

                    _canalConnectors.Add(canalConnector);

                    while (true)
                    {
                        //获取数据 1024表示数据大小 单位为字节
                        var message = canalConnector.GetWithoutAck(subscribeInfo.BatchSize);
                        //批次id 可用于回滚
                        var batchId = message.Id;

                        if (batchId == -1 || message.Entries.Count <= 0)
                        {
                            Thread.Sleep(100);
                            continue;
                        }
                        else
                        {
                            var ret = connector.Process(message.Entries, formater);

                            if (ret)
                            {
                                canalConnector.Ack(batchId);
                            }
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_canalConnectors != null && _canalConnectors.Any())
            {
                foreach (var canalConnector in _canalConnectors)
                {
                    canalConnector.UnSubscribe();
                    canalConnector.Disconnect();
                }
            }
            return Task.FromResult(true);
        }
    }
}
