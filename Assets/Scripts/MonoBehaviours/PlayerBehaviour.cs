#pragma warning disable CS0649
/**************************************************
 *  PlayerpBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.GameplayManagement;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the state and behaviours for the Player object
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.EntityBehaviour" />
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BowBehaviour))]
    [RequireComponent(typeof(InventoryBehaviour))]
    [RequireComponent(typeof(CoverScentBehaviour))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerBehaviour : EntityBehaviour
    {
        private static readonly Vector3 IdlePointOffsetVector = new Vector3(0.0f, -0.4f, 0.0f);

        /// <summary>
        /// The cover scent duration
        /// </summary>
        [SerializeField, ReadOnly]
        private int coverScentDuration = 10;

        /// <summary>
        /// Indicates whether or not this instance is currently in the Walking state.
        /// </summary>
        [SerializeField, ReadOnly]
        private bool isWalking;

        /// <summary>
        /// Indicates whether or not this instance is currently in the Firing state.
        /// </summary>
        [SerializeField, ReadOnly]
        private bool isFiring;

        /// <summary>
        /// The arrows fired count
        /// </summary>
        [SerializeField, ReadOnly]
        private int arrowsFiredCount;

        [SerializeField, ReadOnly]
        private int flashArrowsFiredCount;

        [SerializeField, ReadOnly]
        private int netArrowsFiredCount;

        [SerializeField, ReadOnly]
        private int coverScentsUsed;

        [SerializeField, ReadOnly]
        private int bearTrapsUsed;

        [SerializeField, ReadOnly]
        private int arrowsHitCount;

        [SerializeField, ReadOnly]
        private int flashArrowsHitCount;

        [SerializeField, ReadOnly]
        private int netArrowsHitCount;

        private Dictionary<Type, int> killCount = new Dictionary<Type, int>();

        /// <summary>
        /// The times carried
        /// </summary>
        [SerializeField, ReadOnly]
        private int timesCarried;

        /// <summary>
        /// The move count
        /// </summary>
        [SerializeField, ReadOnly]
        private int moveCount;

        [SerializeField, ReadOnly]
        private ArrowType selectedArrowType;

        [SerializeField, ReadOnly]
        private ItemType selectedItemType;

        /// <summary>
        /// The crown behaviour
        /// </summary>
        private CrownBehaviour crownBehaviour;

        /// <summary>
        /// The bow behaviour
        /// </summary>
        private BowBehaviour bowBehaviour;

        /// <summary>
        /// The animator
        /// </summary>
        private Animator animator;

        /// <summary>
        /// The movement behaviour
        /// </summary>
        private MovementBehaviour movementBehaviour;

        /// <summary>
        /// The inventory behaviour
        /// </summary>
        private InventoryBehaviour inventoryBehaviour;

        /// <summary>
        /// The can move
        /// </summary>
        [SerializeField, ReadOnly]
        private bool canMove = true;

        /// <summary>
        /// The cover scent behaviour
        /// </summary>
        [SerializeField, ReadOnly]
        private CoverScentBehaviour coverScentBehaviour;

        /// <summary>
        /// The Sprite Renderer
        /// </summary>
        [SerializeField, ReadOnly]
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// The lock input
        /// </summary>
        private bool lockInput;

        [SerializeField, ReadOnly]
        private bool hasCameraFocus;

        /// <summary>
        /// Gets the movement behaviour.
        /// </summary>
        /// <value>
        /// The movement behaviour.
        /// </value>
        public MovementBehaviour MovementBehaviour
        {
            get
            {
                if (this.movementBehaviour == null)
                {
                    this.movementBehaviour = MovementBehaviour.GetMovementBehaviour(this);
                }

                return this.movementBehaviour;
            }
        }

        /// <summary>
        /// Gets the duration of the cover scent.
        /// </summary>
        /// <value>
        /// The duration of the cover scent.
        /// </value>
        public int CoverScentDuration
        {
            get => this.coverScentDuration;
            set => this.coverScentDuration = value;
        }

        public ArrowType SelectedArrowType
        {
            get => this.selectedArrowType;
            private set => this.selectedArrowType = value;
        }

        /// <summary>
        /// Gets the type of the selected item type.
        /// </summary>
        /// <value>
        /// The type of the selected item type.
        /// </value>
        public ItemType SelectedItemType
        {
            get => this.selectedItemType;
            set => this.selectedItemType = value;
        }

        /// <summary>
        /// Gets the inventory behaviour.
        /// </summary>
        /// <value>
        /// The inventory behaviour.
        /// </value>
        public InventoryBehaviour Inventory
        {
            get
            {
                if (this.inventoryBehaviour == null)
                {
                    this.inventoryBehaviour = this.GetComponent<InventoryBehaviour>();
                }

                return this.inventoryBehaviour;
            }
        }

        /// <summary>
        /// Gets the animator.
        /// </summary>
        /// <value>
        /// The animator.
        /// </value>
        public Animator Animator
        {
            get
            {
                if (this.animator == null)
                {
                    try
                    {
                        this.animator = this.GetComponent<Animator>();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }

                return this.animator;
            }
        }

        /// <summary>
        /// Gets the Sprite Renderer
        /// </summary>
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
        /// Gets a value indicating whether this instance can move.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can move; otherwise, <c>false</c>.
        /// </value>
        public bool CanMove
        {
            get => this.canMove;
            set => this.canMove = value;
        }

        /// <summary>
        /// The instance
        /// </summary>
        private static PlayerBehaviour instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static PlayerBehaviour Instance { get => instance; }

        public bool HasCameraFocus { get => this.hasCameraFocus; private set => this.hasCameraFocus = value; }

        /// <summary>
        /// Gets the bow behaviour.
        /// </summary>
        /// <value>
        /// The bow behaviour.
        /// </value>
        public BowBehaviour BowBehaviour
        {
            get
            {
                if (this.bowBehaviour == null)
                {
                    this.bowBehaviour = this.GetComponent<BowBehaviour>();
                }

                return this.bowBehaviour;
            }
        }

        /// <summary>
        /// Gets the cover scent behaviour.
        /// </summary>
        /// <value>
        /// The cover scent behaviour.
        /// </value>
        public CoverScentBehaviour CoverScentBehaviour
        {
            get
            {
                if (this.coverScentBehaviour == null)
                {
                    this.coverScentBehaviour = this.GetComponent<CoverScentBehaviour>();
                }

                return this.coverScentBehaviour;
            }
        }

        /// <summary>
        /// Gets the arrows fired count.
        /// </summary>
        /// <value>
        /// The arrows fired count.
        /// </value>
        public int ArrowsFiredCount
        {
            get => this.arrowsFiredCount;
            private set => this.arrowsFiredCount = value;
        }

        public int FlashArrowsFiredCount { get => this.flashArrowsFiredCount; private set => this.flashArrowsFiredCount = value; }

        public int NetArrowsFiredCount { get => this.netArrowsFiredCount; private set => this.netArrowsFiredCount = value; }

        public int CoverScentsUsed { get => this.coverScentsUsed; private set => this.coverScentsUsed = value; }

        public int BearTrapsUsed { get => this.bearTrapsUsed; private set => this.bearTrapsUsed = value; }

        public int ArrowsHitCount
        {
            get => this.arrowsHitCount;
            set => this.arrowsHitCount = value;
        }

        public int FlashArrowsHitCount { get => this.flashArrowsHitCount; set => this.flashArrowsHitCount = value; }

        public int NetArrowsHitCount { get => this.netArrowsHitCount; set => this.netArrowsHitCount = value; }

        /// <summary>
        /// Gets the move count.
        /// </summary>
        /// <value>
        /// The move count.
        /// </value>
        public int MoveCount
        {
            get => this.moveCount;
            private set => this.moveCount = value;
        }

        public bool IsCoverScentActive { get => this.CoverScentBehaviour.IsActive; }

        public bool IsDying { get; private set; } = false;

        public bool IsVisible { get => this.SpriteRenderer != null && this.SpriteRenderer.enabled; }

        /// <summary>
        /// Gets a value indicating whether this instance is walking.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is walking; otherwise, <c>false</c>.
        /// </value>
        public bool IsWalking { get => this.isWalking; }

        public bool IsIdle
        {
            get => IsVisible && !IsWalking && CanMove && !IsFalling && !IsDying &&
                (this.CurrentRoom != null && !this.CurrentRoom.ContainsHazard());
        }

        public bool IsFalling { get; set; } = false;

        /// <summary>
        /// Gets a value indicating whether this instance is firing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is firing; otherwise, <c>false</c>.
        /// </value>
        public bool IsFiring { get => this.isFiring; private set => this.isFiring = value; }

        /// <summary>
        /// Gets a value indicating whether this instance has the crown.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has crown; otherwise, <c>false</c>.
        /// </value>
        public bool HasCrown { get => this.crownBehaviour != null;  }

        /// <summary>
        /// Gets or sets the kill count.
        /// </summary>
        /// <value>
        /// The kill count.
        /// </value>
        public Dictionary<Type, int> KillCount { get => this.killCount; }

        public override IList<string> MovementSounds { get => SoundClips.PlayerFootsteps;  }

        /// <summary>
        /// Gets or sets the times carried.
        /// </summary>
        /// <value>
        /// The times carried.
        /// </value>
        public int TimesCarried { get => this.timesCarried; set => this.timesCarried = value; }

        /// <summary>
        /// Gets the death sound.
        /// </summary>
        /// <param name="killer">The killer.</param>
        /// <returns></returns>
        public string GetDeathSound(EntityBehaviour killer)
        {
            if (killer != null && killer.GetType() == typeof(PitBehaviour))
            {
                return SoundClips.RetroScream;
            }

            return SoundClips.Wilhelm;
        }

        /// <summary>
        /// Executes when this instance is awakened
        /// </summary>
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        /// <summary>
        /// Updates this instance when each frame is updated by the Unity Engine.
        /// </summary>
        public void FixedUpdate()
        {
            this.HasCameraFocus = CameraManager.Instance.CameraTarget.IsFocusedOnTarget(this.gameObject);

            this.Animator.SetBool(Constants.IsFiring, false);

            if (GameManager.Instance.PauseAction)
            {
                return;
            }

            // TODO: figure out what to do about the Crown
            //
            // (this.crownBehaviour != null ?
            //     (Action)GameManager.Instance.CrownInventoryPanel.Enable :
            //     GameManager.Instance.CrownInventoryPanel.Disable).Invoke();

            this.isWalking = this.Animator.GetBool(Constants.IsWalking);

            if (!CameraManager.IsFollowing(this.gameObject))
            {
                return;
            }
        }

        public void LookAtRoom(Direction direction)
        {
            if (this.CurrentRoom.GetAdjacentRooms()[direction] != null)
            {
                CameraManager.TransitionTo(this.CurrentRoom.GetAdjacentRooms()[direction].gameObject);
            }
        }

        public void IncrementMoveCount()
        {
            ++this.MoveCount;
        }

        public void IncrementArrowUseCount(int count)
        {
            switch(this.SelectedArrowType)
            {
                case ArrowType.Arrow:
                    this.ArrowsFiredCount += count;
                    break;
                case ArrowType.FlashArrow:
                    this.FlashArrowsFiredCount += count;
                    break;
                case ArrowType.NetArrow:
                    this.NetArrowsFiredCount += count;
                    break;
            }
        }

        public void IncrementItemUseCount(int count)
        {
            switch (this.SelectedItemType)
            {
                case ItemType.EauDuMuglump:
                    this.CoverScentsUsed += count;
                    break;
                case ItemType.BearTrap:
                    this.BearTrapsUsed += count;
                    break;
            }
        }

        public void ShootArrow(Direction direction, Action onDestinationReached)
        {
            var arrowBehaviour = this.Inventory.GetArrow(this.SelectedArrowType);

            if (arrowBehaviour != null)
            {
                arrowBehaviour.transform.position = this.Position;
                this.IncrementArrowUseCount(this.BowBehaviour.Fire(arrowBehaviour, this.Position, direction, onDestinationReached));
            }
            else
            {
                var message = StringContent.ArrowEmpty[this.SelectedArrowType]();

                if (!GameplayMenuManagerBehaviour.Instance.MainTextPanel.Text.Contains(message))
                {
                    GameplayMenuManagerBehaviour.AppendLineMainWindowText(message);
                }

                onDestinationReached?.Invoke();
            }
        }

        /// <summary>
        /// Sets the parent.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public void SetParent(Transform parent)
        {
            this.gameObject.transform.SetParent(parent);
        }

        /// <summary>
        /// Stops the walking.
        /// </summary>
        public void StopWalking()
        {
            this.isWalking = false;
            this.MovementBehaviour.StopMotion();
        }

        /// <summary>
        /// Attempts to Kill this instance.  Whether or not the kill is successful may be determined
        /// by the implementation code of this method.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void Kill(KillOptions options)
        {
            this.IsDying = true;

            options?.OnKill?.Invoke(this);
            
            if (options != null && options.HideSpriteRenderer)
            {
                this.SpriteRenderer.enabled = false;
            }

            StartCoroutine(nameof(PlayerBehaviour.KillPlayer), options);
        }

        /// <summary>
        /// Kills the player.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        private IEnumerator KillPlayer(KillOptions options)
        {
            if (this.isWalking)
            {
                this.StopWalking();
            }

            if (!string.IsNullOrWhiteSpace(options?.AudioClip))
            {
                GameManager.Instance.SoundEffectManager.PlayAudioOnce(options.AudioClip);
                yield return new WaitForSeconds(GameManager.Instance.SoundEffectManager.GetAudioClip(options.AudioClip).length / 2);
            }

            GameManager.Instance.MusicManager.PlayDistinctAudioOnce(SoundClips.Defeat);

            yield return new WaitForSeconds(GameManager.Instance.MusicManager.GetAudioClip(SoundClips.Defeat).length);

            GameManager.Instance.GameOver(options.GameOverCondition);

            base.Kill(options);
        }

        public override void MoveToRoom(RoomBehaviour room)
        {
            base.MoveToRoom(room);

            if (!this.IsDying && !this.IsFalling)
            {
                this.CanMove = true;
            }
        }

        public void SelectArrows(ArrowType arrowType)
        {
            if (this.SelectedArrowType != arrowType)
            {
                this.SelectedArrowType = arrowType;
                GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.Click2);
            }
        }

        public void SetAnimatorValue(string parameter, bool value)
        {
            this.Animator?.SetBool(parameter, value);
        }

        /// <summary>
        /// Gets the crown.
        /// </summary>
        /// <param name="crown">The crown.</param>
        public void GetCrown(CrownBehaviour crown)
        {
            this.crownBehaviour = crown;
            GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.SmallVictory);
        }

        public void GetArrow(ArrowType arrowType)
        {
            this.Inventory.AddArrow(arrowType, 1);
            GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.Recovery);
        }

        /// <summary>
        /// Gets the arrow.
        /// </summary>
        public void GetItem(ItemType itemType)
        {
            this.Inventory.AddItem(itemType, 1);
            GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.Recovery);
        }

        public void UseBearTrap()
        {
            if (this.CurrentRoom.GetBearTrapBehaviour() != null)
            {
                return;
            }

            this.PlaceTrap(this.Inventory.GetBearTrap());
        }

        public void UseCoverScent()
        {
            if (this.Inventory.GetCoverScent())
            {
                this.CoverScentBehaviour.Activate(this.CoverScentDuration);
                this.IncrementItemUseCount(1);
            }
        }

        private void PlaceTrap(BearTrapBehaviour bearTrap)
        {
            bearTrap?.Place(this.CurrentRoom, this.transform.position);
        }

        public override Vector3 GetIdleZeroPointOffsetVector()
        {
            return PlayerBehaviour.IdlePointOffsetVector;
        }
    }
}
