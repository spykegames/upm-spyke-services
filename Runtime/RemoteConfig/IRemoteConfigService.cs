using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Spyke.Services.RemoteConfig
{
    /// <summary>
    /// Service interface for remote configuration / feature flags.
    /// Implemented by platform-specific providers (Firebase Remote Config, etc.).
    /// </summary>
    public interface IRemoteConfigService
    {
        /// <summary>
        /// Whether the service is initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Whether a fetch has completed successfully.
        /// </summary>
        bool HasFetched { get; }

        /// <summary>
        /// Last fetch time (UTC).
        /// </summary>
        DateTime LastFetchTime { get; }

        /// <summary>
        /// Event fired when config values are updated.
        /// </summary>
        event Action OnConfigUpdated;

        /// <summary>
        /// Fetches remote config values from the server.
        /// </summary>
        /// <param name="cacheExpiration">Cache expiration time. Pass TimeSpan.Zero to force fetch.</param>
        /// <returns>True if fetch was successful.</returns>
        UniTask<bool> FetchAsync(TimeSpan? cacheExpiration = null);

        /// <summary>
        /// Fetches and activates remote config values.
        /// </summary>
        /// <param name="cacheExpiration">Cache expiration time.</param>
        /// <returns>True if fetch and activate was successful.</returns>
        UniTask<bool> FetchAndActivateAsync(TimeSpan? cacheExpiration = null);

        /// <summary>
        /// Activates the last fetched config values.
        /// </summary>
        /// <returns>True if activation was successful.</returns>
        UniTask<bool> ActivateAsync();

        /// <summary>
        /// Gets a string value.
        /// </summary>
        string GetString(string key, string defaultValue = "");

        /// <summary>
        /// Gets an integer value.
        /// </summary>
        int GetInt(string key, int defaultValue = 0);

        /// <summary>
        /// Gets a long value.
        /// </summary>
        long GetLong(string key, long defaultValue = 0);

        /// <summary>
        /// Gets a float value.
        /// </summary>
        float GetFloat(string key, float defaultValue = 0f);

        /// <summary>
        /// Gets a double value.
        /// </summary>
        double GetDouble(string key, double defaultValue = 0.0);

        /// <summary>
        /// Gets a boolean value.
        /// </summary>
        bool GetBool(string key, bool defaultValue = false);

        /// <summary>
        /// Gets all keys.
        /// </summary>
        IReadOnlyList<string> GetKeys();

        /// <summary>
        /// Checks if a key exists in the config.
        /// </summary>
        bool HasKey(string key);

        /// <summary>
        /// Sets default values to use when remote values are not available.
        /// </summary>
        void SetDefaults(IDictionary<string, object> defaults);
    }
}
