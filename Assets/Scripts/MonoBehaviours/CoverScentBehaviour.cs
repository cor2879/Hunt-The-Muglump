/**************************************************
 *  CoverScentBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(PlayerBehaviour))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class CoverScentBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The active color
        /// </summary>
        private static readonly Color activeColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);

        /// <summary>
        /// The inactive state
        /// </summary>
        private const int inactiveState = -1;

        [SerializeField, ReadOnly]
        private Color inactiveColor = Color.white;

        /// <summary>
        /// The player
        /// </summary>
        [SerializeField, ReadOnly]
        private PlayerBehaviour player;

        /// <summary>
        /// The sprite renderer
        /// </summary>
        [SerializeField, ReadOnly]
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// The active turns
        /// </summary>
        [SerializeField, ReadOnly]
        private int activeTurns;

        /// <summary>
        /// The starting turn
        /// </summary>
        [SerializeField, ReadOnly]
        private int startingTurn = CoverScentBehaviour.inactiveState;

        /// <summary>
        /// The last player turn
        /// </summary>
        [SerializeField, ReadOnly]
        private int lastPlayerTurn;

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        public PlayerBehaviour Player
        {
            get
            {
                if (this.player == null)
                {
                    this.player = this.GetComponent<PlayerBehaviour>();
                }

                return this.player;
            }
        }

        /// <summary>
        /// Gets the sprite renderer.
        /// </summary>
        /// <value>
        /// The sprite renderer.
        /// </value>
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (this.spriteRenderer == null)
                {
                    this.spriteRenderer = this.GetComponent<SpriteRenderer>();
                }

                return this.spriteRenderer;
            }
        }

        /// <summary>
        /// Gets the active turns.
        /// </summary>
        /// <value>
        /// The active turns.
        /// </value>
        public int ActiveTurns
        {
            get => this.activeTurns;
            private set => this.activeTurns = value;
        }

        /// <summary>
        /// Gets the starting turn.
        /// </summary>
        /// <value>
        /// The starting turn.
        /// </value>
        public int StartingTurn
        {
            get => this.startingTurn;
            private set => this.startingTurn = value;
        }

        /// <summary>
        /// Gets or sets the last player turn.
        /// </summary>
        /// <value>
        /// The last player turn.
        /// </value>
        public int LastPlayerTurn
        {
            get => this.lastPlayerTurn;
            private set => this.lastPlayerTurn = value;
        }

        /// <summary>
        /// Gets the color of the inactive.
        /// </summary>
        /// <value>
        /// The color of the inactive.
        /// </value>
        public Color InactiveColor
        {
            get => this.inactiveColor;
            private set => this.inactiveColor = value;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get => this.StartingTurn >= 0;
        }

        /// <summary>
        /// Activates the for the specified turn duration.
        /// </summary>
        /// <param name="turnDuration">The number of turns for which the CoverScent should remain active.</param>
        public void Activate(int turnDuration)
        {
            this.ActiveTurns += turnDuration;
            GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.OpenBottle);

            if (this.StartingTurn < 0)
            {
                this.StartingTurn = this.Player.MoveCount;
                this.LastPlayerTurn = this.Player.MoveCount;
                this.SpriteRenderer.color = CoverScentBehaviour.activeColor;                
            }
        }

        public void FixedUpdate()
        {
            if (this.StartingTurn < 0)
            {
                return;
            }

            if (this.ActiveTurns > 0)
            {
                if (this.Player.MoveCount > this.LastPlayerTurn)
                {
                    this.LastPlayerTurn = this.Player.MoveCount;

                    if (--this.ActiveTurns == 0)
                    {
                        this.StartingTurn = CoverScentBehaviour.inactiveState;
                        this.SpriteRenderer.color = this.inactiveColor;
                        this.Player.MovementBehaviour.CurrentDestination.AddSelfDestructClue(StringContent.CoverScentExpired);
                    }
                }
            }
        }

        public void Start()
        {
            this.StartingTurn = CoverScentBehaviour.inactiveState;
            this.InactiveColor = Color.white;
        }
    }
}
