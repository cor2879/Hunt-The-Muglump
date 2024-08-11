/**************************************************
 *  CustomModeSettings.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    /// <summary>
    /// Defines the user settings in the Custom Mode menu
    /// </summary>
    /// <remarks>
    /// Note to my future self - When creating a new Settings object type that
    /// will be serialized as JSON in the PlayerPrefs blob, it is not necessary
    /// to add any special attributes or decorations to the class.  Just a standard
    /// data contract with public properties will do!
    /// </remarks>
    public class CustomModeSettings
    {
        public static readonly CustomModeSettings Default = new CustomModeSettings()
        {
            DungeonSizeSelection = 0,
            RedMuglumpAmountSelection = 0,
            DarkMuglumpAmountSelection = 0,
            HunterMuglumpAmountSelection = 0,
            SilverbackMuglumpAmountSelection = 0,
            BatAmountSelection = 0,
            PitAmountSelection = 0,
            ArrowAmountSelection = 0,
            FlashArrowAmountSelection = 0,
            NetArrowAmountSelection = 0,
            EauDuMuglumpAmountSelection = 0,
            BearTrapAmountSelection = 0
        };

        public CustomModeSettings()
        { }

        public int DungeonSizeSelection { get; set; }

        public int RedMuglumpAmountSelection { get; set; }

        public int DarkMuglumpAmountSelection { get; set; }

        public int HunterMuglumpAmountSelection { get; set; }

        public int SilverbackMuglumpAmountSelection { get; set; }

        public int BatAmountSelection { get; set; }

        public int PitAmountSelection { get; set; }

        public int ArrowAmountSelection { get; set; }

        public int FlashArrowAmountSelection { get; set; }

        public int NetArrowAmountSelection { get; set; }

        public int EauDuMuglumpAmountSelection { get; set; }

        public int BearTrapAmountSelection { get; set; }
    }
}
