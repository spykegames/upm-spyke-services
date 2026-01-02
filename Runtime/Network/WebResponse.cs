using System.Collections.Generic;

namespace Spyke.Services.Network
{
    /// <summary>
    /// Successful HTTP response.
    /// </summary>
    public class WebResponse
    {
        /// <summary>
        /// HTTP status code (e.g., 200, 201).
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Response body as string.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Response body as raw bytes.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Response headers.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Gets a header value by name.
        /// </summary>
        public string GetHeader(string name)
        {
            if (Headers == null) return null;
            return Headers.TryGetValue(name, out var value) ? value : null;
        }

        /// <summary>
        /// Whether the response indicates success (2xx status code).
        /// </summary>
        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;
    }
}
