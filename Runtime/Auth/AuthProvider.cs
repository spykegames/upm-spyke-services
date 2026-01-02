namespace Spyke.Services.Auth
{
    /// <summary>
    /// Authentication provider types.
    /// </summary>
    public enum AuthProvider
    {
        /// <summary>
        /// Anonymous guest authentication using device ID.
        /// </summary>
        Guest,

        /// <summary>
        /// Facebook Login authentication.
        /// </summary>
        Facebook,

        /// <summary>
        /// Sign in with Apple authentication.
        /// </summary>
        Apple,

        /// <summary>
        /// Google Play Games authentication.
        /// </summary>
        GooglePlay
    }
}
