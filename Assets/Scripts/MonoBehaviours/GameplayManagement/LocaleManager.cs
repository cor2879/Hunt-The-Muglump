#pragma warning disable CS0649
/**************************************************
 *  LocaleManager.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.GameplayManagement
{
    using System;
    using System.Collections;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.UI;
    
    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Platform;
    using OldSchoolGames.HuntTheMuglump.Scripts.Rules;
    using OldSchoolGames.HuntTheMuglump.Scripts.UI;
    using OldSchoolGames.HuntTheMuglump.Scripts.UI.GameplayPrompts;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;
    using UnityEngine.SocialPlatforms;

    public class LocaleManager : MonoBehaviour
    {
        private static readonly string[] StringTables =
        {
            "GameOverLocalizationTable",
            StringContent.StringContentTable,
            "MenuPanelsLocalizationTable"
        };

        private static LocaleManager instance;

        private bool isChangingLocale;

        private string appliedCultureCode;

        [SerializeField, ReadOnly]
        private string currentLocale;

        [SerializeField, ReadOnly]
        private string selectedLanguage;

        public string CurrentLocale
        {
            get => this.currentLocale;
            private set => this.currentLocale = value;
        }

        public string SelectedLanguage
        {
            get => this.selectedLanguage;
            private set => this.selectedLanguage = value;
        }

        public static LocaleManager Instance 
        { 
            get => instance; 
            private set => instance = value; 
        }

        public bool IsReady { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;

            // These values are safe to initialize without touching Addressables.
            this.CurrentLocale = Settings.SelectedLanguage.CultureCode;
            this.SelectedLanguage = Settings.SelectedLanguage.Name;

#if UNITY_WEBGL && !UNITY_EDITOR
            // WebGL uses a bundled English StringTable directly. Do not initialize the
            // Addressables-backed Localization runtime on this platform.
            this.IsReady = true;
#endif
        }

        private IEnumerator Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            yield break;
#else
            var initializationOperation = LocalizationSettings.InitializationOperation;
            yield return initializationOperation;

            if (!initializationOperation.IsValid())
            {
                Debug.LogError("Localization initialization returned an invalid operation handle.");
                yield break;
            }

            if (initializationOperation.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogException(
                    initializationOperation.OperationException ??
                    new InvalidOperationException("Localization initialization failed."));
                yield break;
            }

            // GameObjectLocalizer.Start caches SelectedLocaleAsync while initialization is in
            // progress. Let those coroutines consume the completed handle before a locale change
            // can replace and release it.
            yield return null;

            yield return this.ApplyLocaleAsync(Settings.SelectedLanguage);

            if (this.IsReady)
            {
                Debug.Log($"Localization initialized asynchronously for {this.CurrentLocale}.");
            }
#endif
        }

        private void Update()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return;
#else
            if (!this.IsReady || this.isChangingLocale)
            {
                return;
            }

            var language = Settings.SelectedLanguage;

            if (!string.Equals(this.appliedCultureCode, language.CultureCode, StringComparison.OrdinalIgnoreCase))
            {
                StartCoroutine(this.ApplyLocaleAsync(language));
            }
#endif
        }

        private IEnumerator ApplyLocaleAsync(SupportedLanguage language)
        {
            this.isChangingLocale = true;
            this.IsReady = false;

            var locale = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(language.CultureCode)) ??
                LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier("en"));

            if (locale == null)
            {
                Debug.LogError($"No Localization locale is available for '{language.CultureCode}' or the English fallback.");
                this.isChangingLocale = false;
                yield break;
            }

            var selectedLocaleOperation = LocalizationSettings.SelectedLocaleAsync;
            var currentLocale = selectedLocaleOperation.IsValid() && selectedLocaleOperation.IsDone
                ? selectedLocaleOperation.Result
                : null;

            if (!ReferenceEquals(currentLocale, locale))
            {
                LocalizationSettings.SelectedLocale = locale;
            }

            // Load each table asynchronously so WebGL-safe cached lookups are available before
            // gameplay begins. WebGL cannot call Addressables.WaitForCompletion at any point.
            foreach (string tableName in StringTables)
            {
                var tableOperation = LocalizationSettings.StringDatabase.GetTableAsync(tableName, locale);
                yield return tableOperation;

                if (!tableOperation.IsValid())
                {
                    Debug.LogError($"Localization table '{tableName}' returned an invalid operation handle.");
                    continue;
                }

                if (tableOperation.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogException(
                        tableOperation.OperationException ??
                        new InvalidOperationException($"Failed to preload localization table '{tableName}'."));
                }
            }

            this.appliedCultureCode = language.CultureCode;
            this.CurrentLocale = locale.LocaleName;
            this.SelectedLanguage = language.Name;
            this.isChangingLocale = false;
            this.IsReady = true;
        }
    }
}
