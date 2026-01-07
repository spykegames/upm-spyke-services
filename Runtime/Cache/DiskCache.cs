using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Spyke.Services.Cache
{
    /// <summary>
    /// Disk-based cache implementation with LRU eviction and expiry.
    /// Files are stored in Application.persistentDataPath/{directory}/
    /// </summary>
    public class DiskCache : IDiskCache
    {
        private readonly int _maxFiles;
        private readonly string _directory;
        private readonly TimeSpan _expiry;
        private readonly MD5 _md5 = MD5.Create();

        private string _path;
        private List<string> _cachedFiles;

        private const string CacheExtension = ".cache";

        public int Count => _cachedFiles?.Count ?? 0;

        /// <summary>
        /// Creates a new disk cache.
        /// </summary>
        /// <param name="maxFiles">Maximum number of cached files</param>
        /// <param name="directory">Subdirectory name under persistentDataPath</param>
        /// <param name="expiry">How long before cached files expire</param>
        public DiskCache(int maxFiles, string directory, TimeSpan expiry)
        {
            _maxFiles = maxFiles;
            _directory = directory;
            _expiry = expiry;
        }

        public void Initialize()
        {
            _path = Path.Combine(Application.persistentDataPath, _directory);

            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            // Load existing cache files, sorted by creation time (newest first)
            var existingFiles = new DirectoryInfo(_path)
                .GetFiles($"*{CacheExtension}")
                .OrderByDescending(f => f.CreationTime)
                .ToList();

            // Remove files exceeding max count
            for (var i = existingFiles.Count - 1; i >= _maxFiles; i--)
            {
                SafeDeleteFile(existingFiles[i].FullName);
                existingFiles.RemoveAt(i);
            }

            // Remove expired files
            var now = DateTime.Now;
            for (var i = existingFiles.Count - 1; i >= 0; i--)
            {
                if (now - existingFiles[i].CreationTime > _expiry)
                {
                    SafeDeleteFile(existingFiles[i].FullName);
                    existingFiles.RemoveAt(i);
                }
            }

            _cachedFiles = existingFiles.Select(f => f.Name).ToList();

#if SPYKE_DEV
            Debug.Log($"[DiskCache] Initialized at {_path} with {_cachedFiles.Count} files");
#endif
        }

        public byte[] Get(string key)
        {
            EnsureInitialized();

            var fileName = GetFileName(key);
            if (!_cachedFiles.Contains(fileName))
            {
                return null;
            }

            try
            {
                var filePath = Path.Combine(_path, fileName);
                return File.ReadAllBytes(filePath);
            }
            catch (Exception e)
            {
#if SPYKE_DEV
                Debug.LogWarning($"[DiskCache] Failed to read {key}: {e.Message}");
#endif
                return null;
            }
        }

        public async UniTask<byte[]> GetAsync(string key)
        {
            EnsureInitialized();

            var fileName = GetFileName(key);
            if (!_cachedFiles.Contains(fileName))
            {
                return null;
            }

            try
            {
                var filePath = Path.Combine(_path, fileName);
                return await File.ReadAllBytesAsync(filePath);
            }
            catch (Exception e)
            {
#if SPYKE_DEV
                Debug.LogWarning($"[DiskCache] Failed to read async {key}: {e.Message}");
#endif
                return null;
            }
        }

        public void Put(string key, byte[] data)
        {
            EnsureInitialized();

            var fileName = GetFileName(key);
            var filePath = Path.Combine(_path, fileName);

            try
            {
                File.WriteAllBytes(filePath, data);

                // Update cache list (move to front for LRU)
                _cachedFiles.Remove(fileName);
                _cachedFiles.Insert(0, fileName);

                // Evict oldest if over limit
                EvictIfNeeded();
            }
            catch (Exception e)
            {
#if SPYKE_DEV
                Debug.LogWarning($"[DiskCache] Failed to write {key}: {e.Message}");
#endif
            }
        }

        public async UniTask PutAsync(string key, byte[] data)
        {
            EnsureInitialized();

            var fileName = GetFileName(key);
            var filePath = Path.Combine(_path, fileName);

            try
            {
                await File.WriteAllBytesAsync(filePath, data);

                // Update cache list (move to front for LRU)
                _cachedFiles.Remove(fileName);
                _cachedFiles.Insert(0, fileName);

                // Evict oldest if over limit
                EvictIfNeeded();
            }
            catch (Exception e)
            {
#if SPYKE_DEV
                Debug.LogWarning($"[DiskCache] Failed to write async {key}: {e.Message}");
#endif
            }
        }

        public void Clear(string key)
        {
            EnsureInitialized();

            var fileName = GetFileName(key);
            if (_cachedFiles.Contains(fileName))
            {
                SafeDeleteFile(Path.Combine(_path, fileName));
                _cachedFiles.Remove(fileName);
            }
        }

        public void ClearAll()
        {
            EnsureInitialized();

            foreach (var fileName in _cachedFiles)
            {
                SafeDeleteFile(Path.Combine(_path, fileName));
            }
            _cachedFiles.Clear();
        }

        public bool Contains(string key)
        {
            EnsureInitialized();
            return _cachedFiles.Contains(GetFileName(key));
        }

        private string GetFileName(string key)
        {
            var hash = _md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            var sb = new StringBuilder();
            foreach (var b in hash)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString() + CacheExtension;
        }

        private void EvictIfNeeded()
        {
            while (_cachedFiles.Count > _maxFiles)
            {
                var oldest = _cachedFiles[_cachedFiles.Count - 1];
                SafeDeleteFile(Path.Combine(_path, oldest));
                _cachedFiles.RemoveAt(_cachedFiles.Count - 1);
            }
        }

        private void SafeDeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception e)
            {
#if SPYKE_DEV
                Debug.LogWarning($"[DiskCache] Failed to delete {path}: {e.Message}");
#endif
            }
        }

        private void EnsureInitialized()
        {
            if (_cachedFiles == null)
            {
                throw new InvalidOperationException("DiskCache not initialized. Call Initialize() first.");
            }
        }
    }
}
