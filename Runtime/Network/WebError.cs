namespace Spyke.Services.Network
{
    /// <summary>
    /// HTTP request error/failure.
    /// </summary>
    public class WebError
    {
        /// <summary>
        /// HTTP status code (0 if network error).
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Error message/reason.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Response body as string (if available).
        /// </summary>
        public string ResponseText { get; set; }

        /// <summary>
        /// Response body as raw bytes (if available).
        /// </summary>
        public byte[] ResponseData { get; set; }

        /// <summary>
        /// Whether this is a network/connection error (vs server error).
        /// </summary>
        public bool IsNetworkError { get; set; }

        /// <summary>
        /// Whether this is a timeout error.
        /// </summary>
        public bool IsTimeout { get; set; }

        public override string ToString()
        {
            return $"[WebError] {StatusCode}: {Message}";
        }
    }
}
