/**************************************************
 *  Constants.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Utilities
{
    using System;
    using System.Collections.Generic;

    using OldSchoolGames.HuntTheMuglump.Scripts.Platform;

    /// <summary>
    /// Defines constant values that are used for maintaining the game state
    /// </summary>
    public static class Constants
    {
        public static readonly string[] AmountOptions = new[] { None, One, Few, Some, Many, Lots };

        public static readonly float[] AmountOccupantPercentThresholds = new[] { 0.0f, 0.01f, 0.369f, 0.555f, .693f, 0.85f };

        public static readonly float[] AmountItemPercentThresholds = new[] { 0.0f, 0.01f, 0.369f, 0.555f, 0.777f, 0.963f };

        public const float DistanceTolerance = 0.001f;

        public const string Epic = "Epic";

        public const string Few = "Few";

        public const float HearingRange = 25.0f;

        /// <summary>
        /// The HeroFiringXDirection Animator parameter
        /// </summary>
        public const string HeroFiringXDirection = "HeroFiringXDirection";

        /// <summary>
        /// The HeroFiringYDirection Animator parameter
        /// </summary>
        public const string HeroFiringYDirection = "HeroFiringYDirection";

        public const string Huge = "Huge";

        public const string IsAiming = "isAiming";

        ///<summary>
        /// The is dying
        /// </summary>
        public const string IsDying = "isDying";

        /// <summary>
        /// The isDying Animator parameter
        /// </summary>
        public const string IsEating = "isEating";

        /// <summary>
        /// The is flying
        /// </summary>
        public const string IsFlying = "isFlying";

        /// <summary>
        /// The IsFiring Animator parameter
        /// </summary>
        public const string IsFiring = "isFiring";

        /// <summary>
        /// The isRoaring Animator parameter
        /// </summary>
        public const string IsRoaring = "isRoaring";

        /// <summary>
        /// The IsWalking Animator parameter
        /// </summary>
        public const string IsWalking = "isWalking";

        public static readonly Dictionary<string, float> ItemAmountPercentageThresholds = new Dictionary<string, float>
        {
            { None, AmountItemPercentThresholds[0] },
            { One, AmountItemPercentThresholds[1] },
            { Few, AmountItemPercentThresholds[2] },
            { Some, AmountItemPercentThresholds[3] },
            { Many, AmountItemPercentThresholds[4] },
            { Lots, AmountItemPercentThresholds[5] }
        };

        public const string Large = "Large";

        public const string Legendary = "Legendary";

        public const string Lots = "Lots";

        public const string Many = "Many";

        public const string Massive = "Massive";

        public const string Medium = "Medium";

        public const string No = "No";

        public const string None = "None";

        public static readonly Dictionary<string, float> OccupantAmountPercentageThresholds = new Dictionary<string, float>
        {
            { None, AmountOccupantPercentThresholds[0] },
            { One,  AmountOccupantPercentThresholds[1] },
            { Few, AmountOccupantPercentThresholds[2] },
            { Some, AmountOccupantPercentThresholds[3] },
            { Many, AmountOccupantPercentThresholds[4] },
            { Lots, AmountOccupantPercentThresholds[5] }
        };

        public const string One = "One";

        /// <summary>
        /// The pixels per unit
        /// </summary>
        public const float PixelsPerUnit = 32f;

        /// <summary>
        /// The primary scene
        /// </summary>
        public const string PrimaryScene = "Primary";

        /// <summary>
        /// The room height
        /// </summary>
        public const int RoomHeight = 10;

        public const float RoomDarknessScale = 10.1f;

        /// <summary>
        /// The room width
        /// </summary>
        public const int RoomWidth = 10;

        public static readonly string[] SizeOptions = new[] { Tiny, Small, Medium, Large, Huge, Massive, Epic, Legendary };

        public static readonly int[] SizeThresholds = new[] { 33, 77, 144, 222, 369, 555, 777, 1111 };

        public static readonly Dictionary<string, int> SizeDictionary = new Dictionary<string, int>
        {
            { Tiny, SizeThresholds[0] },
            { Small, SizeThresholds[1] },
            { Medium, SizeThresholds[2] },
            { Large, SizeThresholds[3] },
            { Huge, SizeThresholds[4] },
            { Massive, SizeThresholds[5] },
            { Epic, SizeThresholds[6] },
            { Legendary, SizeThresholds[7] }
        };

        public const string Small = "Small";

        public const string Some = "Some";

        public const string Tiny = "Tiny";

        /// <summary>
        /// The title screen scene
        /// </summary>
        public const string TitleScreenScene = "TitleScreen";

        /// <summary>
        /// The game version
        /// </summary>
        /// <remarks>
        /// Version 4.0.0.1: 
        ///     * fixed a mispelling in one of the music soundclips that was preventing it from playing.
        ///     * Changed the LERP behaviour for the Pit to use an interpolation value of 1.0f rather than 0.5f
        /// Version 4.0.0.2:
        ///     * fixed an issue with the bear traps that required the Muglumps to be at the precise y position in order to be trapped, which is not always possible
        ///     * Fixed an issue with the Blue Muglump walking animations
        /// Version 4.0.0.3:
        ///     * Fixed an issue where the background music would restart unnecessarily when saving from the Settings window.
        /// Version 4.1.0.0:
        ///     * Sound Effects!
        /// Version 4.1.1.0:
        ///     * Fixed an issue with the Sorting order of certain torches in the rooms.  Those that should appear in front of characters will and those that should not
        ///       will not.
        /// Version 4.1.1.1:
        ///     * Fixed a bug where static flash arrows sitting in a room would burn on top of an occupant (such as a Muglump)
        /// Version 4.1.2.0:
        ///     * Fixed a bug where Dark/Black Muglumps would not get caught in a trap until they reached their destination.  Then the trap would "trigger" and other
        ///       odd behaviours would result
        ///     * Removed the old main screen buttons and replaced them with Beautiful Interface buttons.  Had to make many modifications to the Beautiful Interface
        ///       ButtonUI scripts in order to make the buttons work well with GamePad/Keyboard support
        ///     * Added a new 'Quick Start' button which instantly starts a new game with whichever settings were used to play the previous game
        ///     * Added a new 'New Game' button which will launch the 'New Game' menu which allows the player to choose dungeon type and whether or not to play on Survival Mode
        ///     * If the player chooses 'Custom Mode', the 'New Game' menu will launch the new Custom Mode menu which is a much streamlined version over the old custom mode menu.
        ///       The old Custom Mode menu is still available if the player chooses.
        ///     * Custom Mode selections made on the new Custom Mode Menu will be saved across play sessions
        ///     * Rebalanced the Custom Mode dungeon generator so that a dungeon that is filled to the brim is pretty much possible.  This can cause other issues however such as
        ///       if there are not enough empty rooms for a fleeing muglump to run away to, etc.  Some of this has been addressed as well but I will really only be able to 
        ///       see how this works with longer term testing and play.
        /// Verstion 4.1.3.0:
        ///     * Added in functionality for the 'More' button on the main menu screen
        ///     * Replaced the Settings menu in both the Title Scene and Primary Scene
        ///     * fixed some missing references, refactorend refined and streamlined a number of items.
        ///     * Stretched the 'old' custom mode menu so that it no longer needs to use the scroll feature.
        /// Version 4.1.3.1:
        ///     * Updated the Settings panel to always animate the text labels within.  Was getting some odd behaviour where
        ///       some of the text fields would stop animating sometimes.  Seems like a bug in Beautiful Interface because if
        ///       I go into the Unity Editor during a debug session and just select the TextUI component, the text draws on screen.
        ///       manually invoking the 'StartAnimation' method of the given TextUI instance seems to resolve the issue.
        ///  Version 5.0.0.0:
        ///     * This version feels so different from prior versions it felt like it required another major version revision.
        ///     * The Menu system has been completely redone from top to bottom (with a few remaining to be reworked) using Beautiful Interface with my own customizations
        ///     * The game controls have been completely revamped.  Gameplay is prompted on screen at all times so there is no guessing game to figure out how to play
        ///     * Removed the jittery effect that was happening sometimes during walking.  This was actually a bug in the CameraTargetBehaviour.GetMovementVector arithmetic function.
        ///  Version 5.0.0.1
        ///     * Fixed a bug where opening/closing the Pause menu while standing over the stairs could cause erratic UI behaviour
        ///     * Added an 'aiming' state for the hero when the Shooting menu is engaged
        ///     * A 'drawing of the bowstring' sound now plays when the hero aims his bow
        ///  Version 5.0.2.0
        ///     * Added Gameplay control prompts for Gamepad play.  If a Gamepad is detected, the menus automatically
        ///       change over to Gamepad mode, and vice versa.
        ///  Version 5.0.2.1
        ///     * Added a text prompt after the game version number to indicate the platform of the current build
        ///     * Fixed some bugs in the new UI system.
        ///  Version 5.0.2.2
        ///     * Fixed a bug where the Directional menu would no longer work if the player were carried by bats
        ///  Version 5.0.2.3
        ///     * Enabled Custom Mode by default
        ///  Version 5.0.2.4
        ///     * If the 'Exit Game?' message box was launched from the Pause menu, it failed to return menuState to the main menu
        ///         on close, resulting in ERROR!  This has been resolved.
        ///  Version 5.0.2.5
        ///     * Aspect Ratio Alignment issues:
        ///         * these issues were discoverd when running Hunt the Muglump on the Steam Deck
        ///             * The 'New Game' menu is out of alignment on the Steam Deck.  Need to adjust the panel anchor points
        ///             * The text on the Settings menu is not visible in some labels, no doubt also due to an aspect ratio issue
        ///             * Some of the text on the game over screen is squished.  Some of this is in all aspect ratios
        ///     * The 'Empty Item' text displays when the player runs out of arrows rather than the 'Empty Arrow' text.
        ///     *** OTHER ***
        ///     * Remove the Credits button from the MoreButtons panel and from the game, for now - DONE
        ///     * Rebalance the Difficulty Settings - DONE
        ///     * Fixed a bug where opening the 'Quit' menu from the Pause Menu and then exiting from that could cause a
        ///       NullReference - Fixed
        ///     * It's possible for the hero to "keep walking" when he's falling into a pit - FIXED
        ///  Version 5.0.2.6
        ///     * fixed some minor cosmetic issues with the UI
        ///  Version 5.0.3.0
        ///  * Update all "GameplayPrompt" UI Prefabs/Instances to properly contain references to their buttons - DONE
        ///  * Fix the North-Edge room that is still displaying one old bitmap tile in a corner - DONE
        ///  * Update the GameplayPrompt Panels to support click-only-play - DONE
        ///  Version 5.0.3.1
        ///  * When starting a new game, the Shooting direction commands were disabled - FIXED
        ///  * Fixed an issue where the Pause menu could be launched from the GameOver screen, which would then cause the UI to go crazy
        ///  * Fixed an issue where the Pause menu could be launched in the middle of a player death sequence, which would then cause the UI to go crazy
        ///  * Removed the text animation effect from the Settings Menu
        ///  Version 5.1.0.0
        ///  * Refactored the Input handling to use the new Unity InputSystem rather than the legacy one.
        ///  Version 5.1.0.1
        ///  * Fixed an issue where the sound effect volume was not set properly when a new game loaded.
        ///  Version 6.0.0.0
        ///  * Hunt the Muglump now works in 9 different languages using Unity's localization APIs
        ///     * English, French, German, Korean, Polish, Portuguese, Russian, Spanish, and Vietnamese
        ///  * Added a music player/selector to the Settings menu
        ///  Version 6.0.0.1
        ///  * Opening the Settings Menu no longer restarts the current music track.
        ///  Version 6.0.0.2
        ///  * d'oh!  Explicit Navigation references were missing on the NewGame Menu.  This has been fixed.
        ///  Version 6.0.0.3
        ///  * Optimized the Shortest Path algorithm using by entities in the dungeon to use the A Star algorithm
        ///    rather than Djikstra's SPA
        ///  Version 6.1.0.0
        ///  * Added a third set of on screen control prompts for mobile/touch interfaces.  These are larger
        ///    buttons than the keyboard and gameplay prompts and should provide a more user friendly experience
        ///    for mobile and touchscreen platforms.
        ///  Version 6.1.0.1
        ///  * Fixed a number of minor bugs, most specifically fixed an issue where the Minimap would become scrunched
        ///    in appearance on some aspect ratios (such as the iPhone).
        ///  Version 6.1.1
        ///  * The 'Move' directional menu will now stay active as the player moves from room to room.
        ///    Also added more stateful properties to the Player behavour such as 'IsVisible' and
        ///    'IsDying'.  These properties will be used to help the Menu State Manager determine
        ///    when and what UI should be displayed on the screen at any given time.
        ///  Version 6.1.2
        ///  * Added support for Apple GameCenter authentication and achievements.  I do not know a way to test this
        ///    without pushing a build that supports achievements to the App store.
        ///  Version 6.1.3
        ///  * Added "In Association with Starship TEK Inc." to the opening splash screen.
        ///  Version 7.0.0
        ///  * Completely revamped the user interface to make it more friendly and intuitive
        ///    for touch/mobile screens.  Still need to adapt it back to the PC/Console version
        ///  * Fixed a bug where the player graphic was not displaying in the Minimap
        /// </remarks>
        public static readonly string Version = $"7.0.0-{PlatformManager.Platform}";

        /// <summary>
        /// The XDirection Animator parameter
        /// </summary>
        public const string XDirection = "xDirection";

        /// <summary>
        /// The XFiringDirection Animator parameter
        /// </summary>
        public const string XFiringDirection = "xFiringDirection";

        public const string Yes = "Yes";

        public static readonly string[] YesNoOptions = { No, Yes };

        /// <summary>
        /// The YDirection Animator parameter
        /// </summary>
        public const string YDirection = "yDirection";

        /// <summary>
        /// The YFiringDirection Animator parameter
        /// </summary>
        public const string YFiringDirection = "yFiringDirection";
    }
}