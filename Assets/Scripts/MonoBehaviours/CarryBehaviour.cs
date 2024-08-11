/**************************************************
 *  CarryBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;

    /// <summary>
    /// Defines the behaviour for EntityBehaviours that can carry other
    /// EntityBehaviours.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class CarryBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets the <see cref="EntityBehaviour" /> that will be carried.
        /// </summary>
        /// <value>
        /// The victim.
        /// </value>
        public EntityBehaviour Victim { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="EntityBehaviour" /> that will do the carrying.
        /// </summary>
        /// <value>
        /// The carrier.
        /// </value>
        public EntityBehaviour Carrier { get; set; }

        /// <summary>
        /// Executes during the Start event of the GameObject lifecycle.
        /// </summary>
        public void Start()
        {
            this.Carrier = GetComponent<EntityBehaviour>();
        }

        /// <summary>
        /// Updates this instance each time the Unity Engine updates the frame at runtime.
        /// </summary>
        public void Update()
        {
            if (this.Victim != null)
            {
                this.Victim.transform.SetPositionAndRotation(this.Carrier.transform.position, Quaternion.identity);
            }
        }

        /// <summary>
        /// Determines whether the specified entity is being carried by this instance.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity is being carried by this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCarrying(EntityBehaviour entity)
        {
            return entity == this.Victim;
        }

        /// <summary>
        /// Carries the specified victim.
        /// </summary>
        /// <param name="victim">The victim.</param>
        public void Carry(EntityBehaviour victim)
        {
            if (victim == null)
            {
                return;
            }

            this.Victim = victim;
            this.Victim.IsBeingCarried = true;

            if (this.Victim.CurrentRoom != null)
            {
                this.Victim.CurrentRoom.Exit(this.Victim);
            }

            this.Victim.transform.SetParent(this.Carrier.transform);
            
            CameraManager.Follow(this.gameObject);
            CameraManager.SetXDamping(0);
            CameraManager.SetYDamping(0);
        }

        /// <summary>
        /// Drops the current victim, if any.
        /// </summary>
        public void Drop()
        {
            if (this.Victim == null)
            {
                return;
            }

            this.Victim.MoveToRoom(this.Carrier.CurrentRoom);

            if (!this.Carrier.CurrentRoom.ContainsHazard())
            {
                Statistic.BatsRidden.Value++;
            }

            this.Victim.transform.SetPositionAndRotation(this.Victim.CurrentRoom.Position + this.Victim.GetIdleZeroPointOffsetVector(), Quaternion.identity);

            CameraManager.TransitionTo(this.Victim);

            this.Victim.IsBeingCarried = false;
            this.Victim = null;
        }

        /// <summary>
        /// Gets the carry behaviour.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static CarryBehaviour GetCarryBehaviour(EntityBehaviour entity)
        {
            var carryBehaviour = entity.GetComponent<CarryBehaviour>();

            if (carryBehaviour != null)
            {
                carryBehaviour.Carrier = entity;
                return carryBehaviour;
            }

            return null;
        }
    }
}
