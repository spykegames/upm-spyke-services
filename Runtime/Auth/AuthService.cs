using System;
using Cysharp.Threading.Tasks;
using Spyke.Services.Network;
using UnityEngine;

namespace Spyke.Services.Auth
{
    /// <summary>
    /// Authentication service implementation.
    /// </summary>
    public class AuthService : IAuthService
    {
        private const string CREDENTIALS_KEY = "spyke_auth_credentials";
        private const string DEVICE_ID_KEY = "spyke_device_id";

        private readonly IWebService _webService;
        private readonly string _authEndpoint;

        public AuthState State { get; private set; } = AuthState.NotAuthenticated;
        public AuthCredentials Credentials { get; private set; }
        public bool IsAuthenticated => State == AuthState.Authenticated && Credentials?.IsValid == true;

        public event Action<AuthState> OnStateChanged;
        public event Action<AuthResult> OnAuthenticated;
        public event Action<AuthResult> OnAuthFailed;

        public AuthService(IWebService webService, string authEndpoint = "auth")
        {
            _webService = webService;
            _authEndpoint = authEndpoint;
        }

        public async UniTask<AuthResult> LoginAsGuestAsync()
        {
            return await AuthenticateAsync(AuthProvider.Guest, null);
        }

        public async UniTask<AuthResult> LoginWithFacebookAsync(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                return AuthResult.Failure("Facebook access token is required");
            }
            return await AuthenticateAsync(AuthProvider.Facebook, accessToken);
        }

        public async UniTask<AuthResult> LoginWithAppleAsync(string identityToken, string authorizationCode = null)
        {
            if (string.IsNullOrEmpty(identityToken))
            {
                return AuthResult.Failure("Apple identity token is required");
            }
            return await AuthenticateAsync(AuthProvider.Apple, identityToken);
        }

        public async UniTask<AuthResult> LoginWithGooglePlayAsync(string serverAuthCode)
        {
            if (string.IsNullOrEmpty(serverAuthCode))
            {
                return AuthResult.Failure("Google server auth code is required");
            }
            return await AuthenticateAsync(AuthProvider.GooglePlay, serverAuthCode);
        }

        public async UniTask<AuthResult> RefreshTokenAsync()
        {
            if (Credentials == null || string.IsNullOrEmpty(Credentials.RefreshToken))
            {
                return AuthResult.Failure("No refresh token available");
            }

            SetState(AuthState.Authenticating);

            try
            {
                var request = new WebRequest($"{_authEndpoint}/refresh")
                    .Post()
                    .SetBody(JsonUtility.ToJson(new RefreshRequest { refresh_token = Credentials.RefreshToken }));

                var response = await _webService.SendAsync(request);

                if (!response.IsSuccess)
                {
                    return HandleFailure("Token refresh failed");
                }

                var authResponse = JsonUtility.FromJson<AuthApiResponse>(response.Text);
                return HandleSuccess(authResponse, Credentials.Provider);
            }
            catch (WebServiceException ex)
            {
                return HandleFailure(ex.Error.Message, ex.Error.StatusCode);
            }
            catch (Exception ex)
            {
                return HandleFailure(ex.Message);
            }
        }

        public void Logout()
        {
            Credentials = null;
            SetState(AuthState.NotAuthenticated);
            ClearCredentials();

#if SPYKE_DEV
            Debug.Log("[AuthService] Logged out");
#endif
        }

