namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using UnityEngine;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;

    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Platform;
    using OldSchoolGames.HuntTheMuglump.Scripts.Rules;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class Badge
    {
        private bool? earned;
        private bool? enabled;
        private DateTimeOffset? earnedDate;

        private string earnedPlayerPrefsKey;
        private string earnedDatePlayerPrefsKey;
        private string enabledPlayerPrefsKey;
        private string displayName;
        private string description;
        private string bonusDescription;
        private string currentCulture;

        private Badge()
        {
#if DEBUG

#endif
        }

        private Badge(IEnumerable<Rule> rules) : this()
        {
            foreach (var rule in rules)
            {
                this.AddRule(rule);
            }
        }

        public void StartListening()
        {
            foreach (var rule in this.Rules)
            {
                rule.OnUpdate += this.OnRuleUpdated;
            }
        }

        public void StopListening()
        {
            foreach (var rule in this.Rules)
            {
                rule.OnUpdate -= this.OnRuleUpdated;
            }
        }

        private List<Rule> InnerRules { get; } = new List<Rule>();

        public Rule[] Rules { get { return this.InnerRules.ToArray(); } }

        public string Name { get; private set; }

        public Color BackgroundColor { get; private set; } = Color.black;

        public string DisplayName 
        {
            get => LocalizationSettings.StringDatabase.GetLocalizedString(
                    StringContent.StringContentTable, 
                    this.displayName,
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            set => this.displayName = value;
        }

        public string Description 
        { 
            get => LocalizationSettings.StringDatabase.GetLocalizedString(
                    StringContent.StringContentTable, 
                    this.description,
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))); 
            set => this.description = value; 
        }

        public IEnumerable<Pair<string, string>> BonusDescriptionParameters 
        {
            get;
            set; 
        }

        public string BonusDescription 
        { 
            get
            {
                if ((this.BonusDescriptionParameters != null && this.BonusDescriptionParameters.Any()) && 
                    (string.IsNullOrEmpty(this.bonusDescription) || 
                    !string.Equals(LocalizationSettings.SelectedLocale.LocaleName, this.currentCulture, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var fallbackBehavior = FallbackBehavior.UseProjectSettings;

                    this.currentCulture = LocalizationSettings.SelectedLocale.LocaleName;
                    var stringBuilder = new StringBuilder(
                        LocalizationSettings.StringDatabase.GetLocalizedString(
                            StringContent.StringContentTable,
                            this.BonusDescriptionParameters.First().First,
                            locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)),
                            fallbackBehavior,
                            this.BonusDescriptionParameters.First().Second));

                    foreach (var pair in this.BonusDescriptionParameters.Skip(1))
                    {
                        stringBuilder.Append(
                            $",{LocalizationSettings.StringDatabase.GetLocalizedString(StringContent.StringContentTable, pair.First, locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)), fallbackBehavior, pair.Second)}");
                    }

                    this.bonusDescription = stringBuilder.ToString();
                }

                return this.bonusDescription;
            }
        }

        public string TextureName { get; private set; }

        private string EarnedPlayerPrefsKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.earnedPlayerPrefsKey))
                {
                    this.earnedPlayerPrefsKey = this.BuildPlayerPrefsKey(nameof(Badge.Earned));

                    if (!PlayerPrefsManager.IsKeyRegistered(this.earnedPlayerPrefsKey, PlayerPrefsDataType.Bool))
                    {
                        PlayerPrefsManager.RegisterKey(this.earnedPlayerPrefsKey, PlayerPrefsDataType.Bool);
                    }
                }

                return this.earnedPlayerPrefsKey;
            }
        }

        private string EarnedDatePlayerPrefsKey
        {
            get
            {
                if (this.earnedDatePlayerPrefsKey == null)
                {
                    this.earnedDatePlayerPrefsKey = this.BuildPlayerPrefsKey(nameof(Badge.EarnedDate));

                    if (!PlayerPrefsManager.IsKeyRegistered(this.earnedDatePlayerPrefsKey, PlayerPrefsDataType.DateTimeOffset))
                    {
                        PlayerPrefsManager.RegisterKey(this.earnedDatePlayerPrefsKey, PlayerPrefsDataType.DateTimeOffset);
                    }
                }

                return this.earnedDatePlayerPrefsKey;
            }
        }

        private string EnabledPlayerPrefsKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.enabledPlayerPrefsKey))
                {
                    this.enabledPlayerPrefsKey = this.BuildPlayerPrefsKey(nameof(Badge.Enabled));

                    if (!PlayerPrefsManager.IsKeyRegistered(this.enabledPlayerPrefsKey, PlayerPrefsDataType.Bool))
                    {
                        PlayerPrefsManager.RegisterKey(this.enabledPlayerPrefsKey, PlayerPrefsDataType.Bool);
                    }
                }

                return this.enabledPlayerPrefsKey;
            }
        }

        public bool Earned
        {
            get
            {
                if (this.earned == null)
                {
                    this.earned = PlayerPrefsManager.GetBool(this.EarnedPlayerPrefsKey);
                }

                return this.earned.Value;
            }

            set
            {
                if (this.earned != value)
                {
                    PlayerPrefsManager.SetProperty(this.EarnedPlayerPrefsKey, value);
                }

                var oldValue = this.earned;
                this.earned = value;

                if (oldValue != null && !oldValue.Value && value)
                {
                    this.BadgeEarned();
                }
            }
        }

        public DateTimeOffset? EarnedDate
        {
            get
            {
                if (this.earnedDate == null)
                {
                    this.earnedDate = PlayerPrefsManager.GetDateTimeOffset(this.EarnedDatePlayerPrefsKey);
                }

                return this.earnedDate;
            }

            set
            {
                PlayerPrefsManager.SetProperty(this.EarnedDatePlayerPrefsKey, value ?? default(DateTimeOffset));
                this.earnedDate = value;
            }
        }

        public bool Enabled
        {
            get
            {
                if (!this.Earned)
                {
                    return false;
                }

                if (this.enabled == null)
                {
                    this.enabled = PlayerPrefsManager.GetBool(this.EnabledPlayerPrefsKey, defaultValue: true);
                }

                return this.enabled.Value;
            }

            set
            {
                var oldValue = this.enabled;

                if (this.Earned)
                {
                    if (this.enabled != value)
                    {
                        PlayerPrefsManager.SetProperty(this.EnabledPlayerPrefsKey, value);
                    }

                    this.enabled = value;
                }
                else
                {
                    this.enabled = false;
                }

                if (this.enabled != oldValue)
                {
                    if (this.enabled.Value)
                    {
                        this.OnBadgeEnabled();
                    }
                    else
                    {
                        this.OnBadgeDisabled();
                    }
                }
            }
        }

        public Action OnEnabled { get; set; }

        public Action OnDisabled { get; set; }

        private void BadgeEarned()
        {
            this.OnBadgeEarned?.Invoke(this);
            this.EarnedDate = DateTime.UtcNow;
            PlatformManager.Instance.EarnBadge(this);
        }

        public Action<Badge> OnBadgeEarned;

        private Action InnerApply;

        public void Apply()
        {
            this.InnerApply?.Invoke();
        }

        /// <summary>
        /// Evaluates this instance.
        /// </summary>
        /// <returns></returns>
        public bool Evaluate()
        {
            return !this.Rules.Any() || this.InnerRules.TrueForAll(rule =>
            {
                var earned = rule.Evaluate();
                return earned;
            });
        }

        private void OnRuleUpdated()
        {
            if (!this.Earned)
            {
                this.Earned = this.Evaluate();
            }
        }

        public void OnBadgeEnabled()
        {
            this.OnEnabled?.Invoke();
        }

        public void OnBadgeDisabled()
        {
            this.OnDisabled?.Invoke();
        }

        public void AddRule(Rule rule)
        {
            this.InnerRules.Add(rule);
        }

        private string BuildPlayerPrefsKey(string propertyName)
        {
            return $"{nameof(Badge)}_{this.Name}_{propertyName}";
        }

        public void DeleteBadge()
        {
            PlayerPrefsManager.DeleteKey(this.EarnedDatePlayerPrefsKey);
            PlayerPrefsManager.DeleteKey(this.EarnedPlayerPrefsKey);
            PlayerPrefsManager.DeleteKey(this.EnabledPlayerPrefsKey);
        }

        public static Badge[] GetStaticBadges()
        {
            return new Badge[]
            {
                new Badge(new Rule[]
                    {
                        StaticRules.NoMuglumpsKilled,
                        StaticRules.Survived
                    })
                {
                    Name = "Coward",
                    DisplayName = "CowardsDieManyTimes",
                    Description = "EscapeTheDungeonAliveWithoutKillingASingleMuglump",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("FlashArrowBonus", "+1")
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.Inventory.AddFlashArrows(1);
                    },
                    TextureName = Textures.Player
                },

                new Badge(new Rule[]
                {
                    StaticRules.FirstBlood
                })
                {
                    Name = "FirstBlood",
                    DisplayName = "FirstBlood",
                    Description = "SlayYourFirstMuglump",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("ArrowBonus", "+1")
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.Inventory.AddArrows(1);
                    },
                    TextureName = Textures.MuglumpHandsUp
                },

                new Badge(new Rule[]
                {
                    StaticRules.Shiny
                })
                {
                    Name = "Shiny",
                    DisplayName = "Shiny",
                    Description = "FindTheCrown",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("NetArrowBonus", "+1")
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.Inventory.AddNetArrows(1);
                    },
                    TextureName = Textures.Crown
                },

                new Badge(new Rule[]
                {
                    StaticRules.HitchHiker
                })
                {
                    Name = "HitchHiker",
                    DisplayName = "Hitchhiker",
                    Description = "RideTheBatsFiveTimesInASingleGameWithoutDying",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("FlashArrowBonus", "+1")
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.Inventory.AddFlashArrows(1);
                    },
                    TextureName = Textures.Bats
                },

                new Badge(new Rule[]
                {
                    StaticRules.MuglumpSlayer
                })
                {
                    Name = "MuglumpSlayer",
                    DisplayName = "MuglumpSlayer",
                    Description = "Kill10Muglumps",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("ArrowBonus", "+1")
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.Inventory.AddArrows(3);
                    },
                    TextureName = Textures.MuglumpHandsUp
                },

                new Badge(new Rule[]
                {
                    StaticRules.BatRider
                })
                {
                    Name = "BatRider",
                    DisplayName = "BatRider",
                    Description = "RideTheBatsATotalOf25Times",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("FlashArrowBonus", "+3")
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.Inventory.AddFlashArrows(3);
                    },
                    TextureName = Textures.Bats
                },

                new Badge(new Rule[]
                {
                    StaticRules.NormalDifficulty,
                    StaticRules.AllMuglumpsKilled,
                    StaticRules.Accurate
                })
                {
                    Name = "Sharpshooter",
                    DisplayName = "Sharpshooter",
                    Description = "KillAllTheMuglumpsInADungeonWithoutMissing",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("ArrowBonus", "+2")
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.Inventory.AddArrows(2);
                    },
                    TextureName = Textures.Arrow
                },

                new Badge(new Rule[]
                {
                    StaticRules.NormalDifficulty,
                    StaticRules.AllMuglumpsKilled
                })
                {
                    Name = "HuntTheMuglumps",
                    DisplayName = "HuntTheMuglumps",
                    Description = "HuntTheMuglumpsBadgeDescription",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("ArrowBonus", "+1"),
                        new Pair<string, string>("NetArrowBonus", "+1"),
                        new Pair<string, string>("EauDuMuglumpDuration", "+2")
                        // "+1 Arrows, +1 Net Arrows, +2 Eau du Muglump duration",
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.Inventory.AddArrows(1);
                        PlayerBehaviour.Instance.Inventory.AddNetArrows(1);
                        PlayerBehaviour.Instance.CoverScentDuration += 2;
                    },
                    TextureName = Textures.MuglumpHandsUp
                },

                new Badge(new Rule[]
                {
                    StaticRules.IfItBleeds
                })
                {
                    Name = "IfItBleeds",
                    DisplayName = "IfItBleeds",
                    Description = "IfItBleedsDescription",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("FlashArrowBonus", "+1"),
                        new Pair<string, string>("NetArrowBonus", "+1")
                        // "+1 Flash Arrows, +1 Net Arrows",
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.Inventory.AddFlashArrows(1);
                        PlayerBehaviour.Instance.Inventory.AddNetArrows(1);
                    },
                    BackgroundColor = new Color32(0xF1, 0xE5, 0x13, 0xFF),
                    TextureName = Textures.DarkMuglumpHandsUp
                },

                new Badge(new Rule[]
                {
                    StaticRules.GoldenTicket
                })
                {
                    Name = "GoldenMuglump",
                    DisplayName = "GoldenMuglump",
                    Description = "GoldMuglumpBadgeDescription",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("ArrowBonus", "+5"),
                        new Pair<string, string>("FlashArrowBonus", "+3"),
                        new Pair<string, string>("NetArrowBonus", "+2")
                        // "+5 Arrows, +3 Flash Arrows, +2 Net Arrows",
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.Inventory.AddArrows(5);
                        PlayerBehaviour.Instance.Inventory.AddFlashArrows(3);
                        PlayerBehaviour.Instance.Inventory.AddNetArrows(2);
                    },
                    TextureName = Textures.GoldMuglumpHandsUp
                },

                new Badge(new Rule[]
                {
                    StaticRules.Explorer
                })
                {
                    Name = "Explorer",
                    DisplayName = "Explorer",
                    Description = "ExplorerBadgeDescription",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("GoldMuglumpSpawnBonus", "+10%")
                        // "+10% gold muglump spawn chance",
                    },
                    OnEnabled = () =>
                    {
                        Settings.GoldMuglumpCriticalRange += 2;
                    },
                    OnDisabled = () =>
                    {
                        Settings.GoldMuglumpCriticalRange -= 2;
                    },
                    TextureName = Textures.Player
                },

                new Badge(new Rule[]
                {
                    StaticRules.DungeonCrawler
                })
                {
                    Name = "DungeonCrawler",
                    DisplayName = "DungeonCrawler",
                    Description = "DungeonCrawlerBadgeDescription",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("ArrowBonus", "+5"),
                        new Pair<string, string>("FlashArrowBonus", "+5"),
                        new Pair<string, string>("NetArrowBonus", "+5")
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.Inventory.AddArrows(5);
                        PlayerBehaviour.Instance.Inventory.AddFlashArrows(5);
                        PlayerBehaviour.Instance.Inventory.AddNetArrows(5);
                    },
                    TextureName = Textures.Player   
                },

                new Badge(new Rule[]
                {
                    StaticRules.LightItUp
                })
                {
                    Name = "LightItUp",
                    DisplayName = "LightItUp",
                    Description = "LightItUpBadgeDescription",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("FlashArrowBonus", "+2")
                        // "+2 Flash Arrows",
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.Inventory.AddFlashArrows(2);
                    },
                    TextureName = Textures.FlashArrow
                },

                new Badge(new Rule[]
                {
                    StaticRules.HuntTheHunter
                })
                {
                    Name = "HuntTheHunter",
                    DisplayName = "HuntTheHunter",
                    Description = "HuntTheHunterBadgeDescription",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("EauDuMuglumpDuration", "+5")
                        // "+5 Eau du Muglump duration",
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.CoverScentDuration += 5;
                    },
                    TextureName = Textures.BlueMuglumpHandsUp
                },

                new Badge(new Rule[]
                {
                    StaticRules.ImSpecial
                })
                {
                    Name = "ImSpecial",
                    DisplayName = "ImSpecial",
                    Description = "ImSpecialBadgeDescription",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("FlashArrowBonus", "+2"),
                        new Pair<string, string>("NetArrowBonus", "+2"),
                        new Pair<string, string>("EauDuMuglumpDuration", "+2")
                        // "+2 Flash Arrows, +2 Net Arrows, +2 Eau du Muglump duration",
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.CoverScentDuration += 2;
                        PlayerBehaviour.Instance.Inventory.AddFlashArrows(2);
                        PlayerBehaviour.Instance.Inventory.AddNetArrows(2);
                    },
                    TextureName = Textures.BlueMuglumpHandsUp
                },

                new Badge(new Rule[]
                {
                    StaticRules.ApexHunter
                })
                {
                    Name = "ApexHunter",
                    DisplayName = "ApexHunter",
                    Description = "ApexHunterBadgeDescription",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("BearTrapBonus", "+1"),
                        new Pair<string, string>("NetArrowBonus", "+1")
                        // "+1 Bear Traps, +1 Net Arrows",
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.Inventory.AddBearTraps(1);
                    }, 
                    TextureName = Textures.SilverbackMuglumpHandsUp
                },

                new Badge(new Rule[]
                {
                    StaticRules.MuglumpMastery
                })
                {
                    Name = "MuglumpMastery",
                    DisplayName = "MuglumpMastery",
                    Description = "MuglumpMasteryBadgeDescription",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("BearTrapBonus", "+2"),
                        new Pair<string, string>("NetArrowBonus", "+2"),
                        new Pair<string, string>("EauDuMuglumpBonus", "+1")
                        // "+2 Bear Traps, +2 Net Arrows, +1 Eau du Muglump",
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.Inventory.AddBearTraps(2);
                        PlayerBehaviour.Instance.Inventory.AddNetArrows(2);
                        PlayerBehaviour.Instance.Inventory.AddCoverScent(1);
                    },
                    TextureName = Textures.SilverbackMuglumpHandsUp
                },

                new Badge(new Rule[]
                {
                    StaticRules.Trapper
                })
                {
                    Name = "Trapper",
                    DisplayName = "Trapper",
                    Description = "TrapperBadgeDescription",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("EnableTrapAttractionBonus", string.Empty)
                        // "Blue and Silverback Muglumps are attracted to traps when Eau du Muglump is active",
                    },
                    OnEnabled = () =>
                    {
                        Difficulty.EnableTrapAttraction();
                    },
                    OnDisabled = () =>
                    {
                        Difficulty.DisableTrapAttraction();
                    },
                    TextureName = Textures.OpenBearTrap
                },

                new Badge(new Rule[]
                {
                    StaticRules.ExpertTrapper
                })
                {
                    Name = "ExpertTrapper",
                    DisplayName = "ExpertTrapper",
                    Description = "ExpertTrapperBadgeDescription",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("BearTrapBonus", "+1"),
                        new Pair<string, string>("EauDuMuglumpBonus", "+1")
                        // "+1 Bear Traps, +1 Eau du Muglump",
                    },
                    InnerApply = () =>
                    {
                        PlayerBehaviour.Instance.Inventory.AddBearTraps(1);
                        PlayerBehaviour.Instance.Inventory.AddCoverScent(1);
                    },
                    TextureName = Textures.OpenBearTrap
                },

                new Badge(new Rule[]
                {
                    StaticRules.MasterTrapper
                })
                {
                    Name = "MasterTrapper",
                    DisplayName = "MasterTrapper",
                    Description = "MasterTrapperBadgeDescription",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("ComingSoon", string.Empty)
                        // "Coming Soon",
                    },
                    InnerApply = () => { },
                    TextureName = Textures.OpenBearTrap
                },

                new Badge(new Rule[]
                {
                    StaticRules.Overkill
                })
                {
                    Name = "Overkill",
                    DisplayName = "Overkill",
                    Description = "OverkillBadgeDescription",
                    BonusDescriptionParameters = new List<Pair<string, string>>()
                    {
                        new Pair<string, string>("HardcoreModeBonus", string.Empty)
                        // "Unlock Hardcore Mode (coming soon)",
                    },
                    InnerApply = () => {},
                    TextureName = Textures.FlashArrow
                }
            };
        }
    }
}
