using System;

namespace Spyro.Cache
{
    public interface ICacheProvider
    {
        void Set<T>(string key, T value, ExpirationType expirationType = ExpirationType.Absolute, DateTimeOffset ExpireTime = default);
        void Set<T>(string key, Func<T> setter, ExpirationType expirationType = ExpirationType.Absolute, DateTimeOffset ExpireTime = default);
        T Get<T>(string key);
    }
}
