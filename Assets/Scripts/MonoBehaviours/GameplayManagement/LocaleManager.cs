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
        private static LocaleManager instance;

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

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }

            if (LocalizationSettings.SelectedLocale == null || string.Equals(LocalizationSettings.SelectedLocale.LocaleName, "None", StringComparison.OrdinalIgnoreCase))
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)) ?? 
                    LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier("en"));
            }
        }

        private void Update()
        {
            this.CurrentLocale = LocalizationSettings.SelectedLocale.LocaleName;

            this.SelectedLanguage = Settings.SelectedLanguage.Name;

            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode));
        }
    }
}
