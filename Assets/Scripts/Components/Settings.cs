/**************************************************
 *  Settings.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    using System;
    using UnityEngine;
    using UnityEngine.Localization.Settings;

    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.UI;

    /// <summary>
    /// Defines the game settings.  Uses the <see cref="PlayerPrefs" /> class
    /// to persist the settings for multiple play sessions.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// The difficulty
        /// </summary>
        private static PlayerPrefsObjectProperty<Difficulty> difficulty = new PlayerPrefsObjectProperty<Difficulty>("difficulty", Difficulty.Default);

        /// <summary>
        /// The Custom Mode Settings selections (zero-based indexed)
        /// </summary>
        private static PlayerPrefsObjectProperty<CustomModeSettings> customModeSettings = new PlayerPrefsObjectProperty<CustomModeSettings>("customModeSettings", defaultValue: CustomModeSettings.Default);

        /// <summary>
        /// The survival mode
        /// </summary>
        private static PlayerPrefsBoolProperty survivalMode = new PlayerPrefsBoolProperty("survivalMode");

        /// <summary>
        /// The play sound
        /// </summary>
        private static PlayerPrefsBoolProperty playSound = new PlayerPrefsBoolProperty("playSound", defaultValue: true);

        /// <summary>
        /// The enable silverback muglumps setting
        /// </summary>
        private static PlayerPrefsBoolProperty enableSilverbackMuglumps = new PlayerPrefsBoolProperty("enableSilverbackMuglumps", defaultValue: true);

        /// <summary>
        /// The enable vibration setting
        /// </summary>
        private static PlayerPrefsBoolProperty enableVibration = new PlayerPrefsBoolProperty("enableVibration", defaultValue: true);

        private static PlayerPrefsIntProperty musicVolume = new PlayerPrefsIntProperty("musicVolume", defaultValue: AudioManager.MaxVolume);

        private static PlayerPrefsIntProperty soundEffectVolume = new PlayerPrefsIntProperty("soundEffectVolume", defaultValue: AudioManager.MaxVolume);

        private static PlayerPrefsObjectProperty<ScoreRecordHistory> scoreRecordHistory = new PlayerPrefsObjectProperty<ScoreRecordHistory>("scoreRecordHistory", defaultValue: new ScoreRecordHistory());

        private static PlayerPrefsStringProperty selectedLanguage = new PlayerPrefsStringProperty("selectedLanguage", defaultValue: string.Empty);

        private static PlayerPrefsIntProperty menuStyle = new PlayerPrefsIntProperty("menuStyle", defaultValue: 1);

        /// <summary>
        /// Gets or sets the difficulty.
        /// </summary>
        /// <value>
        /// The difficulty.
        /// </value>
        public static Difficulty Difficulty
        {
            get
            {
                if (difficulty.Get() == null)
                {
                    Difficulty = Difficulty.Default;
                }

                return difficulty.Get();
            }

            set
            {
                difficulty.Set(value);
            }
        }

        /// <summary>
        /// Gets or sets the Custom Mode Settings
        /// </summary>
        public static CustomModeSettings CustomModeSettings
        {
            get
            {
                if (customModeSettings.Get() == null)
                {
                    CustomModeSettings = CustomModeSettings.Default;
                }

                return customModeSettings.Get();
            }

            set
            {
                customModeSettings.Set(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [survival mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [survival mode]; otherwise, <c>false</c>.
        /// </value>
        public static bool SurvivalMode
        {
            get
            {
                return survivalMode.Get();
            }

            set
            {
                survivalMode.Set(value);
            }
        }

        public static bool EnableSilverbackMuglumps
        {
            get
            {
                return enableSilverbackMuglumps.Get();
            }

            set
            {
                enableSilverbackMuglumps.Set(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [play sound].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [play sound]; otherwise, <c>false</c>.
        /// </value>
        public static bool PlaySound
        {
            get
            {
                return playSound.Get();
            }

            set
            {
                playSound.Set(value);

                if (TitleScreenBehaviour.Instance != null)
                {
                    if (value && !TitleScreenBehaviour.Instance.MusicManager.IsPlaying)
                    {
                        TitleScreenBehaviour.Instance.MusicManager.Play();
                    }
                    else if (!value && TitleScreenBehaviour.Instance.MusicManager.IsPlaying)
                    {
                        TitleScreenBehaviour.Instance.MusicManager.Stop();
                    }
                }

                if (GameManager.Instance != null)
                {
                    if (value && !GameManager.Instance.MusicManager.IsPlaying)
                    {
                        GameManager.Instance.MusicManager.Play();
                    }
                    else if (!value && GameManager.Instance.MusicManager.IsPlaying)
                    {
                        GameManager.Instance.MusicManager.Stop();
                    }
                }
            }
        }

        public static int MusicVolume
        {
            get
            {
                return musicVolume.Get();
            }

            set
            {
                musicVolume.Set(value);

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.MusicManager.Volume = value;
                }

                if (TitleScreenBehaviour.Instance != null)
                {
                    TitleScreenBehaviour.Instance.MusicManager.Volume = value;
                }
            }
        }

        public static int SoundEffectVolume
        {
            get
            {
                return soundEffectVolume.Get();
            }

            set
            {
                soundEffectVolume.Set(value);

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SoundEffectManager.Volume = value;
                }

                if (TitleScreenBehaviour.Instance != null)
                {
                    TitleScreenBehaviour.Instance.SoundEffectManager.Volume = value;
                }
            }
        }

        public static bool EnableVibration
        {
            get
            {
                return false;
            }

            set
            {
                enableVibration.Set(value);
            }
        }

        public static ScoreRecordHistory ScoreRecordHistory
        {
            get
            {
                return scoreRecordHistory.Get();
            }

            set
            {
                scoreRecordHistory.Set(value);
            }
        }

        public static SupportedLanguage SelectedLanguage
        {
            get
            {
                return SupportedLanguage.SupportedLanguages.TryGetValue(selectedLanguage.Get(), out var language) ? language :
                    SupportedLanguage.SupportedLanguages.TryGetValue(LocalizationSettings.SelectedLocale.LocaleName, out var currentLanguage) ? currentLanguage :
                        SupportedLanguage.SupportedLanguages["en"];
            }

            set => selectedLanguage.Set(value.CultureCode);
        }

        public static MenuStyle MenuStyle
        {
            get => (MenuStyle)menuStyle.Get();
            set => menuStyle.Set((int)value);
        }

        /// <summary>
        /// Gets the dice roll used to determine if a gold muglump will be spawned
        /// </summary>
        /// <value>
        /// The gold muglump dice roll.
        /// </value>
        public static int GoldMuglumpDiceRoll { get; private set; } = 20;

        /// <summary>
        /// Gets the gold muglump critical range.
        /// </summary>
        /// <value>
        /// The gold muglump critical range.
        /// </value>
        public static int GoldMuglumpCriticalRange { get; set; } = 0;

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public static void SaveSettings()
        {
            customModeSettings.Save();
            scoreRecordHistory.Save();
            PlayerPrefsManager.SavePlayerPrefs();
        }
    }
}
