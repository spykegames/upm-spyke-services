using System.Collections.Generic;
using UnityEngine;

namespace Spyke.Services.Analytics
{
    /// <summary>
    /// Analytics service implementation that delegates to registered providers.
    /// </summary>
    public class AnalyticsService : IAnalyticsService
    {
        private readonly List<IAnalyticsProvider> _providers = new();
        private string _userId;

        public bool IsEnabled { get; set; } = true;

        public void RegisterProvider(IAnalyticsProvider provider)
        {
            if (provider != null && !_providers.Contains(provider))
            {
                _providers.Add(provider);

#if SPYKE_DEV
                Debug.Log($"[AnalyticsService] Registered provider: {provider.Name}");
#endif

                // Set user ID if already set
                if (!string.IsNullOrEmpty(_userId))
                {
                    provider.SetUserId(_userId);
                }
            }
        }

        public void UnregisterProvider(IAnalyticsProvider provider)
        {
            if (provider != null)
            {
                _providers.Remove(provider);

#if SPYKE_DEV
                Debug.Log($"[AnalyticsService] Unregistered provider: {provider.Name}");
#endif
            }
        }

        public void SetUserId(string userId)
        {
            _userId = userId;

            if (!IsEnabled) return;

            foreach (var provider in _providers)
            {
                if (provider.IsReady)
                {
                    provider.SetUserId(userId);
                }
            }
        }

        public void SetUserProperty(string name, string value)
        {
            if (!IsEnabled) return;

            foreach (var provider in _providers)
            {
                if (provider.IsReady)
                {
                    provider.SetUserProperty(name, value);
                }
            }
        }

        public void LogEvent(string eventName)
        {
            LogEvent(eventName, null);
        }

        public void LogEvent(string eventName, Dictionary<string, object> parameters)
        {
            if (!IsEnabled || string.IsNullOrEmpty(eventName)) return;

#if SPYKE_DEV
            Debug.Log($"[AnalyticsService] LogEvent: {eventName} | Params: {FormatParams(parameters)}");
#endif

            foreach (var provider in _providers)
            {
                if (provider.IsReady)
                {
                    provider.LogEvent(eventName, parameters);
                }
            }
        }

        public void LogEvent(IAnalyticsEventBuilder builder)
        {
            if (builder == null) return;
            LogEvent(builder.EventName, builder.Parameters);
        }

        public void LogScreenView(string screenName, string screenClass = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "screen_name", screenName }
            };

            if (!string.IsNullOrEmpty(screenClass))
            {
                parameters["screen_class"] = screenClass;
            }

            LogEvent("screen_view", parameters);
        }

        public void LogPurchase(string productId, double price, string currency, string transactionId = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "item_id", productId },
                { "value", price },
                { "currency", currency }
            };

            if (!string.IsNullOrEmpty(transactionId))
            {
                parameters["transaction_id"] = transactionId;
            }

            LogEvent("purchase", parameters);
        }

#if SPYKE_DEV
        private static string FormatParams(Dictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return "{}";
            }

            var parts = new List<string>();
            foreach (var kvp in parameters)
            {
                parts.Add($"{kvp.Key}={kvp.Value}");
            }
            return "{" + string.Join(", ", parts) + "}";
        }
#endif
    }
}
