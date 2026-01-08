using System;
using Cysharp.Threading.Tasks;

namespace Spyke.Services.Auth
{
    /// <summary>
    /// Interface for Apple Sign In authentication.
    /// Implemented by platform-specific providers in upm-spyke-sdks.
    /// </summary>
    public interface IAppleAuthProvider
    {
        /// <summary>
        /// Whether Sign In with Apple is supported on current platform.
        /// </summary>
        bool IsSupported { get; }

        /// <summary>
        /// Whether the user is currently signed in.
        /// </summary>
        bool IsSignedIn { get; }

        /// <summary>
        /// Current Apple user ID (null if not signed in).
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// Current identity token (null if not signed in or expired).
        /// </summary>
        string IdentityToken { get; }

        /// <summary>
        /// Current authorization code (null if not available).
        /// </summary>
        string AuthorizationCode { get; }

        /// <summary>
        /// Current credential state.
        /// </summary>
        AppleCredentialState CredentialState { get; }

        /// <summary>
        /// Invoked when credentials are revoked by user.
        /// </summary>
        event Action OnCredentialsRevoked;

        /// <summary>
        /// Sign in with Apple.
        /// Will show native sign-in UI.
        /// </summary>
        /// <param name="requestEmail">Whether to request email (only works on first sign-in).</param>
        /// <param name="requestFullName">Whether to request full name (only works on first sign-in).</param>
        UniTask<AppleAuthResult> SignInAsync(bool requestEmail = true, bool requestFullName = true);

        /// <summary>
        /// Check the current credential state for a user.
        /// </summary>
        /// <param name="userId">Apple user ID to check.</param>
        UniTask<AppleCredentialState> GetCredentialStateAsync(string userId);

        /// <summary>
        /// Sign out (clears local state only, does not revoke Apple credentials).
        /// </summary>
        void SignOut();
    }
}
