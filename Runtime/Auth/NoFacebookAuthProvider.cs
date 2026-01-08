using Cysharp.Threading.Tasks;

namespace Spyke.Services.Auth
{
    /// <summary>
    /// No-op implementation of IFacebookAuthProvider.
    /// Use for testing, editor, or when Facebook is not available.
    /// </summary>
    public class NoFacebookAuthProvider : IFacebookAuthProvider
    {
        public bool IsInitialized => false;
        public bool IsLoggedIn => false;
        public bool IsLimitedLogin => false;
        public string AccessToken => null;
        public string UserId => null;
        public bool HasFriendsPermission => false;

        public UniTask<bool> InitializeAsync(string appId = null, string clientToken = null)
        {
            return UniTask.FromResult(false);
        }

        public UniTask<FacebookAuthResult> LoginAsync()
        {
            return UniTask.FromResult(
                FacebookAuthResult.Failed("Facebook SDK not available", FacebookAuthError.SdkInitializationFailed));
        }

        public UniTask<FacebookAuthResult> LoginWithFriendsPermissionAsync()
        {
            return UniTask.FromResult(
                FacebookAuthResult.Failed("Facebook SDK not available", FacebookAuthError.SdkInitializationFailed));
        }

        public UniTask<FacebookAuthResult> RefreshTokenAsync()
        {
            return UniTask.FromResult(
                FacebookAuthResult.Failed("Facebook SDK not available", FacebookAuthError.SdkInitializationFailed));
        }

        public void Logout()
        {
            // No-op
        }

        public string GetProfilePictureUrl(string userId, int width, int height)
        {
            return null;
        }

        public void SetAdvertiserTrackingEnabled(bool enabled)
        {
            // No-op
        }
    }
}
