using System.Collections.Generic;

namespace Spyke.Services.Analytics
{
    /// <summary>
    /// Analytics service interface for event tracking.
    /// </summary>
    public interface IAnalyticsService
    {
        /// <summary>
        /// Whether analytics is enabled.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Set the user ID for analytics tracking.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        void SetUserId(string userId);

        /// <summary>
        /// Set a user property.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        void SetUserProperty(string name, string value);

        /// <summary>
        /// Log an event with no parameters.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        void LogEvent(string eventName);

        /// <summary>
        /// Log an event with parameters.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="parameters">Event parameters.</param>
        void LogEvent(string eventName, Dictionary<string, object> parameters);

        /// <summary>
        /// Log an event using a builder.
        /// </summary>
        /// <param name="builder">Event builder.</param>
        void LogEvent(IAnalyticsEventBuilder builder);

        /// <summary>
        /// Log a screen view event.
        /// </summary>
        /// <param name="screenName">Screen name.</param>
        /// <param name="screenClass">Screen class (optional).</param>
        void LogScreenView(string screenName, string screenClass = null);

        /// <summary>
        /// Log a purchase/transaction event.
        /// </summary>
        /// <param name="productId">Product identifier.</param>
        /// <param name="price">Price amount.</param>
        /// <param name="currency">Currency code (e.g., "USD").</param>
        /// <param name="transactionId">Transaction ID (optional).</param>
        void LogPurchase(string productId, double price, string currency, string transactionId = null);

        /// <summary>
        /// Register an analytics provider.
        /// </summary>
        /// <param name="provider">Provider to register.</param>
        void RegisterProvider(IAnalyticsProvider provider);

        /// <summary>
        /// Unregister an analytics provider.
        /// </summary>
        /// <param name="provider">Provider to unregister.</param>
        void UnregisterProvider(IAnalyticsProvider provider);
    }
}
