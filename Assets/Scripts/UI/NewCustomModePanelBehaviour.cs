#pragma warning disable CS0649
/**************************************************
 *  CustomModePanelBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using BeautifulInterface = Interface.Elements.Scripts;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Extensions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(ButtonsPanelBehaviour))]
    [RequireComponent(typeof(CanvasGroup))]
    public class NewCustomModePanelBehaviour : PanelBehaviour
    {
        #region Private Fields

        [SerializeField]
        private BeautifulInterface.ButtonUI startGameButton;

        [SerializeField]
        private BeautifulInterface.ButtonUI backButton;

        [SerializeField]
        private BeautifulInterface.ButtonUI oldCustomModeButton;

        [SerializeField]
        private SizeSelectorBehaviour dungeonSizeSelector;

        [SerializeField]
        private AmountSelectorBehaviour redMuglumpAmountSelector;

        [SerializeField]
        private AmountSelectorBehaviour darkMuglumpAmountSelector;

        [SerializeField]
        private AmountSelectorBehaviour hunterMuglumpAmountSelector;

        [SerializeField]
        private AmountSelectorBehaviour silverbackMuglumpAmountSelector;

        [SerializeField]
        private AmountSelectorBehaviour batAmountSelector;

        [SerializeField]
        private AmountSelectorBehaviour pitAmountSelector;

        [SerializeField]
        private AmountSelectorBehaviour arrowAmountSelector;

        [SerializeField]
        private AmountSelectorBehaviour flashArrowAmountSelector;

        [SerializeField]
        private AmountSelectorBehaviour netArrowAmountSelector;

        [SerializeField]
        private AmountSelectorBehaviour eauDuMuglumpAmountSelector;

        [SerializeField]
        private AmountSelectorBehaviour bearTrapAmountSelector;

        [SerializeField, ReadOnly]
        private SizeSelectorBehaviour[] sizeSelectors;

        [SerializeField, ReadOnly]
        private AmountSelectorBehaviour[] occupantAmountSelectors;

        [SerializeField, ReadOnly]
        private AmountSelectorBehaviour[] itemAmountSelectors;

        #endregion

        #region Buttons

        public BeautifulInterface.ButtonUI StartGameButton
        {
            get
            {
                this.ValidateUnityEditorParameter(this.startGameButton, nameof(this.startGameButton));

                return this.startGameButton;
            }
        }

        public BeautifulInterface.ButtonUI BackButton
        {
            get
            {
                this.ValidateUnityEditorParameter(this.backButton, nameof(this.backButton));

                return this.backButton;
            }
        }

        public BeautifulInterface.ButtonUI OldCustomModeButton
        {
            get
            {
                this.ValidateUnityEditorParameter(this.oldCustomModeButton, nameof(this.oldCustomModeButton));

                return this.oldCustomModeButton;
            }
        }

        #endregion

        #region Selectors

        #region Size Selectors

        public SizeSelectorBehaviour DungeonSizeSelector
        {
            get
            {
                this.ValidateUnityEditorParameter(this.dungeonSizeSelector, nameof(this.dungeonSizeSelector));

                return this.dungeonSizeSelector;
            }
        }

        #endregion

        #region Occupant Amount Selectors

        public AmountSelectorBehaviour RedMuglumpAmountSelector
        {
            get
            {
                this.ValidateUnityEditorParameter(this.redMuglumpAmountSelector, nameof(this.redMuglumpAmountSelector));

                return this.redMuglumpAmountSelector;
            }
        }

        public AmountSelectorBehaviour DarkMuglumpAmountSelector
        {
            get
            {
                this.ValidateUnityEditorParameter(this.darkMuglumpAmountSelector, nameof(this.darkMuglumpAmountSelector));

                return this.darkMuglumpAmountSelector;
            }
        }

        public AmountSelectorBehaviour HunterMuglumpAmountSelector
        {
            get
            {
                this.ValidateUnityEditorParameter(this.hunterMuglumpAmountSelector, nameof(this.hunterMuglumpAmountSelector));

                return this.hunterMuglumpAmountSelector;
            }
        }

        public AmountSelectorBehaviour SilverbackMuglumpAmountSelector
        {
            get
            {
                this.ValidateUnityEditorParameter(this.silverbackMuglumpAmountSelector, nameof(this.silverbackMuglumpAmountSelector));

                return this.silverbackMuglumpAmountSelector;
            }
        }

        public AmountSelectorBehaviour BatAmountSelector
        { 
            get
            {
                this.ValidateUnityEditorParameter(this.batAmountSelector, nameof(this.batAmountSelector));

                return this.batAmountSelector;
            }
        }

        public AmountSelectorBehaviour PitAmountSelector
        {
            get
            {
                this.ValidateUnityEditorParameter(this.pitAmountSelector, nameof(this.pitAmountSelector));

                return this.pitAmountSelector;
            }
        }

        #endregion

        #region Item Amount Selectors

        public AmountSelectorBehaviour ArrowAmountSelector
        {
            get
            {
                this.ValidateUnityEditorParameter(this.arrowAmountSelector, nameof(this.arrowAmountSelector));

                return this.arrowAmountSelector;
            }
        }

        public AmountSelectorBehaviour FlashArrowAmountSelector
        {
            get
            {
                this.ValidateUnityEditorParameter(this.flashArrowAmountSelector, nameof(this.flashArrowAmountSelector));

                return this.flashArrowAmountSelector;
            }
        }

        public AmountSelectorBehaviour NetArrowAmountSelector
        {
            get
            {
                this.ValidateUnityEditorParameter(this.netArrowAmountSelector, nameof(this.netArrowAmountSelector));

                return this.netArrowAmountSelector;
            }
        }

        public AmountSelectorBehaviour EauDuMuglumpAmountSelector
        {
            get
            {
                this.ValidateUnityEditorParameter(this.eauDuMuglumpAmountSelector, nameof(this.eauDuMuglumpAmountSelector));

                return this.eauDuMuglumpAmountSelector;
            }
        }

        public AmountSelectorBehaviour BearTrapAmountSelector
        {
            get
            {
                this.ValidateUnityEditorParameter(this.bearTrapAmountSelector, nameof(this.bearTrapAmountSelector));

                return this.bearTrapAmountSelector;
            }
        }

        #endregion

        #endregion

        private SizeSelectorBehaviour[] SizeSelectors { get => this.sizeSelectors; set => this.sizeSelectors = value; }

        private AmountSelectorBehaviour[] OccupantAmountSelectors { get => this.occupantAmountSelectors; set => this.occupantAmountSelectors = value; }

        private AmountSelectorBehaviour[] ItemAmountSelectors { get => this.itemAmountSelectors; set => this.itemAmountSelectors = value; }

        private static Difficulty CustomDifficulty
        {
            get
            {
                return Difficulty.GetDifficulty(DifficultySetting.Custom);
            }

            set
            {
                Difficulty.SetCustomDifficulty(value);
            }
        }

        private static int GetSizeThresholdIndex(int size)
        {
            var i = Constants.SizeThresholds.Length - 1;

            for (; i > -1; i--)
            {
                if (size >= Constants.SizeThresholds[i])
                {
                    return i;
                }
            }

            return i;
        }

        private void LoadDungeonSizeSelector()
        {
            var dungeonSizeValue = CustomDifficulty.MinimumRoomCount;
            this.DungeonSizeSelector.Initialize();
            this.DungeonSizeSelector.SelectedIndex = GetSizeThresholdIndex(dungeonSizeValue);
        }

        private void LoadSizeSelectors()
        {
            this.LoadDungeonSizeSelector();
        }

        private void LoadOccupantAmountSelectors()
        {
            var customModeSettings = Settings.CustomModeSettings;

            this.RedMuglumpAmountSelector.Initialize();
            this.RedMuglumpAmountSelector.SelectedIndex = customModeSettings.RedMuglumpAmountSelection;

            this.DarkMuglumpAmountSelector.Initialize();
            this.DarkMuglumpAmountSelector.SelectedIndex = customModeSettings.DarkMuglumpAmountSelection;

            this.HunterMuglumpAmountSelector.Initialize();
            this.HunterMuglumpAmountSelector.SelectedIndex = customModeSettings.HunterMuglumpAmountSelection;

            this.SilverbackMuglumpAmountSelector.Initialize();
            this.SilverbackMuglumpAmountSelector.SelectedIndex = customModeSettings.SilverbackMuglumpAmountSelection;

            this.BatAmountSelector.Initialize();
            this.BatAmountSelector.SelectedIndex = customModeSettings.BatAmountSelection;

            this.PitAmountSelector.Initialize();
            this.PitAmountSelector.SelectedIndex = customModeSettings.PitAmountSelection;
        }

        /***********************************************************************************
         * The old way that didn't quite work but I'm keeping this code because some of it is interesting
         * 
         *   var dungeonSizeValue = Constants.SizeOptions[GetSizeThresholdIndex(CustomDifficulty.MinimumRoomCount)];
         *   var dungeonSizeThreshold = Constants.SizeDictionary[dungeonSizeValue];
         *   var maxThresholdIndex = Constants.AmountOccupantPercentThresholds.Max();
         *   var weight = Constants.AmountOccupantPercentThresholds.Sum();
         *
         *   var maxOccupantAmounts = new Dictionary<int, string>
         *   {
         *       { 0, Constants.None },
         *       { 1, Constants.One }
         *   };
         *
         *   foreach (var threshold in Constants.AmountOptions.Skip(2))
         *   {
         *       maxOccupantAmounts[(int)((Constants.OccupantAmountPercentageThresholds[threshold] / weight) * maxThresholdIndex * dungeonSizeThreshold)] = threshold;
         *   }
         *
         *   this.RedMuglumpAmountSelector.SelectedIndex = this.GetWeightedThreshold(maxOccupantAmounts, CustomDifficulty.MuglumpCount);
         *   this.DarkMuglumpAmountSelector.SelectedIndex = this.GetWeightedThreshold(maxOccupantAmounts, CustomDifficulty.BlackMuglumpCount);
         *   this.HunterMuglumpAmountSelector.SelectedIndex = this.GetWeightedThreshold(maxOccupantAmounts, CustomDifficulty.BlueMuglumpCount);
         *   this.SilverbackMuglumpAmountSelector.SelectedIndex = this.GetWeightedThreshold(maxOccupantAmounts, CustomDifficulty.SilverbackMuglumpCount);
         *   this.PitAmountSelector.SelectedIndex = this.GetWeightedThreshold(maxOccupantAmounts, CustomDifficulty.PitCount);
         *   this.BatAmountSelector.SelectedIndex = this.GetWeightedThreshold(maxOccupantAmounts, CustomDifficulty.BatCount);
         *
         **************************************************************************************/

        private void LoadItemAmountSelectors()
        {
            var customModeSettings = Settings.CustomModeSettings;

            this.ArrowAmountSelector.Initialize();
            this.ArrowAmountSelector.SelectedIndex = customModeSettings.ArrowAmountSelection;

            this.FlashArrowAmountSelector.Initialize();
            this.FlashArrowAmountSelector.SelectedIndex = customModeSettings.FlashArrowAmountSelection;

            this.NetArrowAmountSelector.Initialize();
            this.NetArrowAmountSelector.SelectedIndex = customModeSettings.NetArrowAmountSelection;

            this.EauDuMuglumpAmountSelector.Initialize();
            this.EauDuMuglumpAmountSelector.SelectedIndex = customModeSettings.EauDuMuglumpAmountSelection;

            this.BearTrapAmountSelector.Initialize();
            this.BearTrapAmountSelector.SelectedIndex = customModeSettings.BearTrapAmountSelection;
        }

        // ****************************************************************************
        // The old way that didn't quite work but I'm keeping this code because it's interesting
        //
        // var dungeonSizeValue = Constants.SizeOptions[this.DungeonSizeSelector.SelectedIndex = GetSizeThresholdIndex(CustomDifficulty.MinimumRoomCount)];
        // var dungeonSizeThreshold = Constants.SizeDictionary[dungeonSizeValue];
        // var maxThresholdIndex = Constants.AmountItemPercentThresholds.Max();
        // var weight = Constants.AmountItemPercentThresholds.Sum();
        // var maxItemAmounts = new Dictionary<int, string>
        // {
        //     { 0, Constants.None },
        //     { 1, Constants.One }
        // };
        //
        // foreach (var threshold in Constants.AmountOptions.Skip(2))
        // {
        //     maxItemAmounts[(int)((Constants.ItemAmountPercentageThresholds[threshold] / weight) * maxThresholdIndex * dungeonSizeThreshold)] = threshold;
        // }
        //
        // this.ArrowAmountSelector.SelectedIndex = this.GetWeightedThreshold(maxItemAmounts, CustomDifficulty.ArrowCount);
        // this.FlashArrowAmountSelector.SelectedIndex = this.GetWeightedThreshold(maxItemAmounts, CustomDifficulty.FlashArrowCount);
        // this.NetArrowAmountSelector.SelectedIndex = this.GetWeightedThreshold(maxItemAmounts, CustomDifficulty.NetArrowCount);
        // this.EauDuMuglumpAmountSelector.SelectedIndex = this.GetWeightedThreshold(maxItemAmounts, CustomDifficulty.CoverScentCount);
        // this.BearTrapAmountSelector.SelectedIndex = this.GetWeightedThreshold(maxItemAmounts, CustomDifficulty.BearTrapCount);
        // ******************************************************************************

        private int GetWeightedThreshold(IDictionary<int, string> amountThresholds, int rawCount)
        {
            var i = amountThresholds.Keys.Count - 1;

            for (; i > -1; i--)
            {
                if (rawCount >= amountThresholds.Keys.ElementAt(i))
                {
                    return i;
                }
            }

            return 0;
        }

        private void CalculateOccupantAmounts()
        {
            var dungeonSizeValue = this.DungeonSizeSelector.SelectedValue;
            var dungeonSizeThreshold = Constants.SizeDictionary[dungeonSizeValue];

            var maxThresholdIndex = this.OccupantAmountSelectors.Max(selector => selector.SelectedIndex);

            var weight = this.OccupantAmountSelectors.Sum(selector => Constants.OccupantAmountPercentageThresholds[selector.SelectedValue]);

            CustomDifficulty.MuglumpCount = this.RedMuglumpAmountSelector.SelectedIndex <= 1 ? this.RedMuglumpAmountSelector.SelectedIndex :
                (int)((Constants.OccupantAmountPercentageThresholds[this.RedMuglumpAmountSelector.SelectedValue] / weight) *
                Constants.AmountOccupantPercentThresholds.Last() * dungeonSizeThreshold);

            CustomDifficulty.BlackMuglumpCount = this.DarkMuglumpAmountSelector.SelectedIndex <= 1 ? this.DarkMuglumpAmountSelector.SelectedIndex :
                ((int)((Constants.OccupantAmountPercentageThresholds[this.DarkMuglumpAmountSelector.SelectedValue] / weight) *
                Constants.AmountOccupantPercentThresholds.Last() * dungeonSizeThreshold));

            CustomDifficulty.BlueMuglumpCount = this.HunterMuglumpAmountSelector.SelectedIndex <= 1 ? this.HunterMuglumpAmountSelector.SelectedIndex :
                (int)((Constants.OccupantAmountPercentageThresholds[this.HunterMuglumpAmountSelector.SelectedValue] / weight) *
                Constants.AmountOccupantPercentThresholds.Last() * dungeonSizeThreshold);

            CustomDifficulty.SilverbackMuglumpCount = this.SilverbackMuglumpAmountSelector.SelectedIndex <= 1 ? this.SilverbackMuglumpAmountSelector.SelectedIndex :
                ((int)((Constants.OccupantAmountPercentageThresholds[this.SilverbackMuglumpAmountSelector.SelectedValue] / weight) * 
                Constants.AmountOccupantPercentThresholds.Last() * dungeonSizeThreshold));

            CustomDifficulty.PitCount = this.PitAmountSelector.SelectedIndex <= 1 ? this.PitAmountSelector.SelectedIndex :
                (int)((Constants.OccupantAmountPercentageThresholds[this.PitAmountSelector.SelectedValue] / weight) * 
                Constants.AmountOccupantPercentThresholds.Last() * dungeonSizeThreshold);

            CustomDifficulty.BatCount = this.BatAmountSelector.SelectedIndex <= 1 ? this.BatAmountSelector.SelectedIndex :
                (int)((Constants.OccupantAmountPercentageThresholds[this.BatAmountSelector.SelectedValue] / weight) * 
                Constants.AmountOccupantPercentThresholds.Last() * dungeonSizeThreshold);
        }

        private void CalculateItemAmounts()
        {
            var dungeonSizeValue = this.DungeonSizeSelector.SelectedValue;
            var dungeonSizeThreshold = Constants.SizeDictionary[dungeonSizeValue];

            var maxThresholdIndex = this.ItemAmountSelectors.Max(selector => selector.SelectedIndex);

            var weight = this.ItemAmountSelectors.Sum(selector => Constants.ItemAmountPercentageThresholds[selector.SelectedValue]);

            CustomDifficulty.ArrowCount = this.ArrowAmountSelector.SelectedIndex <= 1 ? this.ArrowAmountSelector.SelectedIndex :
                (int)((Constants.ItemAmountPercentageThresholds[this.ArrowAmountSelector.SelectedValue] / weight) *
                Constants.AmountItemPercentThresholds.Last() * dungeonSizeThreshold);

            CustomDifficulty.FlashArrowCount = this.FlashArrowAmountSelector.SelectedIndex <= 1 ? this.FlashArrowAmountSelector.SelectedIndex :
                (int)((Constants.ItemAmountPercentageThresholds[this.FlashArrowAmountSelector.SelectedValue] / weight) *
                Constants.AmountItemPercentThresholds.Last() * dungeonSizeThreshold);

            CustomDifficulty.NetArrowCount = this.NetArrowAmountSelector.SelectedIndex <= 1 ? this.NetArrowAmountSelector.SelectedIndex :
                (int)((Constants.ItemAmountPercentageThresholds[this.NetArrowAmountSelector.SelectedValue] / weight) *
                Constants.AmountItemPercentThresholds.Last() * dungeonSizeThreshold);

            CustomDifficulty.CoverScentCount = this.EauDuMuglumpAmountSelector.SelectedIndex <= 1 ? this.EauDuMuglumpAmountSelector.SelectedIndex :
                (int)((Constants.ItemAmountPercentageThresholds[this.EauDuMuglumpAmountSelector.SelectedValue] / weight) *
                Constants.AmountItemPercentThresholds.Last() * dungeonSizeThreshold);

            CustomDifficulty.BearTrapCount = this.BearTrapAmountSelector.SelectedIndex <= 1 ? this.BearTrapAmountSelector.SelectedIndex :
                (int)((Constants.ItemAmountPercentageThresholds[this.BearTrapAmountSelector.SelectedValue] / weight) *
                Constants.AmountItemPercentThresholds.Last() * dungeonSizeThreshold);
        }

        private void LoadAmountSelectors()
        {
            this.LoadOccupantAmountSelectors();
            this.LoadItemAmountSelectors();
        }

        private void LoadValues()
        {
            this.LoadSizeSelectors();
            this.LoadAmountSelectors();
        }

        private void SaveSelections()
        {
            CustomDifficulty.MinimumRoomCount = Constants.SizeDictionary[this.DungeonSizeSelector.SelectedValue];

            this.CalculateOccupantAmounts();
            this.CalculateItemAmounts();
        }

        public void SetUpCustomMode()
        {
            TitleScreenBehaviour.Instance.MainButtonsPanel.Hide();
            this.BackButton.Select();
            this.LoadValues();
        }

        public void SaveSettingsAndStartGame()
        {
            this.SaveSelections();
            Difficulty.SaveCustomDifficulty();

            Settings.CustomModeSettings = new CustomModeSettings()
            {
                DungeonSizeSelection = this.DungeonSizeSelector.SelectedIndex,
                RedMuglumpAmountSelection = this.RedMuglumpAmountSelector.SelectedIndex,
                DarkMuglumpAmountSelection = this.DarkMuglumpAmountSelector.SelectedIndex,
                HunterMuglumpAmountSelection = this.HunterMuglumpAmountSelector.SelectedIndex,
                SilverbackMuglumpAmountSelection = this.SilverbackMuglumpAmountSelector.SelectedIndex,
                BatAmountSelection = this.BatAmountSelector.SelectedIndex,
                PitAmountSelection = this.PitAmountSelector.SelectedIndex,
                ArrowAmountSelection = this.ArrowAmountSelector.SelectedIndex,
                FlashArrowAmountSelection = this.FlashArrowAmountSelector.SelectedIndex,
                NetArrowAmountSelection = this.NetArrowAmountSelector.SelectedIndex,
                EauDuMuglumpAmountSelection = this.EauDuMuglumpAmountSelector.SelectedIndex,
                BearTrapAmountSelection = this.BearTrapAmountSelector.SelectedIndex
            };

            Settings.SaveSettings();
            SceneManager.LoadScene(Constants.PrimaryScene);
        }

        public void Open()
        {
            this.Initialize();
            base.Show();
        }

        public void Close()
        {
            this.Hide();
            TitleScreenBehaviour.Instance.MainButtonsPanel.Show();
        }

        public void LaunchOldCustomModeWindow()
        {
            TitleScreenBehaviour.Instance.CustomModePanel.SetUpCustomMode();
            this.Hide();
        }

        public void Start()
        {
            this.StartGameButton.onClick.AddListener(this.SaveSettingsAndStartGame);
            this.BackButton.onClick.AddListener(this.Close);
            this.OldCustomModeButton.onClick.AddListener(this.LaunchOldCustomModeWindow);
        }

        private void Initialize()
        {
            if (this.SizeSelectors == null || !this.SizeSelectors.Any())
            {
                this.SizeSelectors = new[] { this.DungeonSizeSelector };
            }

            if (this.OccupantAmountSelectors == null || !this.OccupantAmountSelectors.Any())
            {
                this.OccupantAmountSelectors = new[]
                {
                    this.RedMuglumpAmountSelector,
                    this.DarkMuglumpAmountSelector,
                    this.HunterMuglumpAmountSelector,
                    this.SilverbackMuglumpAmountSelector,
                    this.BatAmountSelector,
                    this.PitAmountSelector
                };
            }

            if (this.ItemAmountSelectors == null || !this.ItemAmountSelectors.Any())
            {
                this.ItemAmountSelectors = new[]
                {
                    this.ArrowAmountSelector,
                    this.FlashArrowAmountSelector,
                    this.NetArrowAmountSelector,
                    this.EauDuMuglumpAmountSelector,
                    this.BearTrapAmountSelector
                };
            }

            this.LoadValues();
        }

        private void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(NewCustomModePanelBehaviour));
        }
    }
}
