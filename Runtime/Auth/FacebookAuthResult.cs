namespace Spyke.Services.Auth
{
    /// <summary>
    /// Result from Facebook authentication.
    /// </summary>
    public class FacebookAuthResult
    {
        /// <summary>
        /// Whether authentication was successful.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Facebook access token (null if failed).
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Facebook user ID.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Whether this is a limited login (iOS App Tracking Transparency).
        /// </summary>
        public bool IsLimitedLogin { get; }

        /// <summary>
        /// Whether the user cancelled the login.
        /// </summary>
        public bool Cancelled { get; }

        /// <summary>
        /// Error message if authentication failed.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Error code/type if authentication failed.
        /// </summary>
        public FacebookAuthError Error { get; }

        private FacebookAuthResult(
            bool success,
            string accessToken,
            string userId,
            bool isLimitedLogin,
            bool cancelled,
            string errorMessage,
            FacebookAuthError error)
        {
            Success = success;
            AccessToken = accessToken;
            UserId = userId;
            IsLimitedLogin = isLimitedLogin;
            Cancelled = cancelled;
            ErrorMessage = errorMessage;
            Error = error;
        }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        public static FacebookAuthResult Successful(string accessToken, string userId, bool isLimitedLogin = false)
        {
            return new FacebookAuthResult(true, accessToken, userId, isLimitedLogin, false, null, FacebookAuthError.None);
        }

        /// <summary>
        /// Creates a cancelled result.
        /// </summary>
        public static FacebookAuthResult UserCancelled()
        {
            return new FacebookAuthResult(false, null, null, false, true, "User cancelled", FacebookAuthError.Cancelled);
        }

        /// <summary>
        /// Creates a failed result.
        /// </summary>
        public static FacebookAuthResult Failed(string errorMessage, FacebookAuthError error = FacebookAuthError.Unknown)
        {
            return new FacebookAuthResult(false, null, null, false, false, errorMessage, error);
        }
    }

    /// <summary>
    /// Facebook authentication error types.
    /// </summary>
    public enum FacebookAuthError
    {
        /// <summary>
        /// No error.
        /// </summary>
        None = 0,

        /// <summary>
        /// User cancelled the login.
        /// </summary>
        Cancelled,

        /// <summary>
        /// SDK initialization failed.
        /// </summary>
        SdkInitializationFailed,

        /// <summary>
        /// Login failed with error.
        /// </summary>
        LoginFailed,

        /// <summary>
        /// Access token refresh failed.
        /// </summary>
        TokenRefreshFailed,

        /// <summary>
        /// Unknown error.
        /// </summary>
        Unknown
    }
}
