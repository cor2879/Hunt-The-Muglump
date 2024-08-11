#pragma warning disable CS0649
/**************************************************
 *  GameOverScreenBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System;
    using System.Collections;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the behaviours for the Game Over Screen
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.UIHelperBehaviour" />
    public class GameOverScreenBehaviour : UIHelperBehaviour
    {
        private bool lockInput = false;

        /// <summary>
        /// The game over image panel behaviour
        /// </summary>
        [SerializeField]
        private GameOverImagePanelBehaviour gameOverImagePanelBehaviour;

        /// <summary>
        /// The game over text label behaviour
        /// </summary>
        [SerializeField]
        private TextLabelBehaviour gameOverTextLabelBehaviour;

        /// <summary>
        /// The final report panel behavour
        /// </summary>
        [SerializeField]
        private FinalReportPanelBehaviour finalReportPanelBehavour;

        [SerializeField]
        private Button endGameButton;

        [SerializeField]
        private Button newGameButton;

        [SerializeField]
        private Button backButton;

        /// <summary>
        /// Ends the game based on the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void GameOver(GameOverSettings settings)
        {
            Validator.ArgumentIsNotNull(settings, nameof(settings));

            this.gameObject.SetActive(true);

            this.gameOverImagePanelBehaviour.Enable(settings.GameOverCondition);
            this.gameOverTextLabelBehaviour.Enable();
            this.gameOverTextLabelBehaviour.Text = settings.GameOverCondition.Equals(GameOverCondition.Victory) ?
                StringContent.GameOverVictoryText : settings.GameOverCondition.Equals(GameOverCondition.Eaten) ?
                StringContent.GameOverEatenText : settings.GameOverCondition.Equals(GameOverCondition.Fallen) ?
                StringContent.GameOverFallenText : settings.GameOverCondition.Equals(GameOverCondition.Quit)?
                StringContent.GameOverQuitText : string.Empty;
            this.finalReportPanelBehavour.Enable(settings);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.MusicManager.SetBackgroundMusic(SoundClips.DarkDream);
            }
        }

        public void Start()
        {
            InitializeButton(this.newGameButton, this.StartNewGame);
            InitializeButton(this.endGameButton, this.ReturnToMainMenu);
            InitializeButton(this.backButton, this.ReturnToScoreRecordHistory);
        }

        private void Update()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.IsGameOver = true;
            }
        }

        public void StartNewGame()
        {
            SceneManager.LoadScene(Constants.PrimaryScene);
        }

        private void InitializeButton(Button button, UnityAction onClick)
        {
            if (button != null)
            {
                button.onClick.AddListener(onClick);
            }
        }

        /// <summary>
        /// Returns to main menu.
        /// </summary>
        public void ReturnToMainMenu()
        {
            this.endGameButton.onClick.RemoveAllListeners();
            SceneManager.LoadScene(Constants.TitleScreenScene);
        }

        public void ReturnToScoreRecordHistory()
        {
            if (!this.lockInput)
            {
                this.lockInput = true;

                StartCoroutine(
                    nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        () => InputExtension.IsSubmitPressed(),
                        () =>
                        {
                            TitleScreenBehaviour.Instance.ScoreRecordHistoryPanel.Activate();
                            this.lockInput = false;
                            this.Disable();
                        }));
            }
        }
    }
}
