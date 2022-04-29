using CacheManager.Core;
using System;

namespace WhaleLand.Extensions.Cache
{
    public class WhaleLandCacheManager<T> : IWhaleLandCache<T>
    {
        private readonly ICacheManager<T> _cacheManager;
        private readonly string _cacheRegion;

        private string PaddingPrefix(string region)
        {
            return $"{_cacheRegion}:{region}";
        }

        public WhaleLandCacheManager(
          ICacheManager<T> cacheManager,
          string cacheRegion)
        {
            _cacheRegion = cacheRegion;
            _cacheManager = cacheManager;
        }

        public void Add(string key, T value, TimeSpan ts, string region)
        {
            _cacheManager.Put(new CacheItem<T>(key, PaddingPrefix(region), value, ExpirationMode.Absolute, ts));

        }

        public bool Exists(string key, string region)
        {
            return key != null && _cacheManager.Exists(key, PaddingPrefix(region));
        }

        public T Get(string key, string region)
        {
            return _cacheManager.Get<T>(key, PaddingPrefix(region));
        }

        public void ClearRegion(string region)
        {
            _cacheManager.ClearRegion(PaddingPrefix(region));
        }

        public bool Delete(string key, string region)
        {
            return _cacheManager.Remove(key, PaddingPrefix(region));
        }

    }
}
