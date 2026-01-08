namespace Spyke.Services.Push
{
    /// <summary>
    /// Push notification permission status.
    /// Maps to platform-native permission states.
    /// </summary>
    public enum PushNotificationStatus
    {
        /// <summary>
        /// User has not yet been prompted for permission.
        /// </summary>
        NotDetermined = 0,

        /// <summary>
        /// User explicitly denied push notification permission.
        /// </summary>
        Denied,

        /// <summary>
        /// User granted full push notification permission.
        /// </summary>
        Authorized,

        /// <summary>
        /// iOS only: User granted provisional permission (quiet notifications).
        /// </summary>
        Provisional,

        /// <summary>
        /// iOS only: App Clip ephemeral notifications.
        /// </summary>
        Ephemeral
    }
}
