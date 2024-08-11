/**************************************************
 *  CustomModeParameterSliderBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Collections;
    using System.Reflection;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(UISliderPanelBehaviour))]
    public class CustomModeParameterSliderBehaviour : MonoBehaviour
    {
        private static Difficulty customDifficulty;

        private UISliderPanelBehaviour uISliderPanel;

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

        private UISliderPanelBehaviour UISliderPanel
        {
            get
            {
                if (this.uISliderPanel == null)
                {
                    this.uISliderPanel = this.GetComponent<UISliderPanelBehaviour>();
                }

                return this.uISliderPanel;
            }
        }

        private void UpdateProperty(float value)
        {
            UpdateCustomDifficulty(value);
        }

        private void UpdateCustomDifficulty(float value)
        {
            var intValue = (int)value;
            var customDifficulty = CustomDifficulty;

            typeof(Difficulty).GetProperty(this.UISliderPanel.Tag).SetValue(customDifficulty, intValue);

            CustomDifficulty = customDifficulty;

            if (Settings.Difficulty.Setting == CustomDifficulty.Setting)
            {
                Settings.Difficulty = CustomDifficulty;
            }
        }

        public void Start()
        {
            this.UISliderPanel.OnValueChanged.AddListener(this.UpdateProperty);
            this.Initialize();
        }

        public void Initialize()
        {
            if (CustomDifficulty != null)
            {
                this.UISliderPanel.Value = (int)typeof(Difficulty).GetProperty(this.UISliderPanel.Tag).GetValue(CustomDifficulty);
            }
        }
    }
}
