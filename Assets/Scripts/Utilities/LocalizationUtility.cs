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
    using UnityEngine.Localization.Tables;
    using UnityEngine.ResourceManagement.AsyncOperations;

    /// <summary>
    /// Uses a directly bundled English table on WebGL and Unity Localization on
    /// platforms where its Addressables lifecycle is supported by this project.
    /// </summary>
    public static class LocalizationUtility
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        private const string WebGLEnglishTableResource = "LocalizedStringConstantsEnglish";

        private static StringTable webGLEnglishTable;
#endif

        public static string GetLocalizedString(
            string tableName,
            string entryKey,
            Locale locale = null,
            FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings,
            params object[] arguments)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            if (tableName != StringContent.StringContentTable)
            {
                Debug.LogWarning($"No WebGL English table is registered for '{tableName}'. Using key '{entryKey}'.");
                return entryKey;
            }

            if (webGLEnglishTable == null)
            {
                webGLEnglishTable = Resources.Load<StringTable>(WebGLEnglishTableResource);
            }

            if (webGLEnglishTable == null)
            {
                Debug.LogError($"WebGL English string table resource was not found. Using key '{entryKey}'.");
                return entryKey;
            }

            var entry = webGLEnglishTable.GetEntry(entryKey);

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
