using System;
using Cysharp.Threading.Tasks;

namespace Spyke.Services.Push
{
    /// <summary>
    /// Service interface for push notification handling.
    /// Implemented by platform-specific providers (OneSignal, Firebase, etc.).
    /// </summary>
    public interface IPushNotificationService
    {
        /// <summary>
        /// Current push notification permission status.
        /// </summary>
        PushNotificationStatus Status { get; }

        /// <summary>
        /// Whether push notifications are currently enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// The device push token (may be null if not yet available).
        /// </summary>
        string PushToken { get; }

        /// <summary>
        /// The user/subscription ID from the push provider.
        /// </summary>
        string PushUserId { get; }

        /// <summary>
        /// Invoked when permission status changes.
        /// </summary>
        event Action<PushNotificationStatus> OnPermissionChanged;

        /// <summary>
        /// Invoked when push token is updated.
        /// Parameters: (userId, token)
        /// </summary>
        event Action<string, string> OnPushTokenUpdated;

        /// <summary>
        /// Invoked when a notification is clicked/opened.
        /// </summary>
        event Action<PushNotificationData> OnNotificationClicked;

        /// <summary>
        /// Invoked when a notification is received while app is in foreground.
        /// </summary>
        event Action<PushNotificationData> OnNotificationReceived;

        /// <summary>
        /// Requests push notification permission from the user.
        /// </summary>
        /// <returns>True if permission was granted.</returns>
        UniTask<bool> RequestPermissionAsync();

        /// <summary>
        /// Sets the external user ID for targeting.
        /// </summary>
        void SetUserId(string userId);

        /// <summary>
        /// Adds a tag for segmentation.
        /// </summary>
        void SetTag(string key, string value);

        /// <summary>
        /// Removes a tag.
        /// </summary>
        void RemoveTag(string key);

        /// <summary>
        /// Opens the device's notification settings.
        /// </summary>
        void OpenNotificationSettings();
    }
}
