namespace Spyke.Services.Auth
{
    /// <summary>
    /// Result from Apple Sign In authentication.
    /// </summary>
    public class AppleAuthResult
    {
        /// <summary>
        /// Whether authentication was successful.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Apple user identifier.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Identity token (JWT) for backend authentication.
        /// </summary>
        public string IdentityToken { get; }

        /// <summary>
        /// Authorization code for token exchange.
        /// </summary>
        public string AuthorizationCode { get; }

        /// <summary>
        /// User's email (only provided on first sign-in).
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// User's given name (only provided on first sign-in).
        /// </summary>
        public string GivenName { get; }

        /// <summary>
        /// User's family name (only provided on first sign-in).
        /// </summary>
        public string FamilyName { get; }

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
        public AppleAuthError Error { get; }

        private AppleAuthResult(
            bool success,
            string userId,
            string identityToken,
            string authorizationCode,
            string email,
            string givenName,
            string familyName,
            bool cancelled,
            string errorMessage,
            AppleAuthError error)
        {
            Success = success;
            UserId = userId;
            IdentityToken = identityToken;
            AuthorizationCode = authorizationCode;
            Email = email;
            GivenName = givenName;
            FamilyName = familyName;
            Cancelled = cancelled;
            ErrorMessage = errorMessage;
            Error = error;
        }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        public static AppleAuthResult Successful(
            string userId,
            string identityToken,
            string authorizationCode = null,
            string email = null,
            string givenName = null,
            string familyName = null)
        {
            return new AppleAuthResult(
                true, userId, identityToken, authorizationCode,
                email, givenName, familyName,
                false, null, AppleAuthError.None);
        }

        /// <summary>
        /// Creates a cancelled result.
        /// </summary>
        public static AppleAuthResult UserCancelled()
        {
            return new AppleAuthResult(
                false, null, null, null, null, null, null,
                true, "User cancelled", AppleAuthError.Cancelled);
        }

        /// <summary>
        /// Creates a failed result.
        /// </summary>
        public static AppleAuthResult Failed(string errorMessage, AppleAuthError error = AppleAuthError.Unknown)
        {
            return new AppleAuthResult(
                false, null, null, null, null, null, null,
                false, errorMessage, error);
        }
    }

    /// <summary>
    /// Apple authentication error types.
    /// </summary>
    public enum AppleAuthError
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
        /// Platform not supported (non-iOS/macOS).
        /// </summary>
        PlatformNotSupported,

        /// <summary>
        /// Sign In with Apple not available.
        /// </summary>
        NotAvailable,

        /// <summary>
        /// Login failed with error.
        /// </summary>
        LoginFailed,

        /// <summary>
        /// Credential state check failed.
        /// </summary>
        CredentialCheckFailed,

        /// <summary>
        /// Credentials were revoked.
        /// </summary>
        CredentialsRevoked,

        /// <summary>
        /// Timeout waiting for response.
        /// </summary>
        Timeout,

        /// <summary>
        /// Unknown error.
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Apple credential state.
    /// </summary>
    public enum AppleCredentialState
    {
        /// <summary>
        /// Credential not found.
        /// </summary>
        NotFound = 0,

        /// <summary>
        /// Credential is authorized and valid.
        /// </summary>
        Authorized,

        /// <summary>
        /// Credential was revoked by user.
        /// </summary>
        Revoked,

        /// <summary>
        /// User transferred to a different Apple ID.
        /// </summary>
        Transferred
    }
}
