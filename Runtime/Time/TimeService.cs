using System;
using Cysharp.Threading.Tasks;
using Spyke.Services.Network;
using UnityEngine;

namespace Spyke.Services.Time
{
    /// <summary>
    /// Server-synchronized time service implementation.
    /// </summary>
    public class TimeService : ITimeService
    {
        private const string OFFSET_KEY = "spyke_time_offset";
        private const string LAST_SYNC_KEY = "spyke_time_last_sync";
        private const int MIN_OFFSET_THRESHOLD_SECONDS = 5;

        private readonly IWebService _webService;
        private readonly string _timeEndpoint;

        private TimeSpan _offset = TimeSpan.Zero;
        private bool _isSynced;

        public DateTime Now => DateTime.Now + _offset;
        public DateTime UtcNow => DateTime.UtcNow + _offset;
        public long UnixTimeSeconds => new DateTimeOffset(UtcNow).ToUnixTimeSeconds();
        public long UnixTimeMilliseconds => new DateTimeOffset(UtcNow).ToUnixTimeMilliseconds();
        public bool IsSynced => _isSynced;
        public TimeSpan Offset => _offset;

        public event Action OnTimeSynced;

        /// <summary>
        /// Creates a TimeService with optional web service for active sync.
        /// </summary>
        /// <param name="webService">Web service for time sync requests (optional).</param>
        /// <param name="timeEndpoint">Endpoint for time sync (default: "time").</param>
        public TimeService(IWebService webService = null, string timeEndpoint = "time")
        {
            _webService = webService;
            _timeEndpoint = timeEndpoint;
            LoadOffset();
        }

        public async UniTask<bool> SyncAsync()
        {
            if (_webService == null)
            {
#if SPYKE_DEV
                Debug.LogWarning("[TimeService] No web service configured for active sync");
#endif
                return false;
            }

            try
            {
                var requestStart = DateTime.UtcNow;
                var request = new WebRequest(_timeEndpoint).Get();
                var response = await _webService.SendAsync(request);

                if (!response.IsSuccess)
                {
                    return false;
                }

                var requestDuration = DateTime.UtcNow - requestStart;

                // Try to get server time from Date header or response body
                var dateHeader = response.GetHeader("Date");
                if (!string.IsNullOrEmpty(dateHeader) && DateTime.TryParse(dateHeader, out var serverDate))
                {
                    UpdateFromServerDate(serverDate.ToUniversalTime(), requestDuration);
                    return true;
                }

                // Try JSON response with server_time field
                try
                {
                    var timeResponse = JsonUtility.FromJson<TimeResponse>(response.Text);
                    if (timeResponse.server_time > 0)
                    {
                        var serverDateTime = DateTimeOffset.FromUnixTimeSeconds(timeResponse.server_time).UtcDateTime;
                        UpdateFromServerDate(serverDateTime, requestDuration);
                        return true;
                    }
                }
                catch
                {
                    // Ignore JSON parse errors
                }

#if SPYKE_DEV
                Debug.LogWarning("[TimeService] Could not parse server time from response");
#endif
                return false;
            }
            catch (Exception ex)
            {
#if SPYKE_DEV
                Debug.LogError($"[TimeService] Sync failed: {ex.Message}");
#endif
                return false;
            }
        }

        public void UpdateFromServerDate(DateTime serverDate, TimeSpan requestDuration)
        {
            // Ignore if request took too long (inaccurate)
            if (requestDuration.TotalSeconds > MIN_OFFSET_THRESHOLD_SECONDS)
            {
#if SPYKE_DEV
                Debug.Log($"[TimeService] Ignoring sync - request too slow ({requestDuration.TotalSeconds}s)");
#endif
                return;
            }

            // Calculate offset accounting for half the request duration
            var estimatedServerNow = serverDate + TimeSpan.FromMilliseconds(requestDuration.TotalMilliseconds / 2);
            var localNow = DateTime.UtcNow;
            var newOffset = estimatedServerNow - localNow;

            // Only update if difference is significant
            var offsetDiff = Math.Abs((newOffset - _offset).TotalSeconds);
            if (offsetDiff > MIN_OFFSET_THRESHOLD_SECONDS)
            {
                SetOffset(newOffset);

#if SPYKE_DEV
                Debug.Log($"[TimeService] Time synced. Offset: {_offset.TotalSeconds}s");
#endif
            }
            else if (!_isSynced)
            {
                _isSynced = true;
                SaveOffset();
                OnTimeSynced?.Invoke();
            }
        }

        public void SetOffset(TimeSpan offset)
        {
            _offset = offset;
            _isSynced = true;
            SaveOffset();
            OnTimeSynced?.Invoke();
        }

        public void LoadOffset()
        {
            var offsetSeconds = PlayerPrefs.GetFloat(OFFSET_KEY, 0);
            if (Math.Abs(offsetSeconds) > 0.001f)
            {
                _offset = TimeSpan.FromSeconds(offsetSeconds);
                _isSynced = true;

#if SPYKE_DEV
                Debug.Log($"[TimeService] Loaded offset: {_offset.TotalSeconds}s");
#endif
            }
        }

        public void Reset()
        {
            _offset = TimeSpan.Zero;
            _isSynced = false;
            PlayerPrefs.DeleteKey(OFFSET_KEY);
            PlayerPrefs.DeleteKey(LAST_SYNC_KEY);
            PlayerPrefs.Save();

#if SPYKE_DEV
            Debug.Log("[TimeService] Reset to local time");
#endif
        }

        private void SaveOffset()
        {
            PlayerPrefs.SetFloat(OFFSET_KEY, (float)_offset.TotalSeconds);
            PlayerPrefs.SetString(LAST_SYNC_KEY, DateTime.UtcNow.ToString("O"));
            PlayerPrefs.Save();
        }

        [Serializable]
        private class TimeResponse
        {
            public long server_time;
        }
    }
}
