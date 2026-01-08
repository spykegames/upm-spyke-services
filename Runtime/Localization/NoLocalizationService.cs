using System;
using System.Collections.Generic;

namespace Spyke.Services.Localization
{
    /// <summary>
    /// No-op implementation of ILocalizationService.
    /// Returns keys as-is. Use for testing or when localization is disabled.
    /// </summary>
    public class NoLocalizationService : ILocalizationService
    {
        private static readonly List<string> EmptyLanguages = new() { "en" };

        public string CurrentLanguage => "en";
        public IReadOnlyList<string> AvailableLanguages => EmptyLanguages;
        public bool IsInitialized => true;

        public event Action<string> OnLanguageChanged;

        public string GetText(string key)
        {
            return key ?? string.Empty;
        }

        public string GetText(string key, params object[] args)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            try
            {
                return string.Format(key, args);
            }
            catch
            {
                return key;
            }
        }

        public bool HasKey(string key)
        {
            return false;
        }

        public bool SetLanguage(string languageCode)
        {
            return false;
        }

        public string GetLanguageDisplayName(string languageCode)
        {
            return languageCode;
        }

        // Suppress unused event warning
        protected virtual void OnLanguageChangedInternal(string language)
        {
            OnLanguageChanged?.Invoke(language);
        }
    }
}
