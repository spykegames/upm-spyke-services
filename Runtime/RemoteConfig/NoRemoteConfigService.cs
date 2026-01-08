using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Spyke.Services.RemoteConfig
{
    /// <summary>
    /// No-op implementation of IRemoteConfigService.
    /// Returns default values. Use for testing or when remote config is disabled.
    /// </summary>
    public class NoRemoteConfigService : IRemoteConfigService
    {
        private readonly Dictionary<string, object> _defaults = new();
        private static readonly List<string> EmptyKeys = new();

        public bool IsInitialized => true;
        public bool HasFetched => false;
        public DateTime LastFetchTime => DateTime.MinValue;

        public event Action OnConfigUpdated;

        public UniTask<bool> FetchAsync(TimeSpan? cacheExpiration = null)
        {
            return UniTask.FromResult(false);
        }

        public UniTask<bool> FetchAndActivateAsync(TimeSpan? cacheExpiration = null)
        {
            return UniTask.FromResult(false);
        }

        public UniTask<bool> ActivateAsync()
        {
            return UniTask.FromResult(false);
        }

        public string GetString(string key, string defaultValue = "")
        {
            if (_defaults.TryGetValue(key, out var value) && value is string str)
            {
                return str;
            }
            return defaultValue;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            if (_defaults.TryGetValue(key, out var value))
            {
                return Convert.ToInt32(value);
            }
            return defaultValue;
        }

        public long GetLong(string key, long defaultValue = 0)
        {
            if (_defaults.TryGetValue(key, out var value))
            {
                return Convert.ToInt64(value);
            }
            return defaultValue;
        }

        public float GetFloat(string key, float defaultValue = 0f)
        {
            if (_defaults.TryGetValue(key, out var value))
            {
                return Convert.ToSingle(value);
            }
            return defaultValue;
        }

        public double GetDouble(string key, double defaultValue = 0.0)
        {
            if (_defaults.TryGetValue(key, out var value))
            {
                return Convert.ToDouble(value);
            }
            return defaultValue;
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            if (_defaults.TryGetValue(key, out var value))
            {
                return Convert.ToBoolean(value);
            }
            return defaultValue;
        }

        public IReadOnlyList<string> GetKeys()
        {
            return EmptyKeys;
        }

        public bool HasKey(string key)
        {
            return _defaults.ContainsKey(key);
        }

        public void SetDefaults(IDictionary<string, object> defaults)
        {
            _defaults.Clear();
            foreach (var kvp in defaults)
            {
                _defaults[kvp.Key] = kvp.Value;
            }
        }

        // Suppress unused event warning
        protected virtual void OnConfigUpdatedInternal()
        {
            OnConfigUpdated?.Invoke();
        }
    }
}
