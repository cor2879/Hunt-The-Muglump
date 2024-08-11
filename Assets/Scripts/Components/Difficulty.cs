/**************************************************
 *  Difficulty.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;

    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines a range of values use for describing the possible difficulty settings.
    /// </summary>
    public enum DifficultySetting
    {
        /*
        /// <summary>
        /// The tutorial setting
        /// </summary>
        Tutorial = 0,
        */
        
        /// <summary>
        /// The beginner setting
        /// </summary>
        Beginner = 0,

        /// <summary>
        /// The easy setting
        /// </summary>
        Easy,

        /// <summary>
        /// The normal setting
        /// </summary>
        Normal,

        /// <summary>
        /// The hard setting
        /// </summary>
        Hard,

        /// <summary>
        /// The expert setting
        /// </summary>
        Expert,

        /// <summary>
        /// The Hunter and Prey setting
        /// </summary>
        HunterAndPrey,

        /// <summary>
        /// The custom setting
        /// </summary>
        Custom
#if DEBUG
        /// <summary>
        /// The black muglump insanity setting
        /// </summary>
        , BlackMuglumpInsanity

        /// <summary>
        /// The black muglump test setting
        /// </summary>
        , BlackMuglumpTest

        /// <summary>
        /// The blue muglump test
        /// </summary>
        , BlueMuglumpTest

        /// <summary>
        /// The empty test
        /// </summary>
        , Empty

        /// <summary>
        /// Lots of bats
        /// </summary>
        , LotsOfBats
#endif
    };

    /// <summary>
    /// Defines a structure for specifying the parameters used to build a dungeon
    /// </summary>
    public class Difficulty
    {
        public const string CustomModePlayerPrefsKey = "CustomMode";

        /// <summary>
        /// The pre-defined difficulty settings.
        /// </summary>
        private static Dictionary<DifficultySetting, Difficulty> difficulties = GetDifficultiesInternal();

        public static readonly Difficulty Default = difficulties[DifficultySetting.Beginner];

        private static Dictionary<DifficultySetting, Difficulty> GetDifficultiesInternal()
        {
            var difficulties = new Dictionary<DifficultySetting, Difficulty>()
            {
                {
                    DifficultySetting.Beginner,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.Beginner,
                        DisplayName = "Beginner",
                        MinimumRoomCount = 33,
                        MuglumpCount = 10,
                        BlackMuglumpCount = 0,
                        BlueMuglumpCount = 0,
                        SilverbackMuglumpCount = 0,
                        BatCount = 1,
                        PitCount = 1,
                        ArrowCount = 20,
                        FlashArrowCount = 1,
                        NetArrowCount = 1,
                        BearTrapCount = 0,
                        EnableBossMode = false
                    }
                },
                {
                    DifficultySetting.Easy,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.Easy,
                        DisplayName = "Easy",
                        MinimumRoomCount = 69,
                        MuglumpCount = 40,
                        BlackMuglumpCount = 5,
                        BlueMuglumpCount = 0,
                        BatCount = 5,
                        PitCount = 5,
                        ArrowCount = 35,
                        FlashArrowCount = 15,
                        NetArrowCount = 10,
                        EnableBossMode = false
                    }
                },
                {
                    DifficultySetting.Normal,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.Normal,
                        DisplayName = "Normal",
                        MinimumRoomCount = 144,
                        MuglumpCount = 70,
                        BlackMuglumpCount = 15,
                        BlueMuglumpCount = 10,
                        SilverbackMuglumpCount = 1,
                        BatCount = 10,
                        PitCount = 10,
                        ArrowCount = 80,
                        FlashArrowCount = 30,
                        NetArrowCount = 10,
                        BearTrapCount = 10,
                        CoverScentCount = 4,
                        EnableBossMode = true
                    }
                },
                {
                    DifficultySetting.Hard,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.Hard,
                        DisplayName = "Hard",
                        MinimumRoomCount = 369,
                        MuglumpCount = 144,
                        BlackMuglumpCount = 111,
                        BlueMuglumpCount = 30,
                        SilverbackMuglumpCount = 10,
                        BatCount = 15,
                        PitCount = 14,
                        ArrowCount = 350,
                        FlashArrowCount = 80,
                        NetArrowCount = 35,
                        CoverScentCount = 7,
                        BearTrapCount = 15,
                        EnableBossMode = true
                    }
                },
                {
                    DifficultySetting.Expert,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.Expert,
                        DisplayName = "Expert",
                        MinimumRoomCount = 555,
                        MuglumpCount = 222,
                        BlackMuglumpCount = 88,
                        BlueMuglumpCount = 63,
                        SilverbackMuglumpCount = 22,
                        BatCount = 55,
                        PitCount = 55,
                        ArrowCount = 222,
                        FlashArrowCount = 144,
                        NetArrowCount = 88,
                        CoverScentCount = 25,
                        BearTrapCount = 50,
                        EnableBossMode = true
                    }
                },
                {
                    DifficultySetting.HunterAndPrey,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.HunterAndPrey,
                        DisplayName = "HunterAndPrey",
                        MinimumRoomCount = 639,
                        MuglumpCount = 0,
                        BlackMuglumpCount = 0,
                        BlueMuglumpCount = 222,
                        SilverbackMuglumpCount = 111,
                        BatCount = 22,
                        PitCount = 22,
                        ArrowCount = 369,
                        FlashArrowCount = 111,
                        NetArrowCount = 88,
                        CoverScentCount = 50,
                        BearTrapCount = 77,
                        EnableBossMode = true
                    }
                }
