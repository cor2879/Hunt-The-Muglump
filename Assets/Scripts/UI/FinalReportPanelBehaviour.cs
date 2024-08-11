#pragma warning disable CS0649
/**************************************************
 *  FinalReportPanelBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System;
    using UnityEngine;
    using UnityEngine.Localization.Settings;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the behaviour for the Final Report UI Panel
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.UIHelperBehaviour" />
    public class FinalReportPanelBehaviour : UIHelperBehaviour
    {
        /// <summary>
        /// The muglumps slain text container
        /// </summary>
        [SerializeField]
        private TextContainerBehaviour redMuglumpsSlainTextContainer;

        [SerializeField]
        private TextContainerBehaviour blackMuglumpsSlainTextContainer;

        [SerializeField]
        private TextContainerBehaviour blueMuglumpsSlainTextContainer;

        [SerializeField]
        private TextContainerBehaviour goldMuglumpsSlainTextContainer;

        [SerializeField]
        private TextContainerBehaviour silverbackMuglumpsSlainTextContainer;

        /// <summary>
        /// The rooms visited text container
        /// </summary>
        [SerializeField]
        private TextContainerBehaviour roomsVisitedTextContainer;

        /// <summary>
        /// The number of moves text container
        /// </summary>
        [SerializeField]
        private TextContainerBehaviour numberOfMovesTextContainer;

        [SerializeField]
        private TextContainerBehaviour difficultySettingTextContainer;

        /// <summary>
        /// The arrows fired text container
        /// </summary>
        [SerializeField]
        private TextContainerBehaviour arrowsFiredTextContainer;

        [SerializeField]
        private TextContainerBehaviour flashArrowsFiredTextContainer;

        [SerializeField]
        private TextContainerBehaviour netArrowsFiredTextContainer;

        [SerializeField]
        private TextContainerBehaviour accuracyTextContainer;

        [SerializeField]
        private TextContainerBehaviour coverScentTextContainer;

        /// <summary>
        /// The times carried text container
        /// </summary>
        [SerializeField]
        private TextContainerBehaviour timesCarriedTextContainer;

        /// <summary>
        /// The found the crown text container
        /// </summary>
        [SerializeField]
        private TextContainerBehaviour foundTheCrownTextContainer;

        /// <summary>
        /// The survived text container
        /// </summary>
        [SerializeField]
        private TextContainerBehaviour survivedTextContainer;

        /// <summary>
        /// The total score text container
        /// </summary>
        [SerializeField]
        private TextContainerBehaviour totalScoreTextContainer;

        [SerializeField, ReadOnly]
        private DifficultySetting difficultySetting;

        /// <summary>
        /// The muglumps slain count
        /// </summary>
        [SerializeField, ReadOnly]
        private int redMuglumpsSlainCount;

        [SerializeField, ReadOnly]
        private int blackMuglumpsSlainCount;

        [SerializeField, ReadOnly]
        private int blueMuglumpsSlainCount;

        [SerializeField, ReadOnly]
        private int goldMuglumpsSlainCount;

        [SerializeField, ReadOnly]
        private int silverbackMuglumpsSlainCount;

        /// <summary>
        /// The total muglumps count
        /// </summary>
        [SerializeField, ReadOnly]
        private int totalRedMuglumpsCount;

        /// <summary>
        /// The total muglumps count
        /// </summary>
        [SerializeField, ReadOnly]
        private int totalBlackMuglumpsCount;

        /// <summary>
        /// The total muglumps count
        /// </summary>
        [SerializeField, ReadOnly]
        private int totalBlueMuglumpsCount;

        [SerializeField, ReadOnly]
        private int totalGoldMuglumpsCount;

        [SerializeField, ReadOnly]
        private int totalSilverbackMuglumpsCount;

        /// <summary>
        /// The arrows fired count
        /// </summary>
        [SerializeField, ReadOnly]
        private int arrowsFiredCount;

        [SerializeField, ReadOnly]
        private int flashArrowsFiredCount;

        [SerializeField, ReadOnly]
        private int netArrowsFiredCount;

        [SerializeField, ReadOnly]
        private float accuracy;

        [SerializeField, ReadOnly]
        private int coverScentsUsed;

        /// <summary>
        /// The total arrows count
        /// </summary>
        [SerializeField, ReadOnly]
        private int arrowsCount;

        [SerializeField, ReadOnly]
        private int flashArrowsCount;

        [SerializeField, ReadOnly]
        private int netArrowsCount;

        [SerializeField, ReadOnly]
        private int coverScentCount;

        /// <summary>
        /// The rooms visited count
        /// </summary>
        [SerializeField, ReadOnly]
        private int roomsVisitedCount;

        /// <summary>
        /// The total rooms count
        /// </summary>
        [SerializeField, ReadOnly]
        private int totalRoomsCount;

        /// <summary>
        /// The number of moves count
        /// </summary>
        [SerializeField, ReadOnly]
        private int numberOfMovesCount;

        /// <summary>
        /// The times carried count
        /// </summary>
        [SerializeField, ReadOnly]
        private int timesCarriedCount;

        /// <summary>
        /// The found the crown
        /// </summary>
        [SerializeField, ReadOnly]
        private bool foundTheCrown;

        /// <summary>
        /// The survived
        /// </summary>
        [SerializeField, ReadOnly]
        private bool survived;

        /// <summary>
        /// The total score
        /// </summary>
        [SerializeField, ReadOnly]
        private int totalScore;

        public TextContainerBehaviour RedMuglumpsSlainTextContainer
        {
            get
            {
                if (this.redMuglumpsSlainTextContainer == null)
                {
                    throw new UIException($"The parameter {nameof(this.redMuglumpsSlainTextContainer)} needs to be set in the Unity Editor for the {nameof(FinalReportPanelBehaviour)}.");
                }

                return this.redMuglumpsSlainTextContainer;
            }
        }

        public TextContainerBehaviour BlackMuglumpsSlainTextContainer
        {
            get
            {
                if (this.blackMuglumpsSlainTextContainer == null)
                {
                    throw new UIException($"The parameter {nameof(this.blackMuglumpsSlainTextContainer)} needs to be set in the Unity Editor for the {nameof(FinalReportPanelBehaviour)}.");
                }

                return this.blackMuglumpsSlainTextContainer;
            }
        }

        public TextContainerBehaviour BlueMuglumpsSlainTextContainer
        {
            get
            {
                if (this.redMuglumpsSlainTextContainer == null)
                {
                    throw new UIException($"The parameter {nameof(this.blueMuglumpsSlainTextContainer)} needs to be set in the Unity Editor for the {nameof(FinalReportPanelBehaviour)}.");
                }

                return this.blueMuglumpsSlainTextContainer;
            }
        }

        public TextContainerBehaviour GoldMuglumpsSlainTextContainer
        {
            get
            {
                if (this.goldMuglumpsSlainTextContainer == null)
                {
                    throw new UIException($"The parameter {nameof(this.goldMuglumpsSlainTextContainer)} needs to be set in the Unity Editor for the {nameof(FinalReportPanelBehaviour)}.");
                }

                return this.goldMuglumpsSlainTextContainer;
            }
        }

        public TextContainerBehaviour SilverbackMuglumpsSlainTextContainer
        {
            get
            {
                this.ValidateUnityEditorParameter(this.silverbackMuglumpsSlainTextContainer, nameof(this.silverbackMuglumpsSlainTextContainer));

                return this.silverbackMuglumpsSlainTextContainer;
            }
        }

        public TextContainerBehaviour FlashArrowsFiredTextContainer
        {
            get
            {
                this.ValidateUnityEditorParameter(this.flashArrowsFiredTextContainer, nameof(this.flashArrowsFiredTextContainer));

                return this.flashArrowsFiredTextContainer;
            }
        }

        public TextContainerBehaviour NetArrowsFiredTextContainer
        {
            get
            {
                this.ValidateUnityEditorParameter(this.netArrowsFiredTextContainer, nameof(this.netArrowsFiredTextContainer));

                return this.netArrowsFiredTextContainer;
            }
        }

        public TextContainerBehaviour AccuracyTextContainer
        {
            get
            {
                this.ValidateUnityEditorParameter(this.accuracyTextContainer, nameof(this.accuracyTextContainer));

                return this.accuracyTextContainer;
            }
        }

        public TextContainerBehaviour CoverScentTextContainer
        {
            get
            {
                this.ValidateUnityEditorParameter(this.coverScentTextContainer, nameof(this.coverScentTextContainer));

                return this.coverScentTextContainer;
            }
        }

        public TextContainerBehaviour DifficultyTextContainer
        {
            get
            {
                this.ValidateUnityEditorParameter(this.difficultySettingTextContainer, nameof(this.difficultySettingTextContainer));

                return this.difficultySettingTextContainer;
            }
        }

        /// <summary>
        /// Gets or sets the muglumps slain count.
        /// </summary>
        /// <value>
        /// The muglumps slain count.
        /// </value>
        public int RedMuglumpsSlainCount
        {
            get => this.redMuglumpsSlainCount;
            set => this.redMuglumpsSlainCount = value;
        }

        /// <summary>
        /// Gets or sets the muglumps slain count.
        /// </summary>
        /// <value>
        /// The muglumps slain count.
        /// </value>
        public int BlackMuglumpsSlainCount
        {
            get => this.blackMuglumpsSlainCount;
            set => this.blackMuglumpsSlainCount = value;
        }

        /// <summary>
        /// Gets or sets the muglumps slain count.
        /// </summary>
        /// <value>
        /// The muglumps slain count.
        /// </value>
        public int BlueMuglumpsSlainCount
        {
            get => this.blueMuglumpsSlainCount;
            set => this.blueMuglumpsSlainCount = value;
        }

        /// <summary>
        /// Gets or sets the muglumps slain count.
        /// </summary>
        /// <value>
        /// The muglumps slain count.
        /// </value>
        public int GoldMuglumpsSlainCount
        {
            get => this.goldMuglumpsSlainCount;
            set => this.goldMuglumpsSlainCount = value;
        }

        public int SilverbackMuglumpsSlainCount
        {
            get => this.silverbackMuglumpsSlainCount;
            set => this.silverbackMuglumpsSlainCount = value;
        }

        /// <summary>
        /// Gets or sets the total muglumps count.
        /// </summary>
        /// <value>
        /// The total muglumps count.
        /// </value>
        public int TotalRedMuglumpsCount
        {
            get => this.totalRedMuglumpsCount;

            set => this.totalRedMuglumpsCount = value;
        }

        /// <summary>
        /// Gets or sets the total muglumps count.
        /// </summary>
        /// <value>
        /// The total muglumps count.
        /// </value>
        public int TotalBlackMuglumpsCount
        {
            get => this.totalBlackMuglumpsCount;

            set => this.totalBlackMuglumpsCount = value;
        }

        /// <summary>
        /// Gets or sets the total muglumps count.
        /// </summary>
        /// <value>
        /// The total muglumps count.
        /// </value>
        public int TotalBlueMuglumpsCount
        {
            get => this.totalBlueMuglumpsCount;

            set => this.totalBlueMuglumpsCount = value;
        }

        /// <summary>
        /// Gets or sets the total muglumps count.
        /// </summary>
        /// <value>
        /// The total muglumps count.
        /// </value>
        public int TotalGoldMuglumpsCount
        {
            get => this.totalGoldMuglumpsCount;

            set => this.totalGoldMuglumpsCount = value;
        }

        public int TotalSilverbackMuglumpsCount
        {
            get => this.totalSilverbackMuglumpsCount;
            set => this.totalSilverbackMuglumpsCount = value;
        }

        /// <summary>
        /// Gets or sets the arrows fired count.
        /// </summary>
        /// <value>
        /// The arrows fired count.
        /// </value>
        public int ArrowsFiredCount
        {
            get => this.arrowsFiredCount;
            set => this.arrowsFiredCount = value;
        }

        public int FlashArrowsFiredCount { get => this.flashArrowsFiredCount; set => this.flashArrowsFiredCount = value; }

        public int NetArrowsFiredCount { get => this.netArrowsFiredCount; set => this.netArrowsFiredCount = value; }

        public float Accuracy { get => this.accuracy; set => this.accuracy = value; }

        public int CoverScentsUsed { get => this.coverScentsUsed; set => this.coverScentsUsed = value; }

        /// <summary>
        /// Gets or sets the total arrows count.
        /// </summary>
        /// <value>
        /// The total arrows count.
        /// </value>
        public int ArrowsCount
        {
            get => this.arrowsCount;
            set => this.arrowsCount = value;
        }

        public int FlashArrowsCount
        {
            get => this.flashArrowsCount;
            set => this.flashArrowsCount = value;
        }

        public int NetArrowsCount { get => this.netArrowsCount; set => this.netArrowsCount = value; }

        public int CoverScentCount { get => this.coverScentCount; set => this.coverScentCount = value; }

        public DifficultySetting DifficultySetting { get => this.difficultySetting; set => this.difficultySetting = value; }

        /// <summary>
        /// Gets or sets the rooms visited count.
        /// </summary>
        /// <value>
        /// The rooms visited count.
        /// </value>
        public int RoomsVisitedCount
        {
            get => this.roomsVisitedCount;
            set => this.roomsVisitedCount = value;
        }

        /// <summary>
        /// Gets or sets the total rooms count.
        /// </summary>
        /// <value>
        /// The total rooms count.
        /// </value>
        public int TotalRoomsCount
        {
            get => this.totalRoomsCount;
            set => this.totalRoomsCount = value;
        }

        /// <summary>
        /// Gets or sets the number of moves count.
        /// </summary>
        /// <value>
        /// The number of moves count.
        /// </value>
        public int NumberOfMovesCount
        {
            get => this.numberOfMovesCount;
            set => this.numberOfMovesCount = value;
        }

        /// <summary>
        /// Gets or sets the times carried count.
        /// </summary>
        /// <value>
        /// The times carried count.
        /// </value>
        public int TimesCarriedCount
        {
            get => this.timesCarriedCount;
            set => this.timesCarriedCount = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [found the crown].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [found the crown]; otherwise, <c>false</c>.
        /// </value>
        public bool FoundTheCrown
        {
            get => this.foundTheCrown;
            set => this.foundTheCrown = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FinalReportPanelBehaviour"/> is survived.
        /// </summary>
        /// <value>
        ///   <c>true</c> if survived; otherwise, <c>false</c>.
        /// </value>
        public bool Survived
        {
            get => this.survived;
            set => this.survived = value;
        }

        /// <summary>
        /// Gets or sets the total score.
        /// </summary>
        /// <value>
        /// The total score.
        /// </value>
        public int TotalScore
        {
            get => this.totalScore;
            set => this.totalScore = value;
        }

        /// <summary>
        /// Enables the Final Report elements using the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Enable(GameOverSettings settings)
        {
            Validator.ArgumentIsNotNull(settings, nameof(settings));

            this.RedMuglumpsSlainCount = settings.RedMuglumpsSlainCount;
            this.BlackMuglumpsSlainCount = settings.BlackMuglumpsSlainCount;
            this.BlueMuglumpsSlainCount = settings.BlueMuglumpsSlainCount;
            this.GoldMuglumpsSlainCount = settings.GoldMuglumpsSlainCount;
            this.SilverbackMuglumpsSlainCount = settings.SilverbackMuglumpsSlainCount;
            this.TotalRedMuglumpsCount = settings.TotalRedMuglumpsCount;
            this.TotalBlackMuglumpsCount = settings.TotalBlackMuglumpsCount;
            this.TotalBlueMuglumpsCount = settings.TotalBlueMuglumpsCount;
            this.TotalGoldMuglumpsCount = settings.TotalGoldMuglumpsCount;
            this.TotalSilverbackMuglumpsCount = settings.TotalSilverbackMuglumpsCount;
            this.ArrowsFiredCount = settings.ArrowsFiredCount;
            this.FlashArrowsFiredCount = settings.FlashArrowsFiredCount;
            this.NetArrowsFiredCount = settings.NetArrowsFiredCount;
            this.Accuracy = settings.Accuracy;
            this.CoverScentsUsed = settings.CoverScentsUsed;
            this.ArrowsCount = settings.ArrowsCount;
            this.FlashArrowsCount = settings.FlashArrowsCount;
            this.NetArrowsCount = settings.NetArrowsCount;
            this.CoverScentCount = settings.CoverScentCount;
            this.RoomsVisitedCount = settings.RoomsVisitedCount;
            this.TotalRoomsCount = settings.TotalRoomsCount;
            this.NumberOfMovesCount = settings.NumberOfMovesCount;
            this.TimesCarriedCount = settings.TimesCarriedCount;
            this.FoundTheCrown = settings.FoundTheCrown;
            this.Survived = settings.Survived;
            this.DifficultySetting = settings.DifficultySetting;
            this.TotalScore = settings.TotalScore;

            this.UpdateText();
            this.Enable();
        }

        /// <summary>
        /// Updates the text.
        /// </summary>
        private void UpdateText()
        {
            this.UpdateMuglumpsSlainText();
            this.UpdateArrowsFiredText();
            this.UpdateCoverScentText();
            this.UpdateRoomsVisitedText();
            this.UpdateNumberOfMovesText();
            this.UpdateTimesCarriedText();
            this.UpdateFoundTheCrownText();
            this.UpdateSurvivedText();
            this.UpdateDifficultySettingText();
            this.UpdateTotalScoreText();
        }

        /// <summary>
        /// Updates the muglumps slain text.
        /// </summary>
        private void UpdateMuglumpsSlainText()
        {
            this.RedMuglumpsSlainTextContainer.Value = $"{this.RedMuglumpsSlainCount}/{this.TotalRedMuglumpsCount}";
            this.BlackMuglumpsSlainTextContainer.Value = $"{this.BlackMuglumpsSlainCount}/{this.TotalBlackMuglumpsCount}";
            this.BlueMuglumpsSlainTextContainer.Value = $"{this.BlueMuglumpsSlainCount}/{this.TotalBlueMuglumpsCount}";
            this.GoldMuglumpsSlainTextContainer.Value = $"{this.GoldMuglumpsSlainCount}/{this.TotalGoldMuglumpsCount}";
            this.SilverbackMuglumpsSlainTextContainer.Value = $"{this.SilverbackMuglumpsSlainCount}/{this.TotalSilverbackMuglumpsCount}";
        }

        /// <summary>
        /// Updates the arrows fired text.
        /// </summary>
        private void UpdateArrowsFiredText()
        {
            this.arrowsFiredTextContainer.Value = $"{this.ArrowsFiredCount}/{this.ArrowsCount}";
            this.FlashArrowsFiredTextContainer.Value = $"{this.FlashArrowsFiredCount}/{this.FlashArrowsCount}";
            this.NetArrowsFiredTextContainer.Value = $"{this.NetArrowsFiredCount}/{this.NetArrowsCount}";
            this.AccuracyTextContainer.Value = $"{100.0f * this.Accuracy:0.00}%";
        }

        private void UpdateCoverScentText()
        {
            this.CoverScentTextContainer.Value = $"{this.CoverScentsUsed}/{this.CoverScentCount}";
        }

        /// <summary>
        /// Updates the rooms visited text.
        /// </summary>
        private void UpdateRoomsVisitedText()
        {
            this.roomsVisitedTextContainer.Value = $"{this.RoomsVisitedCount}/{this.totalRoomsCount}";
        }

        /// <summary>
        /// Updates the number of moves text.
        /// </summary>
        private void UpdateNumberOfMovesText()
        {
            this.numberOfMovesTextContainer.Value = $"{this.NumberOfMovesCount}";
        }

        /// <summary>
        /// Updates the times carried text.
        /// </summary>
        private void UpdateTimesCarriedText()
        {
            this.timesCarriedTextContainer.Value = $"{this.timesCarriedCount}";
        }

        private void UpdateDifficultySettingText()
        {
            this.DifficultyTextContainer.Value = $"{LocalizationSettings.StringDatabase.GetLocalizedString(StringContent.StringContentTable, this.DifficultySetting.ToString())}";
        }

        /// <summary>
        /// Updates the found the crown text.
        /// </summary>
        private void UpdateFoundTheCrownText()
        {
            this.foundTheCrownTextContainer.Value = LocalizationSettings.StringDatabase.GetLocalizedString(StringContent.StringContentTable, this.FoundTheCrown ? "Yes" : "No");
        }

        /// <summary>
        /// Updates the survived text.
        /// </summary>
        private void UpdateSurvivedText()
        {
            this.survivedTextContainer.Value = LocalizationSettings.StringDatabase.GetLocalizedString(StringContent.StringContentTable, this.Survived ? "Yes" : "No");
        }

        /// <summary>
        /// Updates the total score text.
        /// </summary>
        private void UpdateTotalScoreText()
        {
            this.totalScoreTextContainer.Value = $"{this.TotalScore}";
        }

        private void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(FinalReportPanelBehaviour));
        }
    }
}
