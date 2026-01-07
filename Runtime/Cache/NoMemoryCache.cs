namespace Spyke.Services.Cache
{
    /// <summary>
    /// No-op memory cache implementation. Always returns null/default.
    /// Useful for testing or when caching is disabled.
    /// </summary>
    public class NoMemoryCache<T> : IMemoryCache<T> where T : class
    {
        public int Count => 0;

        public T Get(string key) => null;
        public void Put(string key, T data) { }
        public void Clear(string key) { }
        public void ClearAll() { }
        public bool Contains(string key) => false;
    }
}