#if DEBUG
                ,
                {
                    DifficultySetting.BlackMuglumpInsanity,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.BlackMuglumpInsanity,
                        DisplayName = "BlackMuglumpInsanity",
                        MinimumRoomCount = 256,
                        MuglumpCount = 0,
                        BlackMuglumpCount = 30,
                        BatCount = 0,
                        PitCount = 15,
                        ArrowCount = 50,
                        FlashArrowCount = 50,
                        NetArrowCount = 50,
                        CoverScentCount = 10
                    }
                },
                {
                    DifficultySetting.BlackMuglumpTest,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.BlackMuglumpTest,
                        DisplayName = "BlackMuglumpTest",
                        MinimumRoomCount = 32,
                        MuglumpCount = 0,
                        BlackMuglumpCount = 1,
                        BatCount = 0,
                        PitCount = 0,
                        ArrowCount = 4,
                        FlashArrowCount = 4,
                        NetArrowCount = 4,
                        EnableBossMode = true
                    }
                },
                {
                    DifficultySetting.BlueMuglumpTest,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.BlueMuglumpTest,
                        DisplayName = "BlueMuglumpTest",
                        MinimumRoomCount = 60,
                        BlueMuglumpCount = 5,
                        MuglumpCount = 0,
                        BlackMuglumpCount = 0,
                        PitCount = 0,
                        BatCount = 0,
                        ArrowCount = 10,
                        FlashArrowCount = 10,
                        NetArrowCount = 10,
                        CoverScentCount = 10
                    }
                },
                {
                    DifficultySetting.Empty,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.Empty,
                        DisplayName = "Empty",
                        BlueMuglumpCount = 0,
                        MuglumpCount = 0,
                        BlackMuglumpCount = 0,
                        PitCount = 0,
                        BatCount = 0,
                        MinimumRoomCount = 256,
                        FlashArrowCount = 99,
                        NetArrowCount = 99,
                        CoverScentCount = 99
                    }
                },
                {
                    DifficultySetting.LotsOfBats,
                    new Difficulty()
                    {
                        Setting = DifficultySetting.LotsOfBats,
                        DisplayName = "LotsOfBats",
                        BatCount = 75,
                        MinimumRoomCount = 500,
                        EnableBossMode = false
                    }
                }
