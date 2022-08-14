
using System;
using System.Runtime.Caching;

namespace Spyro.Cache
{
    public class InMemoryCache : ICacheProvider
    {
        private readonly ObjectCache cache = MemoryCache.Default;
        public T Get<T>(string key)
        {
            var value = cache.Get(key);
            if (value is null)
                return default(T);

            return (T)value;
        }
        public void Set<T>(string key, T value, ExpirationType expirationType = ExpirationType.Absolute, DateTimeOffset ExpireTime = default)
        {

            CacheItem cacheItem = new CacheItem(key, value);
            CacheItemPolicy cacheItemPolicy = new();
            if (ExpireTime == default)
                ExpireTime = DateTimeOffset.UtcNow.AddMinutes(2);

            if (expirationType == ExpirationType.Absolute)
                cacheItemPolicy.AbsoluteExpiration = ExpireTime;
            if (expirationType == ExpirationType.Sliding)
                cacheItemPolicy.SlidingExpiration = ExpireTime.TimeOfDay;

            cache.Set(cacheItem, cacheItemPolicy);
        }

        public void Set<T>(string key, Func<T> setter, ExpirationType expirationType = ExpirationType.Absolute, DateTimeOffset ExpireTime = default)
        {
            T value = setter.Invoke();

            CacheItem cacheItem = new CacheItem(key, value);
            CacheItemPolicy cacheItemPolicy = new();
            if (ExpireTime == default)
                ExpireTime = DateTimeOffset.UtcNow.AddMinutes(2);

            if (expirationType == ExpirationType.Absolute)
                cacheItemPolicy.AbsoluteExpiration = ExpireTime;
            if (expirationType == ExpirationType.Sliding)
                cacheItemPolicy.SlidingExpiration = ExpireTime.TimeOfDay;

            cache.Set(cacheItem, cacheItemPolicy);
        }
    }
}
