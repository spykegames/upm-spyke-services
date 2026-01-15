using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Spyke.Services.Network
{
    /// <summary>
    /// HTTP web service implementation using UnityWebRequest.
    /// </summary>
    public class WebService : IWebService
    {
        private readonly Dictionary<string, string> _defaultHeaders = new();
        private string _baseUrl = string.Empty;

        public void SetBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl?.TrimEnd('/') ?? string.Empty;
        }

        public void SetDefaultHeader(string name, string value)
        {
            _defaultHeaders[name] = value;
        }

        public void RemoveDefaultHeader(string name)
        {
            _defaultHeaders.Remove(name);
        }

        public async UniTask<WebResponse> SendAsync(WebRequest request, CancellationToken cancellationToken = default)
        {
            var builtRequest = request.Build();
            var url = BuildUrl(builtRequest.Url);

            using var unityRequest = CreateUnityRequest(builtRequest, url);

            ApplyHeaders(unityRequest, builtRequest.Headers);
            ApplyBody(unityRequest, builtRequest);

#if SPYKE_DEV
            Debug.Log($"[WebService] {builtRequest.Method} {url}");
#endif

            try
            {
                await unityRequest.SendWebRequest().WithCancellation(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new WebServiceException(CreateError(unityRequest, ex.Message));
            }

            if (unityRequest.result != UnityWebRequest.Result.Success)
            {
                throw new WebServiceException(CreateError(unityRequest));
            }

            return CreateResponse(unityRequest);
        }

        public async UniTask<T> SendAsync<T>(WebRequest request, CancellationToken cancellationToken = default)
        {
            var response = await SendAsync(request, cancellationToken);
            return JsonUtility.FromJson<T>(response.Text);
        }

        public async UniTask SendAsync(WebRequest request, Action<WebResponse> onSuccess, Action<WebError> onError, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await SendAsync(request, cancellationToken);
                onSuccess?.Invoke(response);
            }
            catch (WebServiceException ex)
            {
                onError?.Invoke(ex.Error);
            }
            catch (Exception ex)
            {
                onError?.Invoke(new WebError
                {
                    StatusCode = 0,
                    Message = ex.Message,
                    IsNetworkError = true
                });
            }
        }

        public async UniTask SendAsync<T>(WebRequest request, Action<T> onSuccess, Action<WebError> onError, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await SendAsync<T>(request, cancellationToken);
                onSuccess?.Invoke(response);
            }
            catch (WebServiceException ex)
            {
                onError?.Invoke(ex.Error);
            }
            catch (Exception ex)
            {
                onError?.Invoke(new WebError
                {
                    StatusCode = 0,
                    Message = ex.Message,
                    IsNetworkError = true
                });
            }
        }

        private string BuildUrl(string url)
        {
            if (string.IsNullOrEmpty(_baseUrl) || url.StartsWith("http://") || url.StartsWith("https://"))
            {
                return url;
            }

            return $"{_baseUrl}/{url.TrimStart('/')}";
        }

        private UnityWebRequest CreateUnityRequest(WebRequest request, string url)
        {
            var unityRequest = request.Method switch
            {
                HttpMethod.GET => UnityWebRequest.Get(url),
                HttpMethod.POST => new UnityWebRequest(url, "POST"),
                HttpMethod.PUT => new UnityWebRequest(url, "PUT"),
                HttpMethod.DELETE => UnityWebRequest.Delete(url),
                HttpMethod.PATCH => new UnityWebRequest(url, "PATCH"),
                _ => UnityWebRequest.Get(url)
            };

            unityRequest.timeout = request.TimeoutSeconds;
            unityRequest.downloadHandler = new DownloadHandlerBuffer();

            return unityRequest;
        }

        private void ApplyHeaders(UnityWebRequest unityRequest, Dictionary<string, string> headers)
        {
            // Apply default headers
            foreach (var header in _defaultHeaders)
            {
                unityRequest.SetRequestHeader(header.Key, header.Value);
            }

            // Apply request-specific headers (override defaults)
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    unityRequest.SetRequestHeader(header.Key, header.Value);
                }
            }
        }

        private void ApplyBody(UnityWebRequest unityRequest, WebRequest request)
        {
            if (!string.IsNullOrEmpty(request.StringBody))
            {
                var bodyBytes = Encoding.UTF8.GetBytes(request.StringBody);
                unityRequest.uploadHandler = new UploadHandlerRaw(bodyBytes);
                unityRequest.SetRequestHeader("Content-Type", "application/json");
            }
            else if (request.RawBody != null && request.RawBody.Length > 0)
            {
                unityRequest.uploadHandler = new UploadHandlerRaw(request.RawBody);
            }
        }

        private WebResponse CreateResponse(UnityWebRequest unityRequest)
        {
            var response = new WebResponse
            {
                StatusCode = (int)unityRequest.responseCode,
                Text = unityRequest.downloadHandler?.text,
                Data = unityRequest.downloadHandler?.data,
                Headers = new Dictionary<string, string>()
            };

            var responseHeaders = unityRequest.GetResponseHeaders();
            if (responseHeaders != null)
            {
                foreach (var header in responseHeaders)
                {
                    response.Headers[header.Key] = header.Value;
                }
            }

#if SPYKE_DEV
            Debug.Log($"[WebService] Response {response.StatusCode}: {response.Text?.Substring(0, Math.Min(200, response.Text?.Length ?? 0))}");
#endif

            return response;
        }

        private WebError CreateError(UnityWebRequest unityRequest, string additionalMessage = null)
        {
            var isNetworkError = unityRequest.result == UnityWebRequest.Result.ConnectionError;
            var isTimeout = unityRequest.error?.Contains("timeout", StringComparison.OrdinalIgnoreCase) ?? false;

            var error = new WebError
            {
                StatusCode = (int)unityRequest.responseCode,
                Message = additionalMessage ?? unityRequest.error ?? "Unknown error",
                ResponseText = unityRequest.downloadHandler?.text,
                ResponseData = unityRequest.downloadHandler?.data,
                IsNetworkError = isNetworkError,
                IsTimeout = isTimeout
            };

#if SPYKE_DEV
            Debug.LogError($"[WebService] Error: {error}");
#endif

            return error;
        }
    }

    /// <summary>
    /// Exception thrown when a web request fails.
    /// </summary>
    public class WebServiceException : Exception
    {
        public WebError Error { get; }

        public WebServiceException(WebError error) : base(error.Message)
        {
            Error = error;
        }
    }
}
