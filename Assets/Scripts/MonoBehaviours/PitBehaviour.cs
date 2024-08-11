/**************************************************
 *  PitTrapBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System.Collections.Generic;

    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the behaviours for Pit objects.
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.EntityBehaviour" />
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PitBehaviour : EntityBehaviour, INettable
    {
        private static readonly Vector3 IdlePointOffsetVector = new Vector3(0.0f, -0.4f, 0.0f);

        [ReadOnly, SerializeField]
        private NetBehaviour net;

        /// <summary>
        /// Gets a value indicating whether this instance is killing the player.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is killing player; otherwise, <c>false</c>.
        /// </value>
        public bool IsKillingPlayer { get; private set; }

        /// <summary>
        /// Gets the duration of the kill.
        /// </summary>
        /// <value>
        /// The duration of the kill.
        /// </value>
        public float KillDuration { get; private set; }

        /// <summary>
        /// Gets the net.
        /// </summary>
        /// <value>
        /// The net.
        /// </value>
        public NetBehaviour Net
        {
            get => this.net;
            private set => this.net = value;
        }

        /// <summary>
        /// Gets the percent complete.
        /// </summary>
        /// <value>
        /// The percent complete.
        /// </value>
        public float PercentComplete { get; private set; }

        /// <summary>
        /// Gets the percent change.
        /// </summary>
        /// <value>
        /// The percent change.
        /// </value>
        public float PercentChange { get; private set; }

        /// <summary>
        /// Gets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        public Vector3 Scale { get; private set; }

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public EntityBehaviour Target { get; private set; }

        public override IList<string> IdleSounds { get => SoundClips.IdleWind; }

        /// <summary>
        /// Executes during the Start event of the GameObject life cycle.
        /// </summary>
        public void Start()
        {
            this.IsKillingPlayer = false;
            this.KillDuration = GameManager.Instance.SoundEffectManager.GetAudioClip(SoundClips.RetroScream).length;
            this.PercentComplete = 0.0f;
            this.PercentChange = (Time.fixedDeltaTime / this.KillDuration) * 2.0f;
            this.Scale = Vector3.zero;
            this.Target = null;
        }

        /// <summary>
        /// Updates this GameObject's state on a fixed interval which is determined by the UnityEngine at runtime.
        /// </summary>
        public void FixedUpdate()
        {
            if (this.IsKillingPlayer)
            {
                this.CreateFallingEffect();
            }
        }

        /// <summary>
        /// Executed when another <see cref="RigidBody2D" /> makes contact with this instance's <see cref="Collider2D" />
        /// </summary>
        /// <param name="collider">The collider</param>
        public void OnTriggerEnter2D(Collider2D collider)
        {
            var player = collider.gameObject.GetComponent<PlayerBehaviour>();

            if (player == null || player.IsBeingCarried || this.Net != null)
            {
                return;
            }

            if (player.CurrentRoom != this.CurrentRoom)
            {
                player.MoveToRoom(this.CurrentRoom);
            }

            this.KillPlayer(player);
        }

        /// <summary>
        /// Handles encounters with the player.  An encounter occurs when the player enters the room that this
        /// instance already occupies.
        /// </summary>
        /// <param name="player">The player.</param>
        public bool HandleEncounter(PlayerBehaviour player)
        {
            if (this.Net == null)
            {
                this.KillPlayer(player);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Kills the player.
        /// </summary>
        /// <param name="player">The player.</param>
        private void KillPlayer(PlayerBehaviour player)
        {
            CameraManager.TransitionTo(this);
            player.IsFalling = true;
            player.Kill(new KillOptions()
            {
                AudioClip = player.GetDeathSound(this),
                OnKill = this.OnKill,
                OnKilled = () =>
                {
                    GameManager.Instance.SetMainWindowText(StringContent.PitDeath);
                },
                GameOverCondition = GameOverCondition.Fallen
            });

            player.transform.position = Vector3.Lerp(player.transform.position, this.Position, 1.0f);
        }

        /// <summary>
        /// Called when a Kill action initiated by this instance begins.
        /// </summary>
        /// <param name="entity">The entity.</param>
        private void OnKill(EntityBehaviour entity)
        {
            this.IsKillingPlayer = true;
            this.PercentComplete = 0.0f;
            entity.GetComponent<SpriteRenderer>().material = GameManager.Instance.Sprite2DMaterial;
            this.Scale = entity.transform.localScale;
            this.Target = entity;
            GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.Falling);
        }

        /// <summary>
        /// Creates the falling effect.
        /// </summary>
        public void CreateFallingEffect()
        {
            if (this.PercentComplete < 1.0f && this.Target != null)
            {
                var newScale = new Vector3(
                    this.Scale.x - (this.PercentChange * this.Scale.x),
                    this.Scale.y - (this.PercentChange * this.Scale.y),
                    0);

                this.Target.transform.localScale = newScale;
                this.Scale = newScale;
                this.PercentComplete += this.PercentChange;

            }
            else
            {
                if (this.Target != null)
                {
                    this.Target.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
                this.IsKillingPlayer = false;
            }
        }

        /// <summary>
        /// Gets the message that should be displayed when the Player enters a room
        /// adjacent to the one this Entity is occupying.
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
            if (this.Net == null)
            {
                return StringContent.PitWarning;
            }

            return null;
        }

        public void ApplyNet(NetBehaviour net)
        {
            if (this.net == null)
            {
                net.gameObject.transform.SetParent(this.gameObject.transform);
                net.transform.position = this.transform.position;
                net.transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
                net.transform.rotation = Quaternion.identity;
                this.Net = net;
            }
            else
            {
                net.gameObject.SetActive(false);
                Destroy(net.gameObject);
            }
        }

        public override Vector3 GetIdleZeroPointOffsetVector()
        {
            return PitBehaviour.IdlePointOffsetVector;
        }
    }
}
