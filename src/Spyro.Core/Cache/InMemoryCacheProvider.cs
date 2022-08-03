namespace Spyro.Cache
{
    public class InMemoryCacheProvider : ICacheProvider
    {
        public T Get<T>(string key)
        {
            throw new System.NotImplementedException();
        }

        public string Set<T>(T value)
        {
            throw new System.NotImplementedException();
        }
    }
}
