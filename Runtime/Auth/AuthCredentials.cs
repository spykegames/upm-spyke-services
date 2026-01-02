using System;

namespace Spyke.Services.Auth
{
    /// <summary>
    /// User authentication credentials.
    /// </summary>
    [Serializable]
    public class AuthCredentials
    {
        /// <summary>
        /// Unique device identifier.
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Authentication provider used.
        /// </summary>
        public AuthProvider Provider { get; set; }

        /// <summary>
        /// Provider-specific ID token (e.g., Facebook token, Apple identity token).
        /// </summary>
        public string IdToken { get; set; }

        /// <summary>
        /// Server-issued access token after successful authentication.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Server-issued refresh token for token renewal.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Token expiration time in UTC.
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// Whether the access token is expired.
        /// </summary>
        public bool IsExpired => ExpiresAt.HasValue && DateTime.UtcNow >= ExpiresAt.Value;

        /// <summary>
        /// Whether credentials are valid for authentication.
        /// </summary>
        public bool IsValid => !string.IsNullOrEmpty(DeviceId) && !string.IsNullOrEmpty(AccessToken) && !IsExpired;
    }
}
