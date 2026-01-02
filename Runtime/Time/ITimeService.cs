using System;
using Cysharp.Threading.Tasks;

namespace Spyke.Services.Time
{
    /// <summary>
    /// Service for server-synchronized time.
    /// </summary>
    public interface ITimeService
    {
        /// <summary>
        /// Current server-synchronized time (local time + offset).
        /// Falls back to local time if not synced.
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Current server-synchronized UTC time.
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// Unix timestamp in seconds.
        /// </summary>
        long UnixTimeSeconds { get; }

        /// <summary>
        /// Unix timestamp in milliseconds.
        /// </summary>
        long UnixTimeMilliseconds { get; }

        /// <summary>
        /// Whether time has been synchronized with server.
        /// </summary>
        bool IsSynced { get; }

        /// <summary>
        /// Current offset between local and server time.
        /// </summary>
        TimeSpan Offset { get; }

        /// <summary>
        /// Event fired when time is synchronized.
        /// </summary>
        event Action OnTimeSynced;

        /// <summary>
        /// Synchronize time with server.
        /// </summary>
        /// <returns>True if sync successful.</returns>
        UniTask<bool> SyncAsync();

        /// <summary>
        /// Update offset from server response header.
        /// Call this when receiving any HTTP response with Date header.
        /// </summary>
        /// <param name="serverDate">Server date from response header.</param>
        /// <param name="requestDuration">Time taken for the request (for accuracy).</param>
        void UpdateFromServerDate(DateTime serverDate, TimeSpan requestDuration);

        /// <summary>
        /// Manually set the offset (for testing or recovery).
        /// </summary>
        /// <param name="offset">Offset to set.</param>
        void SetOffset(TimeSpan offset);

        /// <summary>
        /// Load persisted offset from storage.
        /// </summary>
        void LoadOffset();

        /// <summary>
        /// Clear synced offset and reset to local time.
        /// </summary>
        void Reset();
    }
}
