using System;

namespace Spyke.Services.Crashlytics
{
    /// <summary>
    /// Service interface for crash reporting.
    /// Implemented by platform-specific providers (Firebase Crashlytics, etc.).
    /// </summary>
    public interface ICrashlyticsService
    {
        /// <summary>
        /// Whether crash reporting is initialized and enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Sets the user identifier for crash reports.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        void SetUserId(string userId);

        /// <summary>
        /// Logs a message to be included with crash reports.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Log(string message);

        /// <summary>
        /// Records a non-fatal exception.
        /// </summary>
        /// <param name="exception">The exception to record.</param>
        void RecordException(Exception exception);

        /// <summary>
        /// Records a non-fatal exception with a custom message.
        /// </summary>
        /// <param name="message">Custom message describing the error.</param>
        /// <param name="exception">The exception to record (optional).</param>
        void RecordException(string message, Exception exception = null);

        /// <summary>
        /// Sets a custom key-value pair for crash reports.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The string value.</param>
        void SetCustomKey(string key, string value);

        /// <summary>
        /// Sets a custom key-value pair for crash reports.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The integer value.</param>
        void SetCustomKey(string key, int value);

        /// <summary>
        /// Sets a custom key-value pair for crash reports.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The boolean value.</param>
        void SetCustomKey(string key, bool value);

        /// <summary>
        /// Forces a crash for testing purposes.
        /// Only use in development builds.
        /// </summary>
        void ForceCrash();
    }
}
