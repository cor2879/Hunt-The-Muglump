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

    using UnityEngine;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;
    using UnityEngine.ResourceManagement.AsyncOperations;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class LocaleManager : MonoBehaviour
    {
        private static LocaleManager instance;

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

        }

        private IEnumerator Start()
        {
            // Initialization remains asynchronous on WebGL. Gameplay string lookups use the
            // bundled English table there, but runtime-instantiated UI prefabs can still contain
            // GameObjectLocalizer components that require a valid AvailableLocales provider.
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

            this.ApplyLocale(Settings.SelectedLanguage);

            if (this.IsReady)
            {
                Debug.Log($"Localization initialized asynchronously for {this.CurrentLocale}.");
            }
        }

        private void Update()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return;
#else
            if (!this.IsReady)
            {
                return;
            }

            var language = Settings.SelectedLanguage;

            if (!string.Equals(this.appliedCultureCode, language.CultureCode, StringComparison.OrdinalIgnoreCase))
            {
                this.ApplyLocale(language);
            }
#endif
        }

        private void ApplyLocale(SupportedLanguage language)
        {
            this.IsReady = false;

            var locale = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(language.CultureCode)) ??
                LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier("en"));

            if (locale == null)
            {
                Debug.LogError($"No Localization locale is available for '{language.CultureCode}' or the English fallback.");
                return;
            }

            var selectedLocaleOperation = LocalizationSettings.SelectedLocaleAsync;
            var currentLocale = selectedLocaleOperation.IsValid() && selectedLocaleOperation.IsDone
                ? selectedLocaleOperation.Result
                : null;

            if (!ReferenceEquals(currentLocale, locale))
            {
                LocalizationSettings.SelectedLocale = locale;
            }

            this.appliedCultureCode = language.CultureCode;
            this.CurrentLocale = locale.LocaleName;
            this.SelectedLanguage = language.Name;
            this.IsReady = true;
        }
    }
}
