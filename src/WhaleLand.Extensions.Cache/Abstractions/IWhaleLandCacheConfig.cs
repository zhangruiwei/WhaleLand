namespace WhaleLand.Extensions.Cache
{
    public interface IWhaleLandCacheConfig
    {
        /// <summary>
        /// 分区前缀(推荐使用服务名称)
        /// </summary>
        string CacheRegion { get; set; }

        /// <summary>
        /// 配置名称（默认：WhaleLandCache）
        /// </summary>
        string ConfigName { get; set; }
    }
}
