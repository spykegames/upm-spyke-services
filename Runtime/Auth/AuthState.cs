namespace Spyke.Services.Auth
{
    /// <summary>
    /// Authentication state.
    /// </summary>
    public enum AuthState
    {
        /// <summary>
        /// Not authenticated, no credentials.
        /// </summary>
        NotAuthenticated,

        /// <summary>
        /// Authentication in progress.
        /// </summary>
        Authenticating,

        /// <summary>
        /// Successfully authenticated.
        /// </summary>
        Authenticated,

        /// <summary>
        /// Authentication failed.
        /// </summary>
        Failed,

        /// <summary>
        /// User is banned.
        /// </summary>
        Banned
    }
}
