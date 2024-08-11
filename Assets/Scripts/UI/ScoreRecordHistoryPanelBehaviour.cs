#pragma warning disable CS0649
/**************************************************
 *  ScoreRecordHistoryPanelBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class ScoreRecordHistoryPanelBehaviour
        : UIHelperBehaviour
    {
        [SerializeField]
        private Button backButton;

        [SerializeField]
        private ButtonsPanelBehaviour buttonsPanel;

        public ButtonsPanelBehaviour ButtonsPanel
        {
            get
            {
                if (this.buttonsPanel == null)
                {
                    throw new UIException($"The parameter {nameof(this.buttonsPanel)} needs to be set in the Unity Editory for the {nameof(ScoreRecordHistoryPanelBehaviour)}.");
                }

                return this.buttonsPanel;
            }
        }

        public Button BackButton
        {
            get
            {
                if (this.backButton == null)
                {
                    throw new UIException($"The parameter {nameof(this.backButton)} needs to be set in the Unity Editor for the {nameof(ScoreRecordHistoryPanelBehaviour)}.");
                }

                return this.backButton;
            }
        }

        public void Start()
        {
            this.backButton.onClick.AddListener(this.BackButtonClicked);
        }

        public void OnEnable()
        {
            this.ButtonsPanel.Activate();
            this.ButtonsPanel.DefaultButton.Select();
        }

        public void Activate()
        {
            this.ButtonsPanel.Activate();
        }

        public void Deactivate()
        {
            this.ButtonsPanel.Deactivate();
        }

        private void BackButtonClicked()
        {
            StartCoroutine(
                nameof(this.WaitForPredicateToBeFalseThenDoAction),
                new WaitAction(
                    () => InputExtension.IsSubmitPressed(),
                    () =>
                    {
                        this.Disable();
                        TitleScreenBehaviour.Instance.TitlePanel.Enable();
                        TitleScreenBehaviour.Instance.MoreButtonsPanel.Show();
                    }));

            this.ButtonsPanel.Deactivate();
        }
    }
}
