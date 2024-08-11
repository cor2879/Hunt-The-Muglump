/**************************************************
 *  ArrowBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System;
    using System.Collections;

    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the behaviour for an arrow object
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.Interfaces.IPoolable" />
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(ArcBehaviour))]
    public class ArrowBehaviour : MonoBehaviour, IPoolable
    {
        [SerializeField, ReadOnly]
        private RoomBehaviour destination;

        /// <summary>
        /// The animator
        /// </summary>
        private Animator animator;

        /// <summary>
        /// The arc behaviour
        /// </summary>
        private ArcBehaviour arcBehaviour;

        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// The object pool
        /// </summary>
        protected ObjectPool objectPool;

        /// <summary>
        /// The target
        /// </summary>
        protected EntityBehaviour target;

        /// <summary>
        /// The direction
        /// </summary>
        [SerializeField, ReadOnly]
        private Direction direction;

        /// <summary>
        /// The velocity
        /// </summary>
        [SerializeField, ReadOnly]
        private float velocity;

        [SerializeField, ReadOnly]
        protected Action onFireComplete;

        /// <summary>
        /// Gets the destination.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        public RoomBehaviour Destination
        {
            get { return this.destination; }
            set { this.destination = value; }
        }

        public Direction Direction
        {
            get { return this.direction; }
            set { this.direction = value; }
        }

        public virtual ArrowType ArrowType { get => ArrowType.Arrow; }

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
                    this.animator = this.GetComponent<Animator>();
                }

                return this.animator;
            }
        }

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
        /// Gets the arc behaviour.
        /// </summary>
        /// <value>
        /// The arc behaviour.
        /// </value>
        public ArcBehaviour ArcBehaviour
        {
            get
            {
                if (this.arcBehaviour == null)
                {
                    this.arcBehaviour = this.GetComponent<ArcBehaviour>();
                }

                return this.arcBehaviour;
            }
        }

        /// <summary>
        /// Gets the velocity.
        /// </summary>
        /// <value>
        /// The velocity.
        /// </value>
        public float Velocity
        {
            get { return this.velocity; }
            private set { this.velocity = value; }
        }

        /// <summary>
        /// Gets the game object.
        /// </summary>
        /// <value>
        /// The game object.
        /// </value>
        public GameObject GameObject { get { return this.gameObject; } }

        /// <summary>
        /// Sets the object pool.
        /// </summary>
        /// <param name="objectPool">The object pool.</param>
        public void SetObjectPool(ObjectPool objectPool)
        {
            this.objectPool = objectPool;
        }

        /// <summary>
        /// Called when the poolable object has completed its task and may be
        /// returned to the pool.
        /// </summary>
        public virtual void OnDestinationReached()
        {
            this.onFireComplete?.Invoke();

            if (this.target != null)
            {
                CameraManager.TransitionTo(target);
                this.target = null;
            }
            else
            {
                if (CameraManager.IsFollowing(this.gameObject))
                {
                    CameraManager.TransitionTo(GameManager.Instance.Player);
                }

                if (this.objectPool != null)
                {
                    this.objectPool.Deactivate(this);
                }
                else
                {
                    this.gameObject.SetActive(false);
                    Destroy(this.gameObject);
                }
            }
        }

        public virtual void Fire(RoomBehaviour startingRoom, Direction direction, float velocity, Action onDestinationReached)
        {
            this.onFireComplete = onDestinationReached;
            this.Fire(startingRoom, direction, velocity, 1.0f);
        }

        /// <summary>
        /// Fires from the specified starting room toward the specified direction.
        /// </summary>
        /// <param name="startingRoom">The starting room.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="velocity">The velocity.</param>
        protected void Fire(RoomBehaviour startingRoom, Direction direction, float velocity, float totalPercentage)
        {
            this.Direction = direction;
            GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.ArrowShoot);
            this.SpriteRenderer.enabled = true;
            this.Animator.SetFloat(Constants.XFiringDirection, direction.XValue);
            this.Animator.SetFloat(Constants.YFiringDirection, direction.YValue);
            CameraManager.Follow(this.GameObject);
            CameraManager.SetXDamping(0.0f);
            CameraManager.SetYDamping(0.0f);
            this.Velocity = velocity;

            this.Destination = startingRoom.GetAdjacentRoom(direction);
            var travelDuration = 1.0f / this.velocity;

            this.ArcBehaviour.BeginArc(
                startingRoom.GetAdjacentRoom(direction).Position, 
                travelDuration, 
                direction.RotationAxis,
                totalPercentage,
                this.OnDestinationReached);
        }

        public virtual void ContinuePath(Vector3 startingPosition, Direction direction, RoomBehaviour destination, Quaternion rotation, float velocity, float startingPercentage, float totalPercentage)
        {
            this.Direction = direction;
            this.Animator.SetFloat(Constants.XFiringDirection, direction.XValue);
            this.Animator.SetFloat(Constants.YFiringDirection, direction.YValue);
            CameraManager.Follow(this.GameObject);
            CameraManager.SetXDamping(0.0f);
            CameraManager.SetYDamping(0.0f);
            this.Velocity = velocity;

            this.Destination = destination;
            var travelDuration = 1.0f / this.Velocity;

            this.ArcBehaviour.ContinueArc(
                this.Destination.Position,
                travelDuration,
                this.Direction.RotationAxis,
                rotation,
                startingPercentage,
                totalPercentage,
                this.OnDestinationReached
                );
        }

        /// <summary>
        /// Called when the object's collider interacts with a <see cref="Rigidbody2D" />.
        /// </summary>
        /// <param name="collision">The collision.</param>
        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            var muglump = collision.gameObject.GetComponent<MuglumpBehaviour>();

            if (muglump != null)
            {
                this.target = muglump;
                GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.ArrowHit);
                this.SpriteRenderer.enabled = false;

                this.IncrementArrowHitCount();
                StartCoroutine(KillTarget(muglump, new KillOptions()
                {
                    AudioClip = muglump.GetDeathSound(PlayerBehaviour.Instance),
                    OnKilled = () =>
                    {
                        var muglumpType = muglump.GetType();
                        var hunterBehaviour = muglump.GetComponent<HunterBehaviour>();

                        if (PlayerBehaviour.Instance.KillCount.ContainsKey(muglumpType))
                        {
                            PlayerBehaviour.Instance.KillCount[muglumpType]++;
                        }
                        else
                        {
                            PlayerBehaviour.Instance.KillCount[muglumpType] = 1;
                        }

                        muglump.KillCountStatistic.Value++;

                        if (this.GetType() == typeof(FlashArrowBehaviour) && muglump.Net != null
                            && PlayerBehaviour.Instance.CoverScentBehaviour.IsActive)
                        {
                            if (hunterBehaviour != null)
                            {
                                Statistic.SpecialKills.Value++;
                            }

                            if (muglump.IsTrapped)
                            {
                                Statistic.Overkills.Value++;
                            }
                        }

                        GameManager.Instance.SetMainWindowText(StringContent.MuglumpKill[muglump.GetType()]());
                        PlayerBehaviour.Instance.CurrentRoom.DetectNearbyHunters();
                        GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.SuccessDing);

                        foreach (var message in PlayerBehaviour.Instance.CurrentRoom.GetAdjacentRoomWarningMessages())
                        {
                            GameManager.Instance.AppendLineMainWindowText(message);
                        }
                    },
                    GameOverCondition = GameOverCondition.Unknown
                }));
            }
        }

        /// <summary>
        /// Attempts to kill the target.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        private IEnumerator KillTarget(EntityBehaviour entity, KillOptions options)
        {
            yield return new WaitForSeconds(GameManager.Instance.SoundEffectManager.GetAudioClip(SoundClips.ArrowHit).length / 2);

            if (options == null)
            {
                options = new KillOptions()
                {
                    OnKilled = () =>
                    {
                        if (this.objectPool != null)
                        {
                            this.objectPool.Deactivate(this);
                        }
                        else
                        {
                            Destroy(this);
                        }
                    },
                    OnKillUnsuccessful = () =>
                    {
                        if (this.objectPool != null)
                        {
                            this.objectPool.Deactivate(this);
                        }
                        else
                        {
                            Destroy(this);
                        }
                    },
                    GameOverCondition = GameOverCondition.Unknown
                };
            }
            else
            {
                var onKilled = options.OnKilled;

                options.OnKilled = () =>
                {
                    onKilled?.Invoke();

                    if (this.objectPool != null)
                    {
                        this.objectPool.Deactivate(this);
                    }
                    else
                    {
                        Destroy(this);
                    }
                };

                options.OnKillUnsuccessful = () =>
                {
                    if (this.objectPool != null)
                    {
                        this.objectPool.Deactivate(this);
                    }
                    else
                    {
                        Destroy(this);
                    }
                };
            }

            entity.Kill(options);
        }

        protected virtual void IncrementArrowHitCount()
        {
            switch (this.ArrowType)
            {
                case ArrowType.Arrow:
                    PlayerBehaviour.Instance.ArrowsHitCount++;
                    break;
                case ArrowType.FlashArrow:
                    PlayerBehaviour.Instance.FlashArrowsHitCount++;
                    break;
                case ArrowType.NetArrow:
                    PlayerBehaviour.Instance.NetArrowsHitCount++;
                    break;
            }
        }

        public void Reset()
        {
            this.transform.rotation = Quaternion.identity;
        }

        public static ArrowBehaviour Prefab => PoolablePrefabLibrary.Instance.ArrowPrefab as ArrowBehaviour;

        IPoolable IPoolable.GetPrefab()
        {
            return ArrowBehaviour.Prefab;
        }
    }
}
