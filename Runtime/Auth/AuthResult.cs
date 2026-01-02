namespace Spyke.Services.Auth
{
    /// <summary>
    /// Result of an authentication operation.
    /// </summary>
    public class AuthResult
    {
        /// <summary>
        /// Whether authentication was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Error message if authentication failed.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Error code from server (if applicable).
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// The credentials obtained from authentication.
        /// </summary>
        public AuthCredentials Credentials { get; set; }

        /// <summary>
        /// Whether this is a new user registration.
        /// </summary>
        public bool IsNewUser { get; set; }

        /// <summary>
        /// Whether the user was banned.
        /// </summary>
        public bool IsBanned { get; set; }

        /// <summary>
        /// Server response data (JSON string for game-specific parsing).
        /// </summary>
        public string ResponseData { get; set; }

        public static AuthResult Success(AuthCredentials credentials, bool isNewUser = false, string responseData = null)
        {
            return new AuthResult
            {
                IsSuccess = true,
                Credentials = credentials,
                IsNewUser = isNewUser,
                ResponseData = responseData
            };
        }

        public static AuthResult Failure(string errorMessage, int errorCode = 0)
        {
            return new AuthResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }

        public static AuthResult Banned(string message = "User is banned")
        {
            return new AuthResult
            {
                IsSuccess = false,
                IsBanned = true,
                ErrorMessage = message
            };
        }
    }
}
