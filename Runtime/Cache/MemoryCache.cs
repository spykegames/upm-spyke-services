using System.Collections.Generic;

namespace Spyke.Services.Cache
{
    /// <summary>
    /// Generic in-memory cache implementation using Dictionary.
    /// Thread-safe for single-threaded Unity usage.
    /// </summary>
    public class MemoryCache<T> : IMemoryCache<T> where T : class
    {
        private readonly Dictionary<string, T> _cache = new();

        public int Count => _cache.Count;

        public T Get(string key)
        {
            return _cache.TryGetValue(key, out var value) ? value : null;
        }

        public void Put(string key, T data)
        {
            _cache[key] = data;
        }

        public void Clear(string key)
        {
            _cache.Remove(key);
        }

        public void ClearAll()
        {
            _cache.Clear();
        }

        public bool Contains(string key)
        {
            return _cache.ContainsKey(key);
        }
    }
}
