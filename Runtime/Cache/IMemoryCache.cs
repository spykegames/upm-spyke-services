namespace Spyke.Services.Cache
{
    /// <summary>
    /// Generic in-memory cache interface.
    /// </summary>
    public interface IMemoryCache<T>
    {
        T Get(string key);
        void Put(string key, T data);
        void Clear(string key);
        void ClearAll();
        bool Contains(string key);
        int Count { get; }
    }
}
