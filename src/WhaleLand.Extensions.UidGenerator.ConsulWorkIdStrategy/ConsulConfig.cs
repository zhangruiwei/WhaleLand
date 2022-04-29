﻿namespace WhaleLand.Extensions.UidGenerator.ConsulWorkIdStrategy
{
    public class ConsulConfig
    {

        public ConsulConfig()
        { }

        /// <summary>
        /// 服务注册中心地址（默认:consul）
        /// </summary>
        public string SERVICE_REGISTRY_ADDRESS { get; set; } = "localhost";
        /// <summary>
        /// 服务注册中心端口(默认：8500)
        /// </summary>
        public string SERVICE_REGISTRY_PORT { get; set; } = "8500";
        /// <summary>
        /// 服务注册中心访问Token
        /// </summary>
        public string SERVICE_REGISTRY_TOKEN { get; set; } = "";

        /// <summary>
        /// 服务区域（隔离）
        /// </summary>
        public string SERVICE_REGION { get; set; } = "dc1";

    }
}
