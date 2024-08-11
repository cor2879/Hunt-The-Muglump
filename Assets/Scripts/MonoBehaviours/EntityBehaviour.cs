﻿/**************************************************
 *  EntityBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines a base state and set of behaviours for any interactive game entity.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class EntityBehaviour : MonoBehaviour
    {
        private static readonly string[] NoMovementSound = new string[0];
        
        /// <summary>
        /// Gets or sets a value indicating whether this instance is being carried.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is being carried; otherwise, <c>false</c>.
        /// </value>
        public bool IsBeingCarried { get; set; }

        /// <summary>
        /// Gets or sets the value indicateing whether or not this instance is trapped.
        /// </summary>
        public virtual bool IsTrapped { get; set; }

        public bool IsEnabled { get; private set; }

        /// <summary>
        /// Gets or sets the current room.
        /// </summary>
        /// <value>
        /// The current room.
        /// </value>
        public virtual RoomBehaviour CurrentRoom { get; set; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector3 Position { get => this.gameObject.transform.position; }

        public virtual IList<string> IdleSounds { get => EntityBehaviour.NoMovementSound; }

        public virtual IList<string> MovementSounds { get => EntityBehaviour.NoMovementSound; }

        public virtual bool IsNear(EntityBehaviour other)
        {
            return this.CurrentRoom.IsInView || MovementBehaviour.GetShortestPath(this.CurrentRoom, other.CurrentRoom).Length <= 3;
        }

        public virtual bool IsAdjacent(EntityBehaviour other)
        {
            return this.CurrentRoom != null && this.CurrentRoom == other.CurrentRoom || this.CurrentRoom.IsAdjacent(other.CurrentRoom);
        }

        /// <summary>
        /// Attempts to Kill this instance.  Whether or not the kill is successful may be determined
        /// by the implementation code of this method.
        /// </summary>
        /// <param name="options">The options.</param>
        public virtual void Kill(KillOptions options)
        {
            if (this.CurrentRoom != null)
            {
                this.CurrentRoom.Exit(this);
                this.CurrentRoom = null;
            }

            Destroy(this.gameObject);
            options?.OnKilled?.Invoke();
        }

        /// <summary>
        /// Causes this instance to flicker in the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public virtual IEnumerator Flicker(Color color)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            var originalColor = spriteRenderer.color;

            spriteRenderer.color = color;

            yield return new WaitForSeconds(0.1f);

            spriteRenderer.color = originalColor;
        }

        /// <summary>
        /// Moves this instance to the specified room.
        /// </summary>
        /// <param name="room">The room.</param>
        public virtual void MoveToRoom(RoomBehaviour room)
        {
            this.ExitRoom();
            this.EnterRoom(room);
        }

        /// <summary>
        /// Exits this instance from the specified room.
        /// </summary>
        protected virtual void ExitRoom()
        {
            if (this.CurrentRoom != null)
            {
                Debug.Log($"{this} LeavingRoom ({this.CurrentRoom.RowPosition},{this.CurrentRoom.ColumnPosition})");
                this.CurrentRoom.Exit(this);
                this.CurrentRoom = null;
            }
        }

        /// <summary>
        /// Enters this instance into the specified room.
        /// </summary>
        /// <param name="room">The room.</param>
        protected virtual void EnterRoom(RoomBehaviour room)
        {
            this.CurrentRoom = room;
            Debug.Log($"{this} Entering Room ({room.RowPosition},{room.ColumnPosition})");
            room.Enter(this);
        }

        /// <summary>
        /// Gets the offset vector that determines where the entity should stand within a parent transform by default
        /// when at rest.
        /// </summary>
        /// <returns></returns>
        public virtual Vector3 GetIdleZeroPointOffsetVector()
        {
            return Vector3.zero;
        }

        /// <summary>
        /// Gets the message that should be displayed when the Player enters a room
        /// adjacent to the one this Entity is occupying.
        /// </summary>
        /// <returns></returns>
        public virtual string GetMessage()
        {
            return null;
        }

        public void Enable()
        {
            this.gameObject.SetActive(true);
            this.IsEnabled = true;
        }

        public void Disable()
        {
            this.gameObject.SetActive(false);
            this.IsEnabled = false;
        }

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