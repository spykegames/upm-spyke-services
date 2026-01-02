using System.Collections.Generic;

namespace Spyke.Services.Analytics
{
    /// <summary>
    /// Analytics provider interface (e.g., Firebase, AppsFlyer, custom).
    /// Implement this to add new analytics backends.
    /// </summary>
    public interface IAnalyticsProvider
    {
        /// <summary>
        /// Provider name for identification.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Whether this provider is initialized and ready.
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// Set the user ID.
        /// </summary>
        void SetUserId(string userId);

        /// <summary>
        /// Set a user property.
        /// </summary>
        void SetUserProperty(string name, string value);

        /// <summary>
        /// Log an event with parameters.
        /// </summary>
        void LogEvent(string eventName, Dictionary<string, object> parameters);
    }
}
