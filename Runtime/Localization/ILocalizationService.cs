using System;
using System.Collections.Generic;

namespace Spyke.Services.Localization
{
    /// <summary>
    /// Service interface for localization.
    /// Implemented by platform-specific providers (I2 Localization, Unity Localization, etc.).
    /// </summary>
    public interface ILocalizationService
    {
        /// <summary>
        /// Current language code (e.g., "en", "tr", "de").
        /// </summary>
        string CurrentLanguage { get; }

        /// <summary>
        /// List of available language codes.
        /// </summary>
        IReadOnlyList<string> AvailableLanguages { get; }

        /// <summary>
        /// Whether the localization system is initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Event fired when language changes.
        /// </summary>
        event Action<string> OnLanguageChanged;

        /// <summary>
        /// Gets a localized string by key.
        /// </summary>
        /// <param name="key">The localization key.</param>
        /// <returns>The localized string, or the key if not found.</returns>
        string GetText(string key);

        /// <summary>
        /// Gets a localized string with format parameters.
        /// </summary>
        /// <param name="key">The localization key.</param>
        /// <param name="args">Format arguments.</param>
        /// <returns>The formatted localized string.</returns>
        string GetText(string key, params object[] args);

        /// <summary>
        /// Checks if a localization key exists.
        /// </summary>
        /// <param name="key">The localization key.</param>
        /// <returns>True if the key exists.</returns>
        bool HasKey(string key);

        /// <summary>
        /// Sets the current language.
        /// </summary>
        /// <param name="languageCode">The language code to set.</param>
        /// <returns>True if the language was set successfully.</returns>
        bool SetLanguage(string languageCode);

        /// <summary>
        /// Gets the display name for a language code.
        /// </summary>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The display name (e.g., "English", "Türkçe").</returns>
        string GetLanguageDisplayName(string languageCode);
    }
}
