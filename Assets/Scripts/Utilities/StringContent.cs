/**************************************************
 *  StringContent.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Utilities
{
    using System;
    using System.Collections.Generic;

    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;

    /// <summary>
    /// Centralized class for storing and retrieving all text in the game.
    /// </summary>
    public static class StringContent
    {
        public const string StringContentTable = "LocalizedStringConstants";

        public static Dictionary<ArrowType, Func<string>> ArrowEmpty = new Dictionary<ArrowType, Func<string>>()
        {
            { ArrowType.Arrow, () => 
                LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    "ArrowsEmpty",
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))) },
            // "Thou reachest into thine quiver to draw an arrow, only to find it is empty.  Perhaps thou shouldest have brought more."
            { ArrowType.FlashArrow, () => 
                LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    "FlashArrowsEmpty",
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))) },
            // "Thou dost not have any flash arrows!  A pity..."
            { ArrowType.NetArrow, () => 
                LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    "NetArrowsEmpty",
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)))  }
            // "Thou hast not any net arrows!"
        };

        public static readonly Dictionary<ItemType, Func<string>> ItemEmpty = new Dictionary<ItemType, Func<string>>()
        {
            { ItemType.EauDuMuglump, () => 
                LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    "EauDuMuglumpEmpty",
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))) },
            // "Thy desire to smell like a muglump goes unfulfilled.  There is no Eau du Muglump."
            { ItemType.BearTrap, () => 
                LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    "BearTrapsEmpty",
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))) }
            // "This might be a fantastic spot for a bear trap, if you had one."
        };

        public static string AllMuglumpsHunted
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    "AllMuglumpsHunted",
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "You have eliminated all muglumps in this dungeon.  Return to the entrance!";
        }

        public static string BatWarning
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    "BatWarning",
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Thou canst hear the fluttering of wings from a nearby room.";
        }

        public static string BatCarry
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    "BatCarry",
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Giant bats are carrying thee!";
        }

        public static string BearTrapTrigger
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(BearTrapTrigger),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "One of your bear traps was triggered.";
        }

        public static string BlackMuglumpHit
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(BlackMuglumpHit),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "You found the Black Muglump!  Now follow his trail and finish him off.";
        }

        public static string BlackMuglumpClue
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(BlackMuglumpClue),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "The Black Muglump's trail ends here.  It must be in one of the adjacent rooms.  Choose your aim wisely...";
        }

        public static string BlueMuglumpWarning
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(BlueMuglumpWarning),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Have a care!  A Blue Muglump has caught thy scent!";
        }

        public static string SilverbackMuglumpWarning
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(SilverbackMuglumpWarning),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Have a care!  A Silverback Muglump has caught thy scent!";
        }

        public static string BlueMuglumpDireWarning
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(BlueMuglumpDireWarning),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "A Blue Muglump has thee nearly in its grasp!";
        }

        public static string SilverbackMuglumpDireWarning
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(SilverbackMuglumpDireWarning),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "A Silverback Muglump has thee nearly in its grasp!";
        }

        public static string CoverScentExpired
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(CoverScentExpired),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Beware! Thy Eau du Muglump has worn off!";
        }

        public static string ExitDungeonConfirmation
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(ExitDungeonConfirmation),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Art thou ready to leave the dungeon?  This will end the game.";
        }

        public static string ForfeitDungeonConfirmation
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(ForfeitDungeonConfirmation),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Art thou sure?  This will forfeit the game.";
        }

        public static readonly Dictionary<ArrowType, Func<string>> FoundArrow = new Dictionary<ArrowType, Func<string>>()
        {
            { ArrowType.Arrow, () => LocalizationUtility.GetLocalizedString(
                                        StringContentTable, 
                                        "FoundArrow",
                                        locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))) },
            // "Thou hast found a useable arrow." 
            { ArrowType.FlashArrow, () => LocalizationUtility.GetLocalizedString(
                                            StringContentTable, 
                                            "FoundFlashArrow",
                                            locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))) },
            // "Such good fortune!  Thou hast acquired a flash arrow!"
            { ArrowType.NetArrow, () => LocalizationUtility.GetLocalizedString(
                                            StringContentTable, 
                                            "FoundNetArrow",
                                            locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))) }
            // "A net arrow!  Use this to cross pit traps safely."
        };

        public static readonly Dictionary<ItemType, Func<string>> FoundItem = new Dictionary<ItemType, Func<string>>()
        {
            { ItemType.EauDuMuglump, () => LocalizationUtility.GetLocalizedString(
                                            StringContentTable, 
                                            "FoundEauDuMuglump",
                                            locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))) },
            // "A bottle of Eau du Muglump.  It will make thee smell like a muglump for a few turns."
            { ItemType.BearTrap, () => LocalizationUtility.GetLocalizedString(
                                        StringContentTable, 
                                        "FoundBearTrap",
                                        locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))) }
            // "You found a bear trap.  Also works on muglumps!"
        };

        public static string FoundCrown
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(FoundCrown),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Fortune smiles upon thee, for thou hast found the crown!";
        }

        public static string GameOverVictoryText
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(GameOverVictoryText),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "You survived the dungeon!";
        }

        public static string GameOverEatenText
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(GameOverEatenText),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "You were eaten by a muglump!";
        }

        public static string GameOverFallenText
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(GameOverFallenText),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "You have fallen to your doom!";
        }

        public static string GameOverQuitText 
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(GameOverQuitText),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Does it feel good to be a quitter?";
        }

        public static string Gamepad
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(Gamepad),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Gamepad";
        }

        public static string Keyboard
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(Keyboard),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Keyboard";
        }

        public static readonly Func<string>[] LocalizedAmountOptions = new Func<string>[] 
        { 
            () => LocalizationUtility.GetLocalizedString(StringContentTable, Constants.None, locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))),
            () => LocalizationUtility.GetLocalizedString(StringContentTable, Constants.One, locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))),
            () => LocalizationUtility.GetLocalizedString(StringContentTable, Constants.Few, locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))),
            () => LocalizationUtility.GetLocalizedString(StringContentTable, Constants.Some, locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))),
            () => LocalizationUtility.GetLocalizedString(StringContentTable, Constants.Many, locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))),
            () => LocalizationUtility.GetLocalizedString(StringContentTable, Constants.Lots, locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)))
        };

        public static readonly Dictionary<Type, Func<string>> MuglumpKill = new Dictionary<Type, Func<string>>()
        {
            { typeof(MuglumpBehaviour), () => LocalizationUtility.GetLocalizedString(
                                                StringContentTable, 
                                                "MuglumpKill",
                                                locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))) },
            // "Thy courage and wit have served thee well, for thou hast slain a muglump."
            { typeof(BlackMuglumpBehaviour), () => LocalizationUtility.GetLocalizedString(
                                                    StringContentTable, 
                                                    "BlackMuglumpKill",
                                                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))) },
            // "Congratulations, the Black Muglump was no match for thee."
            { typeof(GoldMuglumpBehaviour), () => LocalizationUtility.GetLocalizedString(
                                                    StringContentTable, 
                                                    "GoldMuglumpKill",
                                                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)))  },
            // "What's this!?  A Gold Muglump!?  Thou art truly a great Muglump Hunter!"
            { typeof(BlueMuglumpBehaviour), () => LocalizationUtility.GetLocalizedString(
                                                    StringContentTable, 
                                                    "BlueMuglumpKill",
                                                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))) },
            // "The hunter who was hunted has now defeated the hunted hunter."
            { typeof(SilverbackMuglumpBehaviour), () => LocalizationUtility.GetLocalizedString(
                                                            StringContentTable, 
                                                            "SilverbackMuglumpKill",
                                                            locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode))) }
            // "The fearsome Silverback Muglump was no match for thee.  Congratulations."
        };

        public static string MuglumpWarning
        {
            get => LocalizationUtility.GetLocalizedString(
                StringContentTable, 
                nameof(MuglumpWarning),
                locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Thou canst hear heavy, labored breathing from a nearby room."
        }

        public static string MuglumpDeath
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(MuglumpDeath),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Like so many that have come before thee, thou art now in a muglump's belly.";
        }

        public static string NewSilverbackMuglump
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(NewSilverbackMuglump),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "A new Silverback Muglump has entered the dungeon!";
        }

        public static string No
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(No),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "No";
        }

        public static string PitWarning
        {
            get => LocalizationUtility.GetLocalizedString(
                StringContentTable, 
                nameof(PitWarning), 
                locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Thou hearest a draft coming from an adjacent room.";
        }

        public static string PitDeath
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(PitDeath),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Thou hast fallen to thy death.  Next time, watcheth thy step.";
        }

        public static string Yes
        {
            get => LocalizationUtility.GetLocalizedString(
                    StringContentTable, 
                    nameof(Yes),
                    locale: LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(Settings.SelectedLanguage.CultureCode)));
            // "Yes";
        }

        public static readonly string[] KeyboardActions = new string[]
        {
            "Move North",
            "Move West", 
            "Move South",
            "Move East", 
            string.Empty,
            "Fire North",
            "Fire West", 
            "Fire South",
            "Fire East", 
            string.Empty,
            "Cycle Inventory Forward",
            "Cycle Inventory Back",   
            string.Empty,
            "Open/Close Minimap",     
            "Center Minimap on Player",
            "Zoom Minimap In",         
            "Zoom Minimap Out",        
            string.Empty,
            "Refresh room messages",   
            "Pause/Open Menu "
        };

        public static readonly string[] KeyboardCommandKeys = new string[]
        {
            "W",
            "A",
            "S",
            "D",
            string.Empty,
            "NumPad 8",
            "NumPad 4",
            "NumPad 2",
            "NumPad 6",
            string.Empty,
            ".",
            ",",
            string.Empty,
            "M",
            "Space",
            "=",
            "-",
            string.Empty,
            "Space",
            "ESC"
        };
    }
}
