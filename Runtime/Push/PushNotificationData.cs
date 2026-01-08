using System.Collections.Generic;

namespace Spyke.Services.Push
{
    /// <summary>
    /// Data received from a push notification.
    /// </summary>
    public class PushNotificationData
    {
        /// <summary>
        /// Unique notification identifier.
        /// </summary>
        public string NotificationId { get; }

        /// <summary>
        /// Notification title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Notification body/message.
        /// </summary>
        public string Body { get; }

        /// <summary>
        /// Optional subtitle (iOS).
        /// </summary>
        public string Subtitle { get; }

        /// <summary>
        /// Template ID from the push provider (if applicable).
        /// </summary>
        public string TemplateId { get; }

        /// <summary>
        /// Template name from the push provider (if applicable).
        /// </summary>
        public string TemplateName { get; }

        /// <summary>
        /// Additional custom data payload.
        /// </summary>
        public IReadOnlyDictionary<string, object> AdditionalData { get; }

        public PushNotificationData(
            string notificationId,
            string title,
            string body,
            string subtitle = null,
            string templateId = null,
            string templateName = null,
            IReadOnlyDictionary<string, object> additionalData = null)
        {
            NotificationId = notificationId ?? string.Empty;
            Title = title ?? string.Empty;
            Body = body ?? string.Empty;
            Subtitle = subtitle ?? string.Empty;
            TemplateId = templateId ?? string.Empty;
            TemplateName = templateName ?? string.Empty;
            AdditionalData = additionalData ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// Tries to get a string value from additional data.
        /// </summary>
        public bool TryGetAdditionalData(string key, out string value)
        {
            if (AdditionalData.TryGetValue(key, out var obj) && obj is string str)
            {
                value = str;
                return true;
            }

            value = null;
            return false;
        }
    }
}
