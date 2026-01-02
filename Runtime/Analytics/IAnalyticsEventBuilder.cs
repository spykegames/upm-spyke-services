using System.Collections.Generic;

namespace Spyke.Services.Analytics
{
    /// <summary>
    /// Fluent builder for analytics events.
    /// </summary>
    public interface IAnalyticsEventBuilder
    {
        /// <summary>
        /// Event name.
        /// </summary>
        string EventName { get; }

        /// <summary>
        /// Event parameters.
        /// </summary>
        Dictionary<string, object> Parameters { get; }
    }
}
