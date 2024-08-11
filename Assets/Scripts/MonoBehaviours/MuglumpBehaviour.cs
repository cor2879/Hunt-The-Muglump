#pragma warning disable CS0649
/**************************************************
 *  MuglumpBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the base set of behaviours and state for all Muglumps
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.EntityBehaviour" />
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(HeavyLaboredBreathingBehaviour))]
    public class MuglumpBehaviour : EntityBehaviour, INettable
    {
        [SerializeField, ReadOnly]
        private Animator animator;

        [SerializeField, ReadOnly]
        private bool isHandlingEncounter;

        [SerializeField, ReadOnly]
        private HeavyLaboredBreathingBehaviour heavyLaboredBreathingBehaviour;

        [SerializeField, ReadOnly]
        private NetBehaviour net;

        [SerializeField]
        private BlinkBehaviour minimapIcon;

        public Animator Animator
        {
            get
            {
                if (this.animator == null)
                {
                    this.animator = this.gameObject.GetComponent<Animator>();
                }

                return this.animator;
            }
        }

        public HeavyLaboredBreathingBehaviour HeavyLaboredBreathingBehaviour
        {
            get
            {
                if (this.heavyLaboredBreathingBehaviour == null)
                {
                    this.heavyLaboredBreathingBehaviour = this.GetComponent<HeavyLaboredBreathingBehaviour>();
                }

                return this.heavyLaboredBreathingBehaviour;
            }
        }

        public bool CanBreathe
        {
            get => this.HeavyLaboredBreathingBehaviour.CanBreathe;
            set => this.HeavyLaboredBreathingBehaviour.CanBreathe = value;
        }

        public void Hit()
        {
            GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.Grunting.GetNext());
        }

        /// <summary>
        /// Gets or sets the value indicating whether or not this instance is considered a boss.
        /// </summary>
        public bool IsBoss { get; set; }

        public virtual MuglumpType MuglumpType { get => MuglumpType.RedMuglump; }

        public BlinkBehaviour MinimapIcon
        {
            get
            {
                if (this.minimapIcon == null)
                {
                    throw new PrefabNotSetException($"The MinimapIcon for this instance needs to be set.");
                }

                return this.minimapIcon;
            }
        }

        public virtual Statistic<int> KillCountStatistic
        {
            get => Statistic.MuglumpsKilled;
        }

        public override IList<string> MovementSounds { get => SoundClips.MediumMonsterFootsteps; }

        public override IList<string> IdleSounds { get => SoundClips.MediumBreathing; }

        public NetBehaviour Net
        {
            get => this.net;
            private set => this.net = value;
        }

        public bool IsHandlingEncounter
        {
            get => this.isHandlingEncounter;
            private set => this.isHandlingEncounter = value;
        }

        public virtual void Update()
        {
            if (this.MinimapIcon != null)
            {
                this.MinimapIcon.Enabled = this.IsTrapped;
            }
        }

        /// <summary>
        /// Gets the death sound.
        /// </summary>
        /// <param name="killer">The killer.</param>
        /// <returns></returns>
        public string GetDeathSound(EntityBehaviour killer)
        {
            return SoundClips.MonsterDeath;
        }

        /// <summary>
        /// Handles encounters with the player.  Encounters occur when the player enters a room
        /// already occupied by this instance.
        /// </summary>
        /// <param name="player">The player.</param>
        public void HandleEncounter(PlayerBehaviour player)
        {
            if (!this.IsHandlingEncounter)
            {
                this.IsHandlingEncounter = true;
                player.CanMove = false;
                this.Animator.SetBool(Constants.IsEating, true);

                StartCoroutine(
                    nameof(this.WaitForDurationThenDoAction),
                    new WaitDuration(
                        Time.fixedDeltaTime,
                        () => player.SpriteRenderer.enabled = false));
                
                StartCoroutine(nameof(MuglumpBehaviour.KillPlayer), player);
            }
        }

        /// <summary>
        /// Kills the player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns></returns>
        public IEnumerator KillPlayer(PlayerBehaviour player)
        {
            GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.AngryMonsterRoar);
            yield return new WaitForSeconds(GameManager.Instance.SoundEffectManager.GetAudioClip(SoundClips.AngryMonsterRoar).length / 2);
            player.Kill(new KillOptions()
            {
                HideSpriteRenderer = true,
                AudioClip = player.GetDeathSound(this),
                OnKilled = () =>
                {
                    GameManager.Instance.SetMainWindowText(StringContent.MuglumpDeath);
                },
                GameOverCondition = GameOverCondition.Eaten
            });

            this.IsHandlingEncounter = false;
        }

        /// <summary>
        /// Attempts to Kill this instance.  Whether or not the kill is successful may be determined
        /// by the implementation code of this method.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void Kill(KillOptions options)
        {
            StartCoroutine(nameof(MuglumpBehaviour.KillMuglump), options);
        }

        /// <summary>
        /// Kills the muglump.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        private IEnumerator KillMuglump(KillOptions options)
        {
            if (!string.IsNullOrEmpty(options?.AudioClip))
            {
                this.Animator.SetBool(Constants.IsDying, true);
                this.CanBreathe = false;
               
                GameManager.Instance.SoundEffectManager.PlayAudioOnce(options.AudioClip);

                yield return new WaitForSeconds(1); // GameManager.Instance.SoundEffectManager.GetAudioClip(options.AudioClip).length / 4);
            }

            base.Kill(options);

            CameraManager.TransitionTo(PlayerBehaviour.Instance);
        }

        /// <summary>
        /// Gets the message that should be displayed when the Player enters a room
        /// adjacent to the one this Entity is occupying.
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
            return StringContent.MuglumpWarning;
        }

        public void ApplyNet(NetBehaviour net)
        {
            if (this.Net == null)
            {
                net.gameObject.transform.SetParent(this.gameObject.transform);
                net.transform.position = this.transform.position;
                net.transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
                net.transform.rotation = Quaternion.identity;
                this.Net = net;
            }
            else
            {
                net.gameObject.SetActive(false);
                Destroy(net.gameObject);
            }
        }

        public bool CanSmellPlayer()
        {
            return !PlayerBehaviour.Instance.CoverScentBehaviour.IsActive;
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
