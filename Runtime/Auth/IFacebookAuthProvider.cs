using System;
using Cysharp.Threading.Tasks;

namespace Spyke.Services.Auth
{
    /// <summary>
    /// Interface for Facebook SDK authentication.
    /// Implemented by platform-specific providers in upm-spyke-sdks.
    /// </summary>
    public interface IFacebookAuthProvider
    {
        /// <summary>
        /// Whether the Facebook SDK is initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Whether the user is currently logged in.
        /// </summary>
        bool IsLoggedIn { get; }

        /// <summary>
        /// Whether this is a limited login (iOS ATT).
        /// </summary>
        bool IsLimitedLogin { get; }

        /// <summary>
        /// Current Facebook access token (null if not logged in).
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        /// Current Facebook user ID (null if not logged in).
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// Initialize the Facebook SDK.
        /// </summary>
        /// <param name="appId">Optional app ID (uses settings if not provided).</param>
        /// <param name="clientToken">Optional client token.</param>
        UniTask<bool> InitializeAsync(string appId = null, string clientToken = null);

        /// <summary>
        /// Authenticate with Facebook.
        /// </summary>
        UniTask<FacebookAuthResult> LoginAsync();

        /// <summary>
        /// Authenticate with extended friend permissions.
        /// </summary>
        UniTask<FacebookAuthResult> LoginWithFriendsPermissionAsync();

        /// <summary>
        /// Refresh the current access token.
        /// </summary>
        UniTask<FacebookAuthResult> RefreshTokenAsync();

        /// <summary>
        /// Log out from Facebook.
        /// </summary>
        void Logout();

        /// <summary>
        /// Check if the user has granted friend list permission.
        /// </summary>
        bool HasFriendsPermission { get; }

        /// <summary>
        /// Get the profile picture URL for a user.
        /// </summary>
        /// <param name="userId">Facebook user ID.</param>
        /// <param name="width">Picture width in pixels.</param>
        /// <param name="height">Picture height in pixels.</param>
        string GetProfilePictureUrl(string userId, int width, int height);

        /// <summary>
        /// Set advertiser tracking enabled (for iOS ATT compliance).
        /// </summary>
        void SetAdvertiserTrackingEnabled(bool enabled);
    }
}
