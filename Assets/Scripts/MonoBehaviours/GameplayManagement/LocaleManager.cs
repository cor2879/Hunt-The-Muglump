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
        }

        private IEnumerator Start()
        {
            var initializationOperation = LocalizationSettings.InitializationOperation;
            yield return initializationOperation;

            if (initializationOperation.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogException(
                    initializationOperation.OperationException ??
                    new InvalidOperationException("Localization initialization failed."));
                yield break;
            }

            yield return this.ApplyLocaleAsync(Settings.SelectedLanguage);

            if (this.IsReady)
            {
                Debug.Log($"Localization initialized asynchronously for {this.CurrentLocale}.");
            }
        }

        private void Update()
        {
            if (!this.IsReady || this.isChangingLocale)
            {
                return;
            }

            var language = Settings.SelectedLanguage;

            if (!string.Equals(this.appliedCultureCode, language.CultureCode, StringComparison.OrdinalIgnoreCase))
            {
                StartCoroutine(this.ApplyLocaleAsync(language));
            }
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

            LocalizationSettings.SelectedLocale = locale;

            // Load each table asynchronously so WebGL-safe cached lookups are available before
            // gameplay begins. WebGL cannot call Addressables.WaitForCompletion at any point.
            foreach (string tableName in StringTables)
            {
                var tableOperation = LocalizationSettings.StringDatabase.GetTableAsync(tableName, locale);
                yield return tableOperation;

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
