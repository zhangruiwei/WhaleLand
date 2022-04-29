namespace WhaleLand.Extensions.Cache
{
    public class WhaleLandCacheConfig : IWhaleLandCacheConfig
    {
        /// <summary>
        /// 分区前缀
        /// </summary>
        public string CacheRegion { get; set; }
        /// <summary>
        /// 配置名称
        /// </summary>
        public string ConfigName { get; set; }
    }
}
