#pragma warning disable CS0649
/**************************************************
 *  IntroPlayerBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System;
    using System.Collections;

    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.UI;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the behaviours for the player object that is used during the game intro.
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.EntityBehaviour" />
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class IntroPlayerBehaviour : EntityBehaviour
    {
        /// <summary>
        /// The movement speed
        /// </summary>
        [SerializeField]
        private float movementSpeed;

        /// <summary>
        /// The animator
        /// </summary>
        private Animator animator;

        /// <summary>
        /// The Sprite Renderer
        /// </summary>
        private SpriteRenderer spriteRenderer;

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
        /// Gets or sets the <see cref="Action" /> to execute once the object has
        /// completed its task.
        /// </summary>
        /// <value>
        /// The on complete.
        /// </value>
        public Action OnComplete { get; set; }

        /// <summary>
        /// Gets the movement percent per update.
        /// </summary>
        /// <value>
        /// The movement percent per update.
        /// </value>
        public float MovementPercentPerUpdate { get; private set; }

        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        public Vector3 Destination { get; set; }

        /// <summary>
        /// Updates the object stated at a fixed interval which is determined by the UnityEngine at runtime.
        /// </summary>
        public void FixedUpdate()
        {
            if (this.Destination.x < this.transform.position.x)
            {
                var movementVector = new Vector3(this.movementSpeed * -1, 0);
                var newPosition = this.transform.position + movementVector;
                this.transform.position = newPosition;

                if (this.Destination.x >= this.transform.position.x)
                {
                    StartCoroutine(nameof(this.KillIntroPlayer));
                }
            }
            else
            {
                this.Animator.SetBool(Constants.IsWalking, false);
                this.Animator.SetFloat(Constants.XDirection, Direction.Idle.XValue);
                this.Animator.SetFloat(Constants.YDirection, Direction.Idle.YValue);
            }
        }

        /// <summary>
        /// Moves to the specified destination.
        /// </summary>
        /// <param name="destination">The destination.</param>
        public void Move(Vector3 destination)
        {
            this.Destination = destination;

            this.Animator.SetBool(Constants.IsWalking, true);
            this.Animator.SetFloat(Constants.XDirection, Direction.West.XValue);
            this.Animator.SetFloat(Constants.YDirection, Direction.West.YValue);
        }

        /// <summary>
        /// Kills the intro player.
        /// </summary>
        /// <returns></returns>
        public IEnumerator KillIntroPlayer()
        {
            var soundEffectManager = TitleScreenBehaviour.Instance.SoundEffectManager;
            
            TitleScreenBehaviour.Instance.IntroBehaviour.Muglump.Animator.SetBool(Constants.IsEating, true);

            StartCoroutine(
                nameof(this.WaitForDurationThenDoAction),
                new WaitDuration(
                    Time.fixedDeltaTime,
                    () => this.SpriteRenderer.enabled = false));

            if (soundEffectManager != null)
            {
                soundEffectManager.PlayAudioOnce(SoundClips.AngryMonsterRoar);

                yield return new WaitForSeconds(soundEffectManager.GetAudioClip(SoundClips.AngryMonsterRoar).length / 2);

                soundEffectManager.PlayAudioOnce(SoundClips.Wilhelm);

                yield return new WaitForSeconds(soundEffectManager.GetAudioClip(SoundClips.Wilhelm).length / 2);
            }

            this.OnComplete?.Invoke();
        }

        public IEnumerator WaitForDurationThenDoAction(WaitDuration waitDuration)
        {
            while (waitDuration.Duration >= float.Epsilon)
            {
                waitDuration.Duration -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            waitDuration.DoAction.Invoke();
        }
    }
}