        public bool LoadCredentials()
        {
            try
            {
                var json = PlayerPrefs.GetString(CREDENTIALS_KEY, null);
                if (string.IsNullOrEmpty(json))
                {
                    return false;
                }

                Credentials = JsonUtility.FromJson<AuthCredentials>(json);
                if (Credentials?.IsValid == true)
                {
                    SetState(AuthState.Authenticated);
                    return true;
                }

                Credentials = null;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void SaveCredentials()
        {
            if (Credentials == null) return;

            try
            {
                var json = JsonUtility.ToJson(Credentials);
                PlayerPrefs.SetString(CREDENTIALS_KEY, json);
                PlayerPrefs.Save();
            }
            catch (Exception ex)
            {
#if SPYKE_DEV
                Debug.LogError($"[AuthService] Failed to save credentials: {ex.Message}");
#endif
            }
        }

        public void ClearCredentials()
        {
            PlayerPrefs.DeleteKey(CREDENTIALS_KEY);
            PlayerPrefs.Save();
        }

        private async UniTask<AuthResult> AuthenticateAsync(AuthProvider provider, string idToken)
        {
            SetState(AuthState.Authenticating);

#if SPYKE_DEV
            Debug.Log($"[AuthService] Authenticating with {provider}");
#endif

            try
            {
                var deviceId = GetOrCreateDeviceId();
                var requestBody = new AuthApiRequest
                {
                    device_id = deviceId,
                    provider = provider.ToString().ToLower(),
                    id_token = idToken
                };

                var request = new WebRequest(_authEndpoint)
                    .Post()
                    .SetBody(JsonUtility.ToJson(requestBody));

                var response = await _webService.SendAsync(request);

                if (!response.IsSuccess)
                {
                    return HandleFailure($"Authentication failed with status {response.StatusCode}");
                }

                var authResponse = JsonUtility.FromJson<AuthApiResponse>(response.Text);
                return HandleSuccess(authResponse, provider, response.Text);
            }
            catch (WebServiceException ex)
            {
                if (ex.Error.StatusCode == 403)
                {
                    SetState(AuthState.Banned);
                    var result = AuthResult.Banned();
                    OnAuthFailed?.Invoke(result);
                    return result;
                }
                return HandleFailure(ex.Error.Message, ex.Error.StatusCode);
            }
            catch (Exception ex)
            {
                return HandleFailure(ex.Message);
            }
        }

        private AuthResult HandleSuccess(AuthApiResponse response, AuthProvider provider, string rawResponse = null)
        {
            Credentials = new AuthCredentials
            {
                DeviceId = GetOrCreateDeviceId(),
                Provider = provider,
                AccessToken = response.access_token,
                RefreshToken = response.refresh_token,
                IdToken = response.id_token,
                ExpiresAt = response.expires_in > 0
                    ? DateTime.UtcNow.AddSeconds(response.expires_in)
                    : null
            };

            SetState(AuthState.Authenticated);
            SaveCredentials();

            var result = AuthResult.Success(Credentials, response.is_new_user, rawResponse);
            OnAuthenticated?.Invoke(result);

#if SPYKE_DEV
            Debug.Log($"[AuthService] Authentication successful. New user: {response.is_new_user}");
#endif

            return result;
        }

        private AuthResult HandleFailure(string message, int errorCode = 0)
        {
            SetState(AuthState.Failed);
            var result = AuthResult.Failure(message, errorCode);
            OnAuthFailed?.Invoke(result);

#if SPYKE_DEV
            Debug.LogError($"[AuthService] Authentication failed: {message}");
#endif

            return result;
        }

        private void SetState(AuthState newState)
        {
            if (State == newState) return;
            State = newState;
            OnStateChanged?.Invoke(newState);
        }

        private string GetOrCreateDeviceId()
        {
            var deviceId = PlayerPrefs.GetString(DEVICE_ID_KEY, null);
            if (string.IsNullOrEmpty(deviceId))
            {
                deviceId = SystemInfo.deviceUniqueIdentifier;
                if (string.IsNullOrEmpty(deviceId) || deviceId == "n/a")
                {
                    deviceId = Guid.NewGuid().ToString();
                }
                PlayerPrefs.SetString(DEVICE_ID_KEY, deviceId);
                PlayerPrefs.Save();
            }
            return deviceId;
        }

        // Internal request/response DTOs for JSON serialization
        [Serializable]
        private class AuthApiRequest
        {
            public string device_id;
            public string provider;
            public string id_token;
        }

        [Serializable]
        private class AuthApiResponse
        {
            public string access_token;
            public string refresh_token;
            public string id_token;
            public int expires_in;
            public bool is_new_user;
        }

        [Serializable]
        private class RefreshRequest
        {
            public string refresh_token;
        }
    }
}
