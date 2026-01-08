using System;
using UnityEngine;

namespace Spyke.Services.Crashlytics
{
    /// <summary>
    /// No-op implementation of ICrashlyticsService.
    /// Logs to Unity console in development. Use for testing or when Crashlytics is disabled.
    /// </summary>
    public class NoCrashlyticsService : ICrashlyticsService
    {
        public bool IsEnabled => false;

        public void SetUserId(string userId)
        {
#if SPYKE_DEV
            Debug.Log($"[NoCrashlyticsService] SetUserId: {userId}");
#endif
        }

        public void Log(string message)
        {
#if SPYKE_DEV
            Debug.Log($"[NoCrashlyticsService] Log: {message}");
#endif
        }

        public void RecordException(Exception exception)
        {
#if SPYKE_DEV
            Debug.LogException(exception);
#endif
        }

        public void RecordException(string message, Exception exception = null)
        {
#if SPYKE_DEV
            if (exception != null)
            {
                Debug.LogError($"[NoCrashlyticsService] {message}: {exception}");
            }
            else
            {
                Debug.LogError($"[NoCrashlyticsService] {message}");
            }
#endif
        }

        public void SetCustomKey(string key, string value)
        {
#if SPYKE_DEV
            Debug.Log($"[NoCrashlyticsService] SetCustomKey: {key} = {value}");
#endif
        }

        public void SetCustomKey(string key, int value)
        {
#if SPYKE_DEV
            Debug.Log($"[NoCrashlyticsService] SetCustomKey: {key} = {value}");
#endif
        }

        public void SetCustomKey(string key, bool value)
        {
#if SPYKE_DEV
            Debug.Log($"[NoCrashlyticsService] SetCustomKey: {key} = {value}");
#endif
        }

        public void ForceCrash()
        {
#if SPYKE_DEV
            Debug.LogError("[NoCrashlyticsService] ForceCrash called (no-op)");
#endif
        }
    }
}
