using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Spyke.Services.Auth
{
    /// <summary>
    /// Authentication service interface.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Current authentication state.
        /// </summary>
        AuthState State { get; }

        /// <summary>
        /// Current user credentials (null if not authenticated).
        /// </summary>
        AuthCredentials Credentials { get; }

        /// <summary>
        /// Whether the user is currently authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Event fired when authentication state changes.
        /// </summary>
        event Action<AuthState> OnStateChanged;

        /// <summary>
        /// Event fired when authentication succeeds.
        /// </summary>
        event Action<AuthResult> OnAuthenticated;

        /// <summary>
        /// Event fired when authentication fails.
        /// </summary>
        event Action<AuthResult> OnAuthFailed;

        /// <summary>
        /// Authenticate as guest using device ID.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        UniTask<AuthResult> LoginAsGuestAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Authenticate with Facebook.
        /// </summary>
        /// <param name="accessToken">Facebook access token.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        UniTask<AuthResult> LoginWithFacebookAsync(string accessToken, CancellationToken cancellationToken = default);

        /// <summary>
        /// Authenticate with Apple.
        /// </summary>
        /// <param name="identityToken">Apple identity token.</param>
        /// <param name="authorizationCode">Apple authorization code.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        UniTask<AuthResult> LoginWithAppleAsync(string identityToken, string authorizationCode = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Authenticate with Google Play Games.
        /// </summary>
        /// <param name="serverAuthCode">Google server auth code.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        UniTask<AuthResult> LoginWithGooglePlayAsync(string serverAuthCode, CancellationToken cancellationToken = default);

        /// <summary>
        /// Refresh the current access token.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        UniTask<AuthResult> RefreshTokenAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Log out and clear credentials.
        /// </summary>
        void Logout();

        /// <summary>
        /// Load saved credentials from storage.
        /// </summary>
        /// <returns>True if credentials were loaded.</returns>
        bool LoadCredentials();

        /// <summary>
        /// Save current credentials to storage.
        /// </summary>
        void SaveCredentials();

        /// <summary>
        /// Clear saved credentials from storage.
        /// </summary>
        void ClearCredentials();
    }
}
