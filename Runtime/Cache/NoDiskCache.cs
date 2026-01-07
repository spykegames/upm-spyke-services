using Cysharp.Threading.Tasks;

namespace Spyke.Services.Cache
{
    /// <summary>
    /// No-op disk cache implementation. Always returns null.
    /// Useful for testing or when persistent caching is disabled.
    /// </summary>
    public class NoDiskCache : IDiskCache
    {
        public int Count => 0;

        public void Initialize() { }
        public byte[] Get(string key) => null;
        public UniTask<byte[]> GetAsync(string key) => UniTask.FromResult<byte[]>(null);
        public void Put(string key, byte[] data) { }
        public UniTask PutAsync(string key, byte[] data) => UniTask.CompletedTask;
        public void Clear(string key) { }
        public void ClearAll() { }
        public bool Contains(string key) => false;
    }
}