#endif
            };

            if (IsCustomModeEnabled())
            {
                difficulties.Add(DifficultySetting.Custom, GetCustomDifficulty());
            }

            return difficulties;
        }

        private string displayName;

        private int silverbackMuglumpCount;

        /// <summary>
        /// Prevents a default instance of the <see cref="Difficulty"/> class from being created.
        /// </summary>
        public Difficulty() { }

        /// <summary>
        /// Gets the setting.
        /// </summary>
        /// <value>
        /// The setting.
        /// </value>
        public DifficultySetting Setting { get; set; }

        public string LocalizedDisplayName
        {
            get => LocalizationSettings.StringDatabase.GetLocalizedString(
                    StringContent.StringContentTable, 
                    this.displayName,
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
        }

        public string DisplayName
        {
            get => this.displayName;
            set => this.displayName = value; 
        }



        /// <summary>
        /// Gets the minimum room count.
        /// </summary>
        /// <value>
        /// The minimum room count.
        /// </value>
        public int MinimumRoomCount { get; set; }

        /// <summary>
        /// Gets the muglump count.
        /// </summary>
        /// <value>
        /// The muglump count.
        /// </value>
        public int MuglumpCount { get; set; }

        /// <summary>
        /// Gets the black muglump count.
        /// </summary>
        /// <value>
        /// The black muglump count.
        /// </value>
        public int BlackMuglumpCount { get; set; }

        /// <summary>
        /// Gets the blue muglump count.
        /// </summary>
        /// <value>
        /// The blue muglump count.
        /// </value>
        public int BlueMuglumpCount { get; set; }

        /// <summary>
        /// Gets or sets the silverback muglump count.
        /// </summary>
        public int SilverbackMuglumpCount 
        { 
            get
            {
                if (Settings.EnableSilverbackMuglumps || Settings.Difficulty.IsCustomMode())
                {
                    return this.silverbackMuglumpCount;
                }

                return 0;
            }

            set
            {
                this.silverbackMuglumpCount = value;
            }
        }

        /// <summary>
        /// Gets the total muglump count.
        /// </summary>
        /// <value>
        /// The total muglump count.
        /// </value>
        public int TotalMuglumpCount
        {
            get { return this.MuglumpCount + this.BlackMuglumpCount + this.BlueMuglumpCount + this.SilverbackMuglumpCount; }
        }

        /// <summary>
        /// Gets or sets the bat count.
        /// </summary>
        /// <value>
        /// The bat count.
        /// </value>
        public int BatCount { get; set; }

        /// <summary>
        /// Gets or sets the pit count.
        /// </summary>
        /// <value>
        /// The pit count.
        /// </value>
        public int PitCount { get; set; }

        /// <summary>
        /// Gets or sets the arrow count.
        /// </summary>
        /// <value>
        /// The arrow count.
        /// </value>
        public int ArrowCount { get; set; }

        /// <summary>
        /// Gets or sets the flash arrow count.
        /// </summary>
        /// <value>
        /// The flash arrow count.
        /// </value>
        public int FlashArrowCount { get; set; }

        /// <summary>
        /// Gets or sets the net arrow count.
        /// </summary>
        /// <value>
        /// The net arrow count.
        /// </value>
        public int NetArrowCount { get; set; }

        /// <summary>
        /// Gets or sets the cover scent count.
        /// </summary>
        /// <value>
        /// The cover scent count.
        /// </value>
        public int CoverScentCount { get; set; }

        /// <summary>
        /// Gets or sets the bear trap count.
        /// </summary>
        public int BearTrapCount { get; set; }

        public bool TrapsAttractHunters { get; set; }

        public bool EnableBossMode { get; set; }

        public bool IsCustomMode()
        {
            return this.Setting == DifficultySetting.Custom;
        }

        /// <summary>
        /// Gets the difficulty.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns></returns>
        public static Difficulty GetDifficulty(DifficultySetting setting)
        {
            if (!difficulties.ContainsKey(setting))
            {
                return null;
            }

            return difficulties[setting];
        }

        public static void SetCustomDifficulty(Difficulty customDifficulty)
        {
            if (!difficulties.ContainsKey(DifficultySetting.Custom) || !(customDifficulty.Setting == DifficultySetting.Custom))
            {
                return;
            }

            difficulties[DifficultySetting.Custom] = customDifficulty;
        }

        /// <summary>
        /// Gets the difficulties.
        /// </summary>
        /// <returns></returns>
        public static Difficulty[] GetDifficulties()
        {
            if (IsCustomModeEnabled())
            {
                EnableCustomDifficulty();
            }

            return difficulties.Values.OrderBy(difficulty => difficulty.Setting).ToArray();
        }

        public static bool IsCustomModeEnabled()
        {
            var dungeonCrawlerBadge = Badge.GetStaticBadges().Where(badge => badge.Name == "DungeonCrawler").FirstOrDefault();

            return true || dungeonCrawlerBadge != null && dungeonCrawlerBadge.Enabled;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Setting.ToString();
        }

        private static Difficulty GetCustomDifficulty()
        {
            Difficulty customDifficulty = null;

            if (!PlayerPrefsManager.IsKeyRegistered(CustomModePlayerPrefsKey, PlayerPrefsDataType.Json))
            {
                PlayerPrefsManager.RegisterKey(CustomModePlayerPrefsKey, PlayerPrefsDataType.Json);
            }

            customDifficulty = PlayerPrefsManager.GetData<Difficulty>(CustomModePlayerPrefsKey);

            if (customDifficulty == null || customDifficulty.Setting != DifficultySetting.Custom)
            {
                customDifficulty = new Difficulty() { Setting = DifficultySetting.Custom, MinimumRoomCount = 50 };
            }

            customDifficulty.EnableBossMode = true;
            customDifficulty.DisplayName = "Custom";

            return customDifficulty;
        }

        public static void SaveCustomDifficulty()
        {
            if (!difficulties.ContainsKey(DifficultySetting.Custom))
            {
                return;
            }

            SaveCustomDifficultyUnsafe();
        }

        private static void SaveCustomDifficultyUnsafe()
        {
            var customDifficulty = difficulties[DifficultySetting.Custom];

            PlayerPrefsManager.SetDataProperty(CustomModePlayerPrefsKey, customDifficulty);
        }

        public static void EnableCustomDifficulty()
        {
            if (!difficulties.ContainsKey(DifficultySetting.Custom))
            {
                difficulties.Add(DifficultySetting.Custom, GetCustomDifficulty());
            }
        }

        public static void DisableCustomDifficulty()
        {
            if (difficulties.ContainsKey(DifficultySetting.Custom))
            {
                SaveCustomDifficultyUnsafe();
                difficulties.Remove(DifficultySetting.Custom);
                Settings.Difficulty = Difficulty.Default;
            }
        }

        public static void EnableTrapAttraction()
        {
            Settings.Difficulty.TrapsAttractHunters = true;
        }

        public static void DisableTrapAttraction()
        {
            Settings.Difficulty.TrapsAttractHunters = false;
        }
    }

}
