#pragma warning disable CS0649
/**************************************************
 *  GameManager.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System;
    using System.Collections;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.GameplayManagement;
    using OldSchoolGames.HuntTheMuglump.Scripts.Platform;
    using OldSchoolGames.HuntTheMuglump.Scripts.Rules;
    using OldSchoolGames.HuntTheMuglump.Scripts.UI;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// This is a Singleton class and is the central "hub" for managing all state in the game.
    /// Almost any behaviour can be managed or accessed by starting in this class.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class GameManager : MonoBehaviour, IGameManager
    {
        /// <summary>
        /// The lock input
        /// </summary>
        private bool lockInput = false;

        /// <summary>
        /// The camera manager
        /// </summary>
        public CameraManager cameraManager;

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

        /// <summary>
        /// The UI canvas
        /// </summary>
        [SerializeField]
        private Canvas uiCanvas;

        /// <summary>
        /// The main window scrolling helper behaviour
        /// </summary>
        private TextScrollingHelperBehaviour mainWindowScrollingHelperBehaviour;

        /// <summary>
        /// The badge earned panel behaviour
        /// </summary>
        [SerializeField]
        private BadgeEarnedPanelBehaviour badgeEarnedPanelBehaviour;

        /// <summary>
        /// The crown inventory panel
        /// </summary>
        [SerializeField]
        private UIHelperBehaviour crownInventoryPanel;

        [SerializeField]
        private MenuPanelBehaviour menuPanel;

        /// <summary>
        /// The tutorial manager
        /// </summary>
        [SerializeField]
        private TutorialManagerBehaviour tutorialManager;

        [SerializeField]
        private SettingsPanelBehaviourBase settingsPanel;

        /// <summary>
        /// The game pad how to play panel
        /// </summary>
        [SerializeField]
        private DisplayPanelBehaviour gamepadHowToPlayPanel;

        /// <summary>
        /// The keyboard how to play panel
        /// </summary>
        [SerializeField]
        private KeyboardHowToPlayPanelBehaviour keyboardHowToPlayPanel;

        /// <summary>
        /// The game over screen behaviour
        /// </summary>
        [SerializeField]
        private GameOverScreenBehaviour gameOverScreenBehaviour;

        /// <summary>
        /// The message box behaviour
        /// </summary>
        [SerializeField]
        private MessageBoxBehaviour messageBoxBehaviour;

        /// <summary>
        /// The minimap behaviour
        /// </summary>
        [SerializeField]
        private MinimapBehaviour minimapBehaviour;

        /// <summary>
        /// The minimap button
        /// </summary>
        [SerializeField]
        private Button minimapButton;

        /// <summary>
        /// The pause action
        /// </summary>
        [SerializeField]
        private bool pauseAction;

        /// <summary>
        /// The sprite2 d material
        /// </summary>
        [SerializeField]
        private Material sprite2DMaterial;

        [SerializeField, ReadOnly]
        private int redMuglumpCount;

        [SerializeField, ReadOnly]
        private int goldMuglumpCount;

        [SerializeField, ReadOnly]
        private bool hasDefeatedBoss;

        /// <summary>
        /// The bat prefab
        /// </summary>
        public GameObject batPrefab;

        /// <summary>
        /// The flock of bats prefab
        /// </summary>
        public GameObject flockOfBatsPrefab;

        /// <summary>
        /// The clue prefab
        /// </summary>
        public GameObject cluePrefab;

        /// <summary>
        /// The player prefab
        /// </summary>
        public GameObject playerPrefab;

        /// <summary>
        /// The muglump prefab
        /// </summary>
        public GameObject muglumpPrefab;

        /// <summary>
        /// The black muglump prefab
        /// </summary>
        public GameObject blackMuglumpPrefab;

        /// <summary>
        /// The blue muglump prefab
        /// </summary>
        public GameObject blueMuglumpPrefab;

        /// <summary>
        /// The gold muglump prefab
        /// </summary>
        public GameObject goldMuglumpPrefab;

        /// <summary>
        /// The silverback muglump prefab
        /// </summary>
        public GameObject silverbackMuglumpPrefab;

        /// <summary>
        /// The flash arrow prefab
        /// </summary>
        public GameObject flashArrowPrefab;

        /// <summary>
        /// The net arrow prefab
        /// </summary>
        public GameObject netArrowPrefab;

        /// <summary>
        /// The creature net prefab
        /// </summary>
        public GameObject creatureNetPrefab;

        /// <summary>
        /// The net prefab
        /// </summary>
        public GameObject netPrefab;

        /// <summary>
        /// The arrow item prefab
        /// </summary>
        public GameObject arrowItemPrefab;

        /// <summary>
        /// The flash arrow item prefab
        /// </summary>
        public GameObject flashArrowItemPrefab;

        /// <summary>
        /// The net arrow item prefab
        /// </summary>
        public GameObject netArrowItemPrefab;

        /// <summary>
        /// The cover scent item prefab
        /// </summary>
        public GameObject coverScentItemPrefab;

        /// <summary>
        /// The bear trap item prefab
        /// </summary>
        public GameObject bearTrapItemPrefab;

        /// <summary>
        /// The camera target prefab
        /// </summary>
        public GameObject cameraTargetPrefab;

        /// <summary>
        /// The pit prefab
        /// </summary>
        public GameObject pitPrefab;

        /// <summary>
        /// The crown prefab
        /// </summary>
        public GameObject crownPrefab;

        /// <summary>
        /// The entrance prefab
        /// </summary>
        public GameObject entrancePrefab;

        /// <summary>
        /// The darkness prefab
        /// </summary>
        public GameObject darknessPrefab;

        /// <summary>
        /// The blood splatter prefabs
        /// </summary>
        public GameObject[] bloodSplatterPrefabs;

        public Guid SessionId { get; } = Guid.NewGuid();

        public bool IsGameOver { get; set; }

        public CameraManager CameraManager
        {
            get => this.cameraManager;
        }

        /// <summary>
        /// Gets the music manager.
        /// </summary>
        /// <value>
        /// The music manager.
        /// </value>
        public AudioManager MusicManager
        {
            get => this.musicManager;
        }

        /// <summary>
        /// Gets the sound effect manager.
        /// </summary>
        /// <value>
        /// The sound effect manager.
        /// </value>
        public AudioManager SoundEffectManager
        {
            get => this.soundEffectManager;
        }

        /// <summary>
        /// Gets the tutorial manager.
        /// </summary>
        /// <value>
        /// The tutorial manager.
        /// </value>
        /// <exception cref="PrefabNotSetException">tutorialManager</exception>
        public TutorialManagerBehaviour TutorialManager
        {
            get
            {
                if (this.tutorialManager == null)
                {
                    throw new PrefabNotSetException($"The {nameof(this.tutorialManager)} property needs to be set in the Unity Editor.");
                }

                return this.tutorialManager;
            }
        }

        public SettingsPanelBehaviourBase SettingsPanel
        {
            get
            {
                if (this.settingsPanel == null)
                {
                    throw new PrefabNotSetException($"The parameter {nameof(this.settingsPanel)} needs to be set in the Unity Editor.");
                }

                return this.settingsPanel;
            }
        }

        public static GameplayMenuManagerBehaviour GameplayMenuManager { get => GameplayMenuManagerBehaviour.Instance; }

        /// <summary>
        /// Gets the gamepad how to play panel.
        /// </summary>
        /// <value>
        /// The gamepad how to play panel.
        /// </value>
        /// <exception cref="PrefabNotSetException">gamepadHowToPlayPanel</exception>
        public DisplayPanelBehaviour GamepadHowToPlayPanel
        {
            get
            {
                if (this.gamepadHowToPlayPanel == null)
                {
                    throw new PrefabNotSetException($"The {nameof(this.gamepadHowToPlayPanel)} property needs to be set in the Unity Editor.");
                }

                return this.gamepadHowToPlayPanel;
            }
        }

        public KeyboardHowToPlayPanelBehaviour KeyboardHowToPlayPanel
        {
            get
            {
                if (this.keyboardHowToPlayPanel == null)
                {
                    throw new PrefabNotSetException($"The {nameof(this.keyboardHowToPlayPanel)} property needs to be set in the Unity Editor.");
                }

                return this.keyboardHowToPlayPanel;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [pause action].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [pause action]; otherwise, <c>false</c>.
        /// </value>
        public bool PauseAction
        {
            get => this.pauseAction;
            set => this.pauseAction = value;
        }

        /// <summary>
        /// Gets or sets the sprite2 d material.
        /// </summary>
        /// <value>
        /// The sprite2 d material.
        /// </value>
        public Material Sprite2DMaterial
        {
            get => this.sprite2DMaterial;
        }

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        public PlayerBehaviour Player { get; set; }

        /// <summary>
        /// Gets or sets the dungeon.
        /// </summary>
        /// <value>
        /// The dungeon.
        /// </value>
        public DungeonBehaviour Dungeon { get; set; }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static GameManager Instance { get; private set; }

        /// <summary>
        /// Gets the crown inventory panel.
        /// </summary>
        /// <value>
        /// The crown inventory panel.
        /// </value>
        public UIHelperBehaviour CrownInventoryPanel { get => this.crownInventoryPanel; }

        public int RedMuglumpCount { get => this.redMuglumpCount; set => this.redMuglumpCount = value; }

        public int GoldMuglumpCount { get => this.goldMuglumpCount; set => this.goldMuglumpCount = value; }

        public MenuPanelBehaviour MenuPanel
        {
            get
            {
                if (this.menuPanel == null)
                {
                    throw new UIException($"The {nameof(this.menuPanel)} property needs to be set in the Unity Editor.");
                }

                return this.menuPanel;
            }
        }

        public GameOverScreenBehaviour GameOverScreenBehaviour
        {
            get
            {
                return this.gameOverScreenBehaviour;
            }
        }

        /// <summary>
        /// Gets the message box behaviour.
        /// </summary>
        /// <value>
        /// The message box behaviour.
        /// </value>
        public MessageBoxBehaviour MessageBoxBehaviour
        {
            get => this.messageBoxBehaviour;
        }

        /// <summary>
        /// Gets the minimap behaviour.
        /// </summary>
        /// <value>
        /// The minimap behaviour.
        /// </value>
        public MinimapBehaviour Minimap
        {
            get => this.minimapBehaviour;
        }

        /// <summary>
        /// Gets the minimap button.
        /// </summary>
        /// <value>
        /// The minimap button.
        /// </value>
        public Button MinimapButton
        {
            get => this.minimapButton;
        }

        /// <summary>
        /// Gets or sets the value indicating whether or not the boss muglump has been defeated.
        /// </summary>
        public bool HasDefeatedBoss
        {
            get => this.hasDefeatedBoss;
            set => this.hasDefeatedBoss = value;
        }

        /// <summary>
        /// Sets the main window text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetMainWindowText(string text)
        {
            GameplayMenuManagerBehaviour.SetMainTextPanelText(text);
        }

        /// <summary>
        /// Appends the line main window text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void AppendLineMainWindowText(string text)
        {
            GameplayMenuManagerBehaviour.AppendLineMainWindowText(text);
        }

        /// <summary>
        /// Clears the main window text.
        /// </summary>
        public void ClearMainWindowText()
        {
            GameplayMenuManagerBehaviour.ClearMainWindowText();
        }

        /// <summary>
        /// Executes when this instance is awakened.
        /// </summary>
        private void Awake()
        {
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
        private void Start()
        {
            SoundClips.CurrentBGM = SoundClips.Playlist[UnityEngine.Random.Range(0, SoundClips.Playlist.Length)];
            this.SoundEffectManager.Volume = Settings.SoundEffectVolume;
            this.MusicManager.Volume = Settings.MusicVolume;
            this.MusicManager.SetBackgroundMusic(SoundClips.CurrentBGM);
            this.SoundEffectManager.SetBackgroundMusic(SoundClips.AmbientCave);
            this.gameOverScreenBehaviour.Disable();
            this.badgeEarnedPanelBehaviour.Disable();
            this.TutorialManager.Disable();
            this.SettingsPanel.Disable();
            this.GamepadHowToPlayPanel.Disable();
            this.KeyboardHowToPlayPanel.DisplayPanelBehaviour.Disable();
            this.MenuPanel.Disable();
            this.messageBoxBehaviour.Hide(false);
            this.minimapBehaviour.Disable();
            this.IsGameOver = false;

#if DEBUG
#if UNITY_STEAM
            Debug.Log($"Steam App ID: {SteamManager.AppId}");
            Debug.Log($"Steam User Id: {SteamManager.SteamUserId}");
#endif
#endif
        }

        /// <summary>
        /// Executes when each frame is updated by the Unity Engine.
        /// </summary>
        private void Update()
        {
            if (!this.PauseAction && InputExtension.IsOpenMinimapPressed() && !this.Minimap.IsActive() && !this.lockInput)
            {
                this.lockInput = true;

                StartCoroutine(nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        InputExtension.IsOpenMinimapPressed,
                        () =>
                        {
                            GameplayMenuManagerBehaviour.OpenMinimap();
                            this.lockInput = false;
                        }));
            }

            InputExtension.HideMouseIfGamepadIsPresent();
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        /// <param name="gameOverCondition">The game over condition.</param>
        public void GameOver(GameOverCondition gameOverCondition)
        {
            var gameOverSettings = new GameOverSettings
            {
                GameOverCondition = gameOverCondition,
                RedMuglumpsSlainCount = this.Player.KillCount.ContainsKey(typeof(MuglumpBehaviour)) ? this.Player.KillCount[typeof(MuglumpBehaviour)] : 0,
                BlackMuglumpsSlainCount = this.Player.KillCount.ContainsKey(typeof(BlackMuglumpBehaviour)) ? this.Player.KillCount[typeof(BlackMuglumpBehaviour)] : 0,
                GoldMuglumpsSlainCount = this.Player.KillCount.ContainsKey(typeof(GoldMuglumpBehaviour)) ? this.Player.KillCount[typeof(GoldMuglumpBehaviour)] : 0,
                BlueMuglumpsSlainCount = this.Player.KillCount.ContainsKey(typeof(BlueMuglumpBehaviour)) ? this.Player.KillCount[typeof(BlueMuglumpBehaviour)] : 0,
                SilverbackMuglumpsSlainCount = this.Player.KillCount.ContainsKey(typeof(SilverbackMuglumpBehaviour)) ? this.Player.KillCount[typeof(SilverbackMuglumpBehaviour)] : 0,
                TotalRedMuglumpsCount = this.RedMuglumpCount,
                TotalBlackMuglumpsCount = Settings.Difficulty.BlackMuglumpCount,
                TotalBlueMuglumpsCount = Settings.Difficulty.BlueMuglumpCount,
                TotalGoldMuglumpsCount = this.GoldMuglumpCount,
                TotalSilverbackMuglumpsCount = Settings.Difficulty.SilverbackMuglumpCount + (Settings.Difficulty.EnableBossMode ? 1 : 0),
                ArrowsFiredCount = this.Player.ArrowsFiredCount,
                FlashArrowsFiredCount = this.Player.FlashArrowsFiredCount,
                NetArrowsFiredCount = this.Player.NetArrowsFiredCount,
                CoverScentsUsed = this.Player.CoverScentsUsed,
                ArrowsHitCount = this.Player.ArrowsHitCount,
                FlashArrowsHitCount = this.Player.FlashArrowsHitCount,
                NetArrowsHitCount = this.Player.NetArrowsHitCount,
                ArrowsCount = Settings.Difficulty.ArrowCount,
                FlashArrowsCount = Settings.Difficulty.FlashArrowCount,
                NetArrowsCount = Settings.Difficulty.NetArrowCount,
                CoverScentCount = Settings.Difficulty.CoverScentCount,
                RoomsVisitedCount = this.Dungeon.Dungeon.Where(room => room.Visited).Count(),
                TotalRoomsCount = this.Dungeon.Dungeon.Count,
                NumberOfMovesCount = this.Player.MoveCount,
                TimesCarriedCount = this.Player.TimesCarried,
                FoundTheCrown = this.Player.HasCrown,
                Survived = gameOverCondition == GameOverCondition.Victory,
                DifficultySetting = Settings.Difficulty.Setting
            };

            gameOverSettings.TotalScore = this.CalculateTotalScore(gameOverSettings);

            PlatformManager.Instance.UpdateLeaderboardScore(LeaderboardNames.TopScores, gameOverSettings.TotalScore);
            PlatformManager.Instance.UpdateLeaderboardScore(LeaderboardNames.RoomsExplored, Statistic.RoomsExplored.Value);

            this.gameOverScreenBehaviour.GameOver(gameOverSettings);

            foreach (var rule in StaticRules.EndGameRules)
            {
                rule.GameOverSettings = gameOverSettings;
            }

            foreach (var rule in StaticRules.EndGameRules)
            {
                rule.Update();
            }

            Settings.ScoreRecordHistory.Add(new ScoreRecord(new Player() { Name = PlatformManager.UserName }, gameOverSettings.TotalScore, gameOverSettings, DateTime.Now));
            Settings.SaveSettings();
        }

        /// <summary>
        /// Calculates the total score.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public int CalculateTotalScore(GameOverSettings settings)
        {
            const int PointsPerRedMuglumpKilled = 500;
            const int PointsPerGoldMuglumpKilled = 10000;
            const int PointsPerBlackMuglumpKilled = 2000;
            const int PointsPerBlueMuglumpKilled = 3000;
            const int PointsPerSilverbackMuglumpKilled = 10000;
            const int PointsPerVisitedRoom = 10;
            const int PointsPerTimesCarried = 150;
            const int TimesCarriedMaximumMultiplier = 5;
            const int PointsForCrown = 5000;
            const int PointsForSurviving = 5000;

            var totalScore = 0;

            totalScore += MathfExtension.Sum(
                PointsPerRedMuglumpKilled * settings.RedMuglumpsSlainCount,
                PointsPerGoldMuglumpKilled * settings.GoldMuglumpsSlainCount,
                PointsPerBlackMuglumpKilled * settings.BlackMuglumpsSlainCount,
                PointsPerBlueMuglumpKilled * settings.BlueMuglumpsSlainCount,
                PointsPerSilverbackMuglumpKilled * settings.SilverbackMuglumpsSlainCount);

            if (settings.RedMuglumpsSlainCount == settings.TotalMuglumpsCount)
            {
                totalScore += (int)(totalScore * settings.Accuracy);
            }

            totalScore += PointsPerVisitedRoom * settings.RoomsVisitedCount;

            totalScore += PointsPerTimesCarried * Math.Min(TimesCarriedMaximumMultiplier, settings.TimesCarriedCount);

            totalScore += settings.FoundTheCrown ? PointsForCrown : 0;

            totalScore += settings.Survived && 
                MathfExtension.Sum(
                    settings.RedMuglumpsSlainCount, 
                    settings.BlackMuglumpsSlainCount, 
                    settings.BlueMuglumpsSlainCount,
                    settings.GoldMuglumpsSlainCount,
                    settings.SilverbackMuglumpsSlainCount) == settings.TotalMuglumpsCount ? PointsForSurviving : 0;

            return totalScore;
        }

        public void ShowBadgeEarned(Badge badge, float duration, Action onShowBadgeComplete)
        {
            this.badgeEarnedPanelBehaviour.Show(badge, duration, onShowBadgeComplete);
        }

        public void StartBossSequence()
        {
            if (this.Player.CurrentRoom.IsEntrance)
            {
                this.HasDefeatedBoss = true;
                return;
            }

            var silverbackMuglumpBehaviour = this.Dungeon.AddEntity<SilverbackMuglumpBehaviour>(this.silverbackMuglumpPrefab, this.Dungeon.GetEntrance());
            silverbackMuglumpBehaviour.IsBoss = true;
            this.AppendLineMainWindowText(StringContent.NewSilverbackMuglump);
        }

        /// <summary>
        /// Waits for predicate to be false then does the action.
        /// </summary>
        /// <param name="waitAction">The wait action.</param>
        /// <returns></returns>
        public IEnumerator WaitForPredicateToBeFalseThenDoAction(WaitAction waitAction)
        {
            while (waitAction.Predicate())
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            waitAction.DoAction.Invoke();
        }
    }
}
