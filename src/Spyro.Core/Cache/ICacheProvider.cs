namespace Spyro.Cache
{
    internal interface ICacheProvider
    {
        string Set<T>(T value);
        T Get<T>(string key);
    }
}
