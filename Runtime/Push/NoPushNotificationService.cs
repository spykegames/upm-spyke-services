using System;
using Cysharp.Threading.Tasks;

namespace Spyke.Services.Push
{
    /// <summary>
    /// No-op implementation of IPushNotificationService.
    /// Use for testing, editor, or when push is disabled.
    /// </summary>
    public class NoPushNotificationService : IPushNotificationService
    {
        public PushNotificationStatus Status => PushNotificationStatus.NotDetermined;
        public bool IsEnabled => false;
        public string PushToken => null;
        public string PushUserId => null;

        public event Action<PushNotificationStatus> OnPermissionChanged;
        public event Action<string, string> OnPushTokenUpdated;
        public event Action<PushNotificationData> OnNotificationClicked;
        public event Action<PushNotificationData> OnNotificationReceived;

        public UniTask<bool> RequestPermissionAsync()
        {
            return UniTask.FromResult(false);
        }

        public void SetUserId(string userId)
        {
            // No-op
        }

        public void SetTag(string key, string value)
        {
            // No-op
        }

        public void RemoveTag(string key)
        {
            // No-op
        }

        public void OpenNotificationSettings()
        {
            // No-op
        }

        // Suppress unused event warnings
        protected virtual void OnPermissionChangedInternal(PushNotificationStatus status)
        {
            OnPermissionChanged?.Invoke(status);
        }

        protected virtual void OnPushTokenUpdatedInternal(string userId, string token)
        {
            OnPushTokenUpdated?.Invoke(userId, token);
        }

        protected virtual void OnNotificationClickedInternal(PushNotificationData data)
        {
            OnNotificationClicked?.Invoke(data);
        }

        protected virtual void OnNotificationReceivedInternal(PushNotificationData data)
        {
            OnNotificationReceived?.Invoke(data);
        }
    }
}
