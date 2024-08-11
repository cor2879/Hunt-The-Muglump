/**************************************************
 *  GameOverSettings.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines a data structure for specifying the state of the endgame.
    /// </summary>
    public class GameOverSettings
    {
        /// <summary>
        /// Gets or sets the game over condition.
        /// </summary>
        /// <value>
        /// The game over condition.
        /// </value>
        public GameOverCondition GameOverCondition { get; set; }

        /// <summary>
        /// Gets or sets the red muglumps slain count.
        /// </summary>
        /// <value>
        /// The red muglumps slain count.
        /// </value>
        public int RedMuglumpsSlainCount { get; set; }

        /// <summary>
        /// Gets or sets the black muglumps slain count.
        /// </summary>
        /// <value>
        /// The black muglumps slain count.
        /// </value>
        public int BlackMuglumpsSlainCount { get; set; }

        /// <summary>
        /// Gets or sets the gold muglumps slain count.
        /// </summary>
        /// <value>
        /// The gold muglumps slain count.
        /// </value>
        public int GoldMuglumpsSlainCount { get; set; }

        /// <summary>
        /// Gets or sets the blue muglumps slain count.
        /// </summary>
        /// <value>
        /// The blue muglumps slain count.
        /// </value>
        public int BlueMuglumpsSlainCount { get; set; }

        /// <summary>
        /// Gets or sets the silverback muglumps slain count.
        /// </summary>
        public int SilverbackMuglumpsSlainCount { get; set; }

        /// <summary>
        /// Gets or sets the total muglumps count.
        /// </summary>
        /// <value>
        /// The total muglumps count.
        /// </value>
        public int TotalRedMuglumpsCount { get; set; }

        public int TotalBlackMuglumpsCount { get; set; }

        public int TotalBlueMuglumpsCount { get; set; }

        public int TotalGoldMuglumpsCount { get; set; }

        public int TotalSilverbackMuglumpsCount { get; set; }

        public int TotalMuglumpsCount
        {
            get => MathfExtension.Sum(
                this.TotalRedMuglumpsCount,
                this.TotalBlackMuglumpsCount,
                this.TotalBlueMuglumpsCount,
                this.TotalGoldMuglumpsCount,
                this.TotalSilverbackMuglumpsCount);
        }

        /// <summary>
        /// Gets or sets the total muglumps slain count.
        /// </summary>
        /// <value>
        /// The total muglumps slain count.
        /// </value>
        public int TotalMuglumpsSlainCount
        {
            get
            {
                return MathfExtension.Sum(
                    this.RedMuglumpsSlainCount,
                    this.BlackMuglumpsSlainCount,
                    this.BlueMuglumpsSlainCount,
                    this.GoldMuglumpsSlainCount,
                    this.SilverbackMuglumpsSlainCount);
            }
        }

        /// <summary>
        /// Gets or sets the arrows fired count.
        /// </summary>
        /// <value>
        /// The arrows fired count.
        /// </value>
        public int ArrowsFiredCount { get; set; }

        public int FlashArrowsFiredCount { get; set; }

        public int NetArrowsFiredCount { get; set; }

        public int CoverScentsUsed { get; set; }

        /// <summary>
        /// Gets or sets the arrows hit count.
        /// </summary>
        /// <value>
        /// The arrows hit count.
        /// </value>
        public int ArrowsHitCount { get; set; }

        public int FlashArrowsHitCount { get; set; }

        public int NetArrowsHitCount { get; set; }

        public float Accuracy
        {
            get
            {
                var divisor = (float)(this.ArrowsFiredCount + this.FlashArrowsFiredCount + this.NetArrowsFiredCount);

                if (divisor == 0)
                {
                    return 0.00f;
                }

                var dividend = (float)(this.ArrowsHitCount + this.FlashArrowsHitCount + this.NetArrowsHitCount);

                return dividend / divisor;
            }
        }

        /// <summary>
        /// Gets or sets the total arrows count.
        /// </summary>
        /// <value>
        /// The total arrows count.
        /// </value>
        public int ArrowsCount { get; set; }

        public int FlashArrowsCount { get; set; }

        public int NetArrowsCount { get; set; }

        public int CoverScentCount { get; set; }

        /// <summary>
        /// Gets or sets the rooms visited count.
        /// </summary>
        /// <value>
        /// The rooms visited count.
        /// </value>
        public int RoomsVisitedCount { get; set; }

        /// <summary>
        /// Gets or sets the total rooms count.
        /// </summary>
        /// <value>
        /// The total rooms count.
        /// </value>
        public int TotalRoomsCount { get; set; }

        /// <summary>
        /// Gets or sets the number of moves count.
        /// </summary>
        /// <value>
        /// The number of moves count.
        /// </value>
        public int NumberOfMovesCount { get; set; }

        /// <summary>
        /// Gets or sets the times carried count.
        /// </summary>
        /// <value>
        /// The times carried count.
        /// </value>
        public int TimesCarriedCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [found the crown].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [found the crown]; otherwise, <c>false</c>.
        /// </value>
        public bool FoundTheCrown { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player survived.
        /// </summary>
        /// <value>
        ///   <c>true</c> if survived; otherwise, <c>false</c>.
        /// </value>
        public bool Survived { get; set; }

        public DifficultySetting DifficultySetting { get; set; }

        /// <summary>
        /// Gets or sets the total score.
        /// </summary>
        /// <value>
        /// The total score.
        /// </value>
        public int TotalScore { get; set; }
    }
}
