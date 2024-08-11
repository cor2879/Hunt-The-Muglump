/**************************************************
 *  ArrowItemBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System;
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the behaviour for arrows which may be picked up and
    /// added to the player inventory.
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.EntityBehaviour" />
    [RequireComponent(typeof(SpriteRenderer))]
    public class ArrowItemBehaviour : EntityBehaviour, IFindable, IArrowType
    {
        private static readonly Vector3 IdlePointOffsetVector = new Vector3(0.0f, -0.4f, 0.0f);

        /// <summary>
        /// Gets the type of the arrow.
        /// </summary>
        /// <value>
        /// The type of the arrow.
        /// </value>
        public virtual ArrowType ArrowType
        {
            get { return ArrowType.Arrow; }
        }

        /// <summary>
        /// Handles the encounter.
        /// </summary>
        /// <param name="player">The player.</param>
        public virtual void HandleEncounter(PlayerBehaviour player)
        {
            player.GetArrow(this.ArrowType);
            GameManager.Instance.AppendLineMainWindowText(StringContent.FoundArrow[this.ArrowType]());
            this.CurrentRoom.Exit(this);

            if (this != null)
            {
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// Gets the offset vector that determines where the entity should stand within a parent transform by default
        /// when at rest.
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetIdleZeroPointOffsetVector()
        {
            return ArrowItemBehaviour.IdlePointOffsetVector;
        }
    }
}
