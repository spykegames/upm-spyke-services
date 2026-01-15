using System.Threading;
using Cysharp.Threading.Tasks;

namespace Spyke.Services.Cache
{
    /// <summary>
    /// Disk-based cache interface for persistent data storage.
    /// </summary>
    public interface IDiskCache
    {
        void Initialize();
        byte[] Get(string key);
        UniTask<byte[]> GetAsync(string key, CancellationToken cancellationToken = default);
        void Put(string key, byte[] data);
        UniTask PutAsync(string key, byte[] data, CancellationToken cancellationToken = default);
        void Clear(string key);
        void ClearAll();
        bool Contains(string key);
        int Count { get; }
    }
}
