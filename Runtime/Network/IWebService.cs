using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Spyke.Services.Network
{
    /// <summary>
    /// Interface for HTTP web requests.
    /// </summary>
    public interface IWebService
    {
        /// <summary>
        /// Sends an HTTP request and returns the response.
        /// </summary>
        /// <param name="request">The request to send.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>WebResponse on success.</returns>
        UniTask<WebResponse> SendAsync(WebRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends an HTTP request with typed response deserialization.
        /// </summary>
        /// <typeparam name="T">Response type to deserialize to.</typeparam>
        /// <param name="request">The request to send.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Deserialized response.</returns>
        UniTask<T> SendAsync<T>(WebRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends an HTTP request with error callback instead of throwing.
        /// </summary>
        /// <param name="request">The request to send.</param>
        /// <param name="onSuccess">Called on success with response.</param>
        /// <param name="onError">Called on error with error details.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        UniTask SendAsync(WebRequest request, Action<WebResponse> onSuccess, Action<WebError> onError, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends an HTTP request with typed response and error callback.
        /// </summary>
        /// <typeparam name="T">Response type to deserialize to.</typeparam>
        /// <param name="request">The request to send.</param>
        /// <param name="onSuccess">Called on success with deserialized response.</param>
        /// <param name="onError">Called on error with error details.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        UniTask SendAsync<T>(WebRequest request, Action<T> onSuccess, Action<WebError> onError, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets a default header for all requests.
        /// </summary>
        /// <param name="name">Header name.</param>
        /// <param name="value">Header value.</param>
        void SetDefaultHeader(string name, string value);

        /// <summary>
        /// Removes a default header.
        /// </summary>
        /// <param name="name">Header name to remove.</param>
        void RemoveDefaultHeader(string name);

        /// <summary>
        /// Sets the base URL for all requests.
        /// </summary>
        /// <param name="baseUrl">Base URL (e.g., "https://api.example.com").</param>
        void SetBaseUrl(string baseUrl);
    }
}
