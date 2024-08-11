#pragma warning disable CS0649
/**************************************************
 *  DungeonCrawlGameManager.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.DungeonCrawl.MonoBehaviours
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
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;

    public class DungeonCrawlGameManager : MonoBehaviour, IGameManager
    {
        #region Fields

        /// <summary>
        /// The camera manager
        /// </summary>
        public CameraManager cameraManager;

        /// <summary>
        /// The lock input
        /// </summary>
        private bool lockInput = false;

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
        /// The music manager
        /// </summary>
        [SerializeField]
        private AudioManager musicManager;

        /// <summary>
        /// The pause action
        /// </summary>
        [SerializeField]
        private bool pauseAction;

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

        #endregion

        #region Properties

        public AudioManager MusicManager
        {
            get => this.musicManager;
        }

        public AudioManager SoundEffectManager
        {
            get => this.soundEffectManager;
        }

        public DungeonBehaviour Dungeon { get; set; }

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

        public PlayerBehaviour Player { get; set; }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static DungeonCrawlGameManager Instance { get; private set; }

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

        #endregion

        #region MonoBehaviour Methods

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

        #endregion

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
