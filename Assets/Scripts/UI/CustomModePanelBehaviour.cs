#pragma warning disable CS0649
/**************************************************
 *  CustomModePanelBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Collections;

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Extensions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class CustomModePanelBehaviour : UIHelperBehaviour
    {
        [SerializeField]
        private Button saveButton;

        [SerializeField]
        private Button cancelButton;

        [SerializeField]
        private CustomModeParameterContainerBehaviour parameterContainer;

        public Button SaveButton
        {
            get
            {
                if (this.saveButton == null)
                {
                    throw new UIException($"{nameof(this.saveButton)} needs to be set in the Unity Editor.");
                }

                return this.saveButton;
            }
        }

        public Button CancelButton
        {
            get
            {
                if (this.cancelButton == null)
                {
                    throw new UIException($"{nameof(this.cancelButton)} needs to be set in the Unity Editor.");
                }

                return this.cancelButton;
            }
        }

        public CustomModeParameterContainerBehaviour ParameterContainer
        {
            get
            {
                if (this.parameterContainer == null)
                {
                    throw new UIException($"{nameof(this.parameterContainer)} needs to be set in the Unity Editor.");
                }

                return this.parameterContainer;
            }
        }

        public void SetUpCustomMode()
        {
            this.Enable();
            this.ParameterContainer.MinimumRoomCountPanel.Slider.Select();
        }

        public void SaveSettingsAndStartGame()
        {
            Difficulty.SaveCustomDifficulty();
            SceneManager.LoadScene(Constants.PrimaryScene);
        }

        public void Close()
        {
            this.Disable();
            TitleScreenBehaviour.Instance.MainButtonsPanel.Show();
            TitleScreenBehaviour.Instance.QuickStartButton.Select();
        }

        public void Start()
        {
            this.SaveButton.onClick.AddListener(this.SaveSettingsAndStartGame);
            this.CancelButton.onClick.AddListener(this.Close);
        }

        public override void Enable()
        {
            base.Enable();
            this.ParameterContainer.Scrollbar.ScrollToTop();
        }
    }
}
