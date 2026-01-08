using System;
using Cysharp.Threading.Tasks;

namespace Spyke.Services.Auth
{
    /// <summary>
    /// No-op implementation of IAppleAuthProvider.
    /// Use for testing, editor, or non-Apple platforms.
    /// </summary>
    public class NoAppleAuthProvider : IAppleAuthProvider
    {
        public bool IsSupported => false;
        public bool IsSignedIn => false;
        public string UserId => null;
        public string IdentityToken => null;
        public string AuthorizationCode => null;
        public AppleCredentialState CredentialState => AppleCredentialState.NotFound;

        public event Action OnCredentialsRevoked;

        public UniTask<AppleAuthResult> SignInAsync(bool requestEmail = true, bool requestFullName = true)
        {
            return UniTask.FromResult(
                AppleAuthResult.Failed("Sign In with Apple not available", AppleAuthError.PlatformNotSupported));
        }

        public UniTask<AppleCredentialState> GetCredentialStateAsync(string userId)
        {
            return UniTask.FromResult(AppleCredentialState.NotFound);
        }

        public void SignOut()
        {
            // No-op
        }

        // Suppress unused event warning
        protected virtual void OnCredentialsRevokedInternal()
        {
            OnCredentialsRevoked?.Invoke();
        }
    }
}
