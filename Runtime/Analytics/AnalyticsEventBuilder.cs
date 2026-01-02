using System.Collections.Generic;

namespace Spyke.Services.Analytics
{
    /// <summary>
    /// Fluent builder for creating analytics events.
    /// </summary>
    public class AnalyticsEventBuilder : IAnalyticsEventBuilder
    {
        public string EventName { get; private set; }
        public Dictionary<string, object> Parameters { get; } = new();

        public AnalyticsEventBuilder(string eventName)
        {
            EventName = eventName;
        }

        /// <summary>
        /// Add a string parameter.
        /// </summary>
        public AnalyticsEventBuilder AddParam(string key, string value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                Parameters[key] = value;
            }
            return this;
        }

        /// <summary>
        /// Add an integer parameter.
        /// </summary>
        public AnalyticsEventBuilder AddParam(string key, int value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                Parameters[key] = value;
            }
            return this;
        }

        /// <summary>
        /// Add a long parameter.
        /// </summary>
        public AnalyticsEventBuilder AddParam(string key, long value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                Parameters[key] = value;
            }
            return this;
        }

        /// <summary>
        /// Add a double parameter.
        /// </summary>
        public AnalyticsEventBuilder AddParam(string key, double value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                Parameters[key] = value;
            }
            return this;
        }

        /// <summary>
        /// Add a boolean parameter.
        /// </summary>
        public AnalyticsEventBuilder AddParam(string key, bool value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                Parameters[key] = value;
            }
            return this;
        }

        /// <summary>
        /// Create a new event builder.
        /// </summary>
        public static AnalyticsEventBuilder Create(string eventName)
        {
            return new AnalyticsEventBuilder(eventName);
        }
    }
}
