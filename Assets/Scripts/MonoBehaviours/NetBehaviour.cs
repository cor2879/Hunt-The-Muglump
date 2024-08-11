#pragma warning disable CS0649
/**************************************************
 *  NetBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(GrowBehaviour))]
    public class NetBehaviour : ArrowBehaviour
    {
        [SerializeField, ReadOnly]
        private GrowBehaviour growBehaviour;

        [SerializeField, ReadOnly]
        private bool doNotDestroyOnDestinationReached;

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public INettable Target
        {
            get; private set;
        }

        public GrowBehaviour GrowBehaviour
        {
            get
            {
                if (this.growBehaviour == null)
                {
                    this.growBehaviour = this.GetComponent<GrowBehaviour>();
                }

                return this.growBehaviour;
            }
        }


        public override void OnDestinationReached()
        {
            if (this.Target != null)
            {
                this.Target.ApplyNet(this);
                this.GrowBehaviour.StopGrowing();
                this.Animator.SetBool(Constants.IsFlying, false);
            }

            if (CameraManager.IsFollowing(this.gameObject))
            {
                CameraManager.TransitionTo(GameManager.Instance.Player);
            }

            if (this.doNotDestroyOnDestinationReached)
            {
                return;
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

        public override void Fire(RoomBehaviour startingRoom, Direction direction, float velocity, Action onDestinationReached)
        {
            this.onFireComplete = onDestinationReached;
            base.Fire(startingRoom, direction, velocity, 0.5f);
        }

        public override void ContinuePath(Vector3 startingPosition, Direction direction, RoomBehaviour destination, Quaternion rotation, float velocity, float startingPercentage, float totalPercentage)
        {
            this.Animator.SetBool(Constants.IsFlying, true);
            this.GrowBehaviour.Grow(new Vector2(4.0f, 4.0f), 1.0f / velocity);
            GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.RopeNet);
            base.ContinuePath(startingPosition, direction, destination, rotation, velocity, startingPercentage, totalPercentage);
        }

        /// <summary>
        /// Called when the object's collider interacts with a <see cref="Rigidbody2D" />.
        /// </summary>
        /// <param name="collision">The collision.</param>
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            var nettable = collision.gameObject.GetComponent<EntityBehaviour>() as INettable;

            if (nettable != null)
            {
                this.Target = nettable;
                this.DoNotDestroyOnDestinationReached();

                PlayerBehaviour.Instance.NetArrowsHitCount++;

                var pit = nettable as PitBehaviour;

                if (pit != null)
                {
                    this.Animator.SetBool(Constants.IsFlying, false);
                }
            }
        }

        /// <summary>
        /// Does the not destroy on destination reached.
        /// </summary>
        private void DoNotDestroyOnDestinationReached()
        {
            this.doNotDestroyOnDestinationReached = true;
        }
    }
}
