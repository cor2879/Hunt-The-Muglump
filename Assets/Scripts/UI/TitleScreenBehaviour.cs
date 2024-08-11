#pragma warning disable CS0649
/**************************************************
 *  TitleScreenBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.InputSystem.EnhancedTouch;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using BeautifulInterface = Interface.Elements.Scripts;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the behaviours for the Title Screen
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(Canvas))]
    public class TitleScreenBehaviour : UIHelperBehaviour, IGameManager
    {
        private Action doOnNextFrame = null;

        /// <summary>
        /// The music manager
        /// </summary>
        [SerializeField]
        private AudioManager musicManager;

        /// <summary>
        /// The sound effect manager
        /// </summary>
        [SerializeField]
        private AudioManager soundEffectManager;

        [SerializeField]
        private CreditsWindowBehaviour creditsCanvas;

        [SerializeField]
        private PanelBehaviour mainButtonsPanel;

        [SerializeField]
        private PanelBehaviour moreButtonsPanel;

        /// <summary>
        /// The start button
        /// </summary>
        [SerializeField]
        private BeautifulInterface.ButtonUI quickStartButton;

        [SerializeField]
        private BeautifulInterface.ButtonUI newGameButton;

        [SerializeField]
        private BeautifulInterface.ButtonUI moreButton;

        [SerializeField]
        private BeautifulInterface.ButtonUI backButton;

        [SerializeField]
        private NewGameMenuBehaviour newGameMenu;

        /// <summary>
        /// The settings button
        /// </summary>
        [SerializeField]
        private BeautifulInterface.ButtonUI settingsButton;

        [SerializeField]
        private BeautifulInterface.ButtonUI badgesButton;

        [SerializeField]
        private CustomModePanelBehaviour customModePanel;

        [SerializeField]
        private NewCustomModePanelBehaviour newCustomModePanel;

        //[SerializeField]
        //private BeautifulInterface.ButtonUI creditsButton;

        /// <summary>
        /// The exit button
        /// </summary>
        [SerializeField]
        private BeautifulInterface.ButtonUI exitButton;

        /// <summary>
        /// The intro behaviour
        /// </summary>
        [SerializeField]
        private IntroBehaviour introBehaviour;

        /// <summary>
        /// The title panel
        /// </summary>
        [SerializeField]
        private UIHelperBehaviour titlePanel;

        /// <summary>
        /// The settings panel
        /// </summary>
        [SerializeField]
        private SettingsPanelBehaviourBase settingsPanel;

        /// <summary>
        /// The Old School Games panel
        /// </summary>
        [SerializeField]
        private OldSchoolGamesPanelBehaviour oldSchoolGamesPanel;

        [SerializeField]
        private BadgesPanelBehaviour badgesPanel;

        [SerializeField]
        private TextLabelBehaviour versionText;

        [SerializeField]
        private ScoreRecordHistoryPanelBehaviour scoreRecordHistoryPanel;

        [SerializeField]
        private BeautifulInterface.ButtonUI historyButton;

        /// <summary>
        /// Determines whether or not a listener needs to be added to the on screen buttons
        /// </summary>
        private bool addListener;

        /// <summary>
        /// The instance
        /// </summary>
        private static TitleScreenBehaviour instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static TitleScreenBehaviour Instance
        {
            get => instance;
            private set => instance = value;
        }

        public CreditsWindowBehaviour CreditsCanvas
        {
            get
            {
                this.ValidateUnityEditorParameter(this.creditsCanvas, nameof(this.creditsCanvas));

                return this.creditsCanvas;
            }
        }

        public OldSchoolGamesPanelBehaviour OldSchoolGamesPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.oldSchoolGamesPanel, nameof(this.oldSchoolGamesPanel));

                return this.oldSchoolGamesPanel;
            }
        }

        public PanelBehaviour MainButtonsPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.mainButtonsPanel, nameof(this.mainButtonsPanel));

                return this.mainButtonsPanel;
            }
        }

        /// <summary>
        /// Gets the music manager.
        /// </summary>
        /// <value>
        /// The music manager.
        /// </value>
        public AudioManager MusicManager
        {
            get
            {
                this.ValidateUnityEditorParameter(this.musicManager, nameof(this.musicManager));

                return this.musicManager;
            }
        }

        /// <summary>
        /// Gets the sound effect manager.
        /// </summary>
        /// <value>
        /// The sound effect manager.
        /// </value>
        public AudioManager SoundEffectManager
        {
            get
            {
                this.ValidateUnityEditorParameter(this.soundEffectManager, nameof(this.soundEffectManager));

                return this.soundEffectManager;
            }
        }

        /// <summary>
        /// Gets the title panel.
        /// </summary>
        /// <value>
        /// The title panel.
        /// </value>
        public UIHelperBehaviour TitlePanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.titlePanel, nameof(this.titlePanel));

                return this.titlePanel;
            }
        }

        /// <summary>
        /// Gets the intro behaviour.
        /// </summary>
        /// <value>
        /// The intro behaviour.
        /// </value>
        public IntroBehaviour IntroBehaviour
        {
            get
            {
                this.ValidateUnityEditorParameter(this.introBehaviour, nameof(this.introBehaviour));

                return this.introBehaviour;
            }
        }

        public TextLabelBehaviour VersionText
        {
            get
            {
                this.ValidateUnityEditorParameter(this.versionText, nameof(this.versionText));

                return this.versionText;
            }
        }

        public BadgesPanelBehaviour BadgesPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.badgesPanel, nameof(this.badgesPanel));

                return this.badgesPanel;
            }
        }

        public CustomModePanelBehaviour CustomModePanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.customModePanel, nameof(this.customModePanel));

                return this.customModePanel;
            }
        }

        public NewCustomModePanelBehaviour NewCustomModePanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.newCustomModePanel, nameof(this.newCustomModePanel));

                return this.newCustomModePanel;
            }
        }

        public ScoreRecordHistoryPanelBehaviour ScoreRecordHistoryPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.scoreRecordHistoryPanel, nameof(this.scoreRecordHistoryPanel));

                return this.scoreRecordHistoryPanel;
            }
        }

        public PanelBehaviour MoreButtonsPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.moreButtonsPanel, nameof(this.moreButtonsPanel));

                return this.moreButtonsPanel;
            }
        }

        public SettingsPanelBehaviourBase SettingsPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.settingsPanel, nameof(this.settingsPanel));

                return this.settingsPanel;
            }
        }

        /// <summary>
        /// Gets the start button.
        /// </summary>
        /// <value>
        /// The start button.
        /// </value>
        public BeautifulInterface.ButtonUI QuickStartButton 
        {
            get
            {
                this.ValidateUnityEditorParameter(this.quickStartButton, nameof(this.quickStartButton));

                return this.quickStartButton;
            }
        }

        public BeautifulInterface.ButtonUI NewGameButton
        {
            get
            {
                this.ValidateUnityEditorParameter(this.newGameButton, nameof(this.newGameButton));

                return this.newGameButton;
            }
        }

        public BeautifulInterface.ButtonUI MoreButton
        {
            get
            {
                this.ValidateUnityEditorParameter(this.moreButton, nameof(this.moreButton));

                return this.moreButton;
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

        /// <summary>
        /// Gets the settings button.
        /// </summary>
        /// <value>
        /// The settings button.
        /// </value>
        public BeautifulInterface.ButtonUI SettingsButton 
        {
            get
            {
                this.ValidateUnityEditorParameter(this.settingsButton, nameof(this.settingsButton));

                return this.settingsButton;
            }
        }

        /// <summary>
        /// Gets the badges button.
        /// </summary>
        /// <value>
        /// The badges button.
        /// </value>
        /// <exception cref="UIException">badgesButton</exception>
        public BeautifulInterface.ButtonUI BadgesButton
        {
            get
            {
                this.ValidateUnityEditorParameter(this.badgesButton, nameof(this.badgesButton));

                return this.badgesButton;
            }
        }

        public BeautifulInterface.ButtonUI HistoryButton
        {
            get
            {
                this.ValidateUnityEditorParameter(this.historyButton, nameof(this.historyButton));

                return this.historyButton;
            }
        }

        //public BeautifulInterface.ButtonUI CreditsButton
        //{
        //    get
        //    {
        //        this.ValidateUnityEditorParameter(this.creditsButton, nameof(this.creditsButton));

        //        return this.creditsButton;
        //    }
        //}

        public NewGameMenuBehaviour NewGameMenu
        {
            get
            {
                this.ValidateUnityEditorParameter(this.newGameMenu, nameof(this.newGameMenu));

                return this.newGameMenu;
            }
        }

        /// <summary>
        /// Gets the exit button.
        /// </summary>
        /// <value>
        /// The exit button.
        /// </value>
        public BeautifulInterface.ButtonUI ExitButton 
        {
            get
            {
                this.ValidateUnityEditorParameter(this.exitButton, nameof(this.exitButton));

                return this.exitButton;
            }
        }

        /// <summary>
        /// Executes when this instance is awakened.
        /// </summary>
        private void Awake()
        {
#if DEBUG
            //foreach (var badge in Badge.GetStaticBadges())
            //{
            //    badge.DeleteBadge();
            //}
#endif

            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// Executes during the Start event of the GameObject life cycle.
        /// </summary>
        public void Start()
        {
            EnhancedTouchSupport.Enable();
            SoundClips.CurrentBGM = SoundClips.Playlist[UnityEngine.Random.Range(0, SoundClips.Playlist.Length)];
            this.SoundEffectManager.Volume = Settings.SoundEffectVolume;
            this.MusicManager.SetBackgroundMusic(SoundClips.CurrentBGM);
            this.OldSchoolGamesPanel.Enable();
            this.TitlePanel.Disable();
            this.IntroBehaviour.Disable();
            this.MoreButtonsPanel.Hide();
            this.NewGameMenu.Hide();
            this.NewCustomModePanel.Hide();
            this.SettingsPanel.Disable();
            this.BadgesPanel.Disable();
            this.ScoreRecordHistoryPanel.Disable();
            this.CustomModePanel.Disable();
            this.VersionText.Text = $"v {Constants.Version}";
        }

        /// <summary>
        /// Updates this instance when the Unity Engine updates each frame.
        /// </summary>
        public void Update()
        {
            if (this.addListener)
            {
                if (this.QuickStartButton.isActiveAndEnabled)
                {
                    this.QuickStartButton.onClick.AddListener(this.QuickStartGame);
                    this.QuickStartButton.Select();
                }

                if (this.NewGameButton.isActiveAndEnabled)
                {
                    this.NewGameButton.onClick.AddListener(this.LaunchNewGameMenu);
                }

                if (this.SettingsButton.isActiveAndEnabled)
                {
                    this.SettingsButton.onClick.AddListener(this.LaunchSettings);
                }

                if (this.BadgesButton.isActiveAndEnabled)
                {
                    this.BadgesButton.onClick.AddListener(this.LaunchBadges);
                }

                if (this.HistoryButton.isActiveAndEnabled)
                {
                    this.HistoryButton.onClick.AddListener(
                        () =>
                        {
                            StartCoroutine(nameof(this.WaitForPredicateToBeFalseThenDoAction),
                                new WaitAction(
                                    () => InputExtension.IsSubmitPressed(),
                                    this.LaunchScoreRecordHistory));
                        });
                }

                //if (this.CreditsButton.isActiveAndEnabled)
                //{
                //    this.CreditsButton.onClick.AddListener(this.ShowCredits);
                //}

                if (this.MoreButton.isActiveAndEnabled)
                {
                    this.MoreButton.onClick.AddListener(this.CloseMainButtonsAndOpenMoreButtons);
                }

                if (this.BackButton.isActiveAndEnabled)
                {
                    this.BackButton.onClick.AddListener(this.CloseMoreButtonsAndOpenMainButtons);
                }

                if (this.ExitButton.isActiveAndEnabled)
                {
                    this.ExitButton.onClick.AddListener(this.ExitGame);
                }

                this.addListener = false;
            }

            InputExtension.HideMouseIfGamepadIsPresent();
        }

        public void FixedUpdate()
        {
            this.doOnNextFrame?.Invoke();
        }

        /// <summary>
        /// Starts a new game with the same settings as the previous game
        /// </summary>
        public void QuickStartGame()
        {
            this.QuickStartButton.Normal();
            SceneManager.LoadScene(Constants.PrimaryScene);
        }

        public void CloseMainButtonsAndOpenMoreButtons()
        {
            this.doOnNextFrame = () =>
            {
                this.MainButtonsPanel.Hide();
                this.doOnNextFrame = () =>
                {
                    this.MoreButtonsPanel.Show();
                    this.doOnNextFrame = null;
                };
            };
        }

        public void CloseMoreButtonsAndOpenMainButtons()
        {
            this.doOnNextFrame = () =>
            {
                this.MoreButtonsPanel.Hide();
                this.doOnNextFrame = () =>
                {
                    this.MainButtonsPanel.Show();
                    this.doOnNextFrame = null;
                };
            };
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void StartGame()
        {
            this.QuickStartButton.Normal();

            if (Settings.Difficulty.Setting == DifficultySetting.Custom)
            {
                this.NewCustomModePanel.Open();
            }
            else
            {
                SceneManager.LoadScene(Constants.PrimaryScene);
            }
        }

        private void LaunchNewGameMenu()
        {
            this.NewGameButton.Normal();
            this.MoreButtonsPanel.Hide();
            this.MainButtonsPanel.Hide();
            this.MainButtonsPanel.GetComponent<ButtonsPanelBehaviour>().Deactivate();
            EventSystem.current.SetSelectedGameObject(null);
            this.NewGameMenu.Show();
        }

        /// <summary>
        /// Launches the settings.
        /// </summary>
        private void LaunchSettings()
        {
            this.MoreButtonsPanel.Hide();
            // EventSystem.current.SetSelectedGameObject(null);
            this.SettingsPanel.Enable();
        }

        /// <summary>
        /// Launches the badges.
        /// </summary>
        private void LaunchBadges()
        {
            this.MoreButtonsPanel.Hide();
            this.BadgesPanel.Enable();
        }

        private void LaunchScoreRecordHistory()
        {
            this.MoreButtonsPanel.Hide();
            this.ScoreRecordHistoryPanel.Enable();
        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        private void ExitGame()
        {
            Application.Quit();
        }

        /// <summary>
        /// Begins the game intro.
        /// </summary>
        public void BeginGameIntro()
        {
            this.IntroBehaviour.Enable();
            this.IntroBehaviour.BeginIntro();
        }

        /// <summary>
        /// Finishes the game intro.
        /// </summary>
        public void FinishGameIntro()
        {
            this.OldSchoolGamesPanel.Disable();
            this.IntroBehaviour.Disable();
            this.TitlePanel.Enable();
            this.MainButtonsPanel.Show();
            this.MoreButtonsPanel.Hide();
            this.addListener = true;
        }

        public void ShowCredits()
        {
            this.Disable();
            this.CreditsCanvas.Enable();
        }

        private void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(TitleScreenBehaviour));
        }
    }
}
