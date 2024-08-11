/**************************************************
 *  BowBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System;

    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines behaviours for the Bow
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(Animator))]
    public class BowBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The velocity of arrows fired from the bow
        /// </summary>
        [SerializeField]
        private float velocity;

        /// <summary>
        /// The animator
        /// </summary>
        private Animator animator;

        /// <summary>
        /// The <see cref="PlayerBehaviour" /> that owns this instance.
        /// </summary>
        private PlayerBehaviour owner;

        /// <summary>
        /// The current direction
        /// </summary>
        private Direction currentDirection;

        /// <summary>
        /// Gets or sets the velocity of arrows fired from this bow.
        /// </summary>
        /// <value>
        /// The velocity.
        /// </value>
        public float Velocity
        {
            get { return this.velocity; }
            set { this.velocity = value; }
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
                    this.animator = GetComponent<Animator>();
                }

                return this.animator;
            }
        }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        public PlayerBehaviour Owner
        {
            get
            {
                if (this.owner == null)
                {
                    this.owner = GetComponent<PlayerBehaviour>();
                }

                return this.owner;
            }
        }

        /// <summary>
        /// Fires an arrow from the specified starting position toward the specified direction.
        /// </summary>
        /// <param name="startingPosition">The starting position.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        public int Fire(ArrowBehaviour arrowBehaviour, Vector3 startingPosition, Direction direction, Action onDestinationReached)
        {
            Debug.Log("Fire!");
            var originRoom = this.Owner.CurrentRoom;

            if (originRoom.GetAdjacentRoom(direction) != null)
            {
                arrowBehaviour.Fire(this.Owner.CurrentRoom, direction, this.velocity, onDestinationReached);

                this.Animator.SetBool(Constants.IsFiring, true);
                this.Animator.SetFloat(Constants.HeroFiringXDirection, direction.XValue);
                this.Animator.SetFloat(Constants.HeroFiringYDirection, direction.YValue);

                return 1;
            }

            return 0;
        }
    }
}
