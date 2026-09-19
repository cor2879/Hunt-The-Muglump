/**************************************************
 *  LocalizationUtility.cs
 *
 *  copyright (c) 2026 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Utilities
{
    using UnityEngine;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;
    using UnityEngine.ResourceManagement.AsyncOperations;

    /// <summary>
    /// Provides localized strings without invoking Addressables.WaitForCompletion in WebGL.
    /// LocaleManager asynchronously preloads the tables before gameplay can begin.
    /// </summary>
    public static class LocalizationUtility
    {
        public static string GetLocalizedString(
            string tableName,
            string entryKey,
            Locale locale = null,
            FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings,
            params object[] arguments)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            if (!LocalizationSettings.InitializationOperation.IsDone)
            {
                Debug.LogError($"Localization was requested before initialization completed: {tableName}/{entryKey}.");
                return entryKey;
            }

            locale = locale ?? LocalizationSettings.SelectedLocaleAsync.Result;

            var tableOperation = LocalizationSettings.StringDatabase.GetTableAsync(tableName, locale);

            if (!tableOperation.IsDone ||
                tableOperation.Status != AsyncOperationStatus.Succeeded ||
                tableOperation.Result == null)
            {
                Debug.LogError($"Localization table is not ready: {tableName} ({locale?.Identifier.Code ?? "no locale"}).");
                return entryKey;
            }

            var entry = tableOperation.Result.GetEntry(entryKey);

            if (entry == null)
            {
                Debug.LogError($"Localization entry was not found: {tableName}/{entryKey}.");
                return entryKey;
            }

            return entry.GetLocalizedString(arguments);
#else
            return LocalizationSettings.StringDatabase.GetLocalizedString(
                tableName,
                entryKey,
                locale,
                fallbackBehavior,
                arguments);
#endif
        }
    }
}
