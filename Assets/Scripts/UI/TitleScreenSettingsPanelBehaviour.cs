#pragma warning disable CS0649
/**************************************************
 *  SettingsPanelBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;

    /// <summary>
    /// Defines the behaviours for the settings panel
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.UIHelperBehaviour" />
    public class TitleScreenSettingsPanelBehaviour : SettingsPanelBehaviourBase
    {
        #region Fields

        /// <summary>
        /// The difficulty dropdown
        /// </summary>
        [SerializeField]
        private Dropdown difficultyDropdown;

        /// <summary>
        /// The survival mode toggle
        /// </summary>
        [SerializeField]
        private Toggle survivalModeToggle;

        [SerializeField]
        private Toggle silverbackMuglumpsToggle;

        #endregion

        #region Properties

        private Dropdown DifficultyDropdown
        {
            get
            {
                ValidateUnityEditorParameter(this.difficultyDropdown, nameof(this.difficultyDropdown), nameof(TitleScreenSettingsPanelBehaviour));

                return this.difficultyDropdown;
            }
        }

        private Toggle SurvivalModeToggle
        {
            get
            {
                ValidateUnityEditorParameter(this.survivalModeToggle, nameof(this.survivalModeToggle), nameof(TitleScreenSettingsPanelBehaviour));

                return this.survivalModeToggle;
            }
        }

        private Toggle SilverbackMuglumpsToggle
        {
            get
            {
                ValidateUnityEditorParameter(this.silverbackMuglumpsToggle, nameof(this.silverbackMuglumpsToggle), nameof(TitleScreenSettingsPanelBehaviour));

                return this.silverbackMuglumpsToggle;
            }
        }

        #endregion

        protected override void Save()
        {
            Settings.Difficulty = Difficulty.GetDifficulty(((OptionData<DifficultySetting>)this.DifficultyDropdown.options[this.DifficultyDropdown.value]).Value);
            Settings.SurvivalMode = this.SurvivalModeToggle.isOn;
            Settings.EnableSilverbackMuglumps = this.SilverbackMuglumpsToggle.isOn;

            base.Save();

            TitleScreenBehaviour.Instance.TitlePanel.Enable();
            TitleScreenBehaviour.Instance.MoreButtonsPanel.Show();

        }

        protected override void Cancel()
        {
            base.Cancel();
            TitleScreenBehaviour.Instance.TitlePanel.Enable();
            TitleScreenBehaviour.Instance.MainButtonsPanel.Show();
            TitleScreenBehaviour.Instance.MainButtonsPanel.GetComponent<ButtonsPanelBehaviour>().Activate();
        }

        protected override void Enabled()
        {
            this.PopulateDifficultyDropdown();
            base.Enabled();
        }

        /// <summary>
        /// Populates the difficulty dropdown.
        /// </summary>
        private void PopulateDifficultyDropdown()
        {
            this.DifficultyDropdown.ClearOptions();

            var difficulties = Difficulty.GetDifficulties();
            var options = new List<Dropdown.OptionData>(difficulties.Length);

            foreach (var difficulty in difficulties)
            {
                options.Add(new OptionData<DifficultySetting>()
                {
                    text = difficulty.ToString(),
                    Value = difficulty.Setting
                });
            }

            this.DifficultyDropdown.AddOptions(options);

            this.DifficultyDropdown.value = (int)Settings.Difficulty.Setting;
        }

        /// <summary>
        /// Populates the survival mode toggle.
        /// </summary>
        private void PopulateSurvivalModeToggle()
        {
            this.SurvivalModeToggle.isOn = Settings.SurvivalMode;
        }

        #region Unity Life Cycle Methods

        public void FixedUpdate()
        {
            var toggles = this.difficultyDropdown.GetComponentsInChildren<ScrollToViewBehaviour>();
            var scrollbar = this.difficultyDropdown.GetComponentInChildren<OnSelectScrollBehaviour>();

            if (toggles != null && scrollbar != null)
            {
                for (var i = 0; i < toggles.Length; i++)
                {
                    toggles[i].Scrollbar = scrollbar;
                    toggles[i].ScrollMapPosition = i;
                }

                scrollbar.RowCount = toggles.Length;
                scrollbar.StepHeight = 20.0f;
                scrollbar.ViewPortHeight = 150.0f;
            }
        }

        public override void Start()
        {
            this.PopulateSurvivalModeToggle();
            base.Start();
        }

        #endregion
    }
}
