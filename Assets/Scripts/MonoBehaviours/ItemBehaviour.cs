/**************************************************
 *  ItemBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the base behaviour for items which may be picked up and
    /// added to the player inventory.
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.EntityBehaviour" />
    public abstract class ItemBehaviour : EntityBehaviour, IFindable, IItemType
    {
        private static readonly Vector3 IdlePointOffsetVector = new Vector3(0.0f, -0.4f, 0.0f);

        /// <summary>
        /// Gets the type of the arrow.
        /// </summary>
        /// <value>
        /// The type of the arrow.
        /// </value>
        public abstract ItemType ItemType { get; }

        /// <summary>
        /// Handles the encounter.
        /// </summary>
        /// <param name="player">The player.</param>
        public virtual void HandleEncounter(PlayerBehaviour player)
        {
            player.GetItem(this.ItemType);
            GameManager.Instance.AppendLineMainWindowText(StringContent.FoundItem[this.ItemType]());
            this.CurrentRoom.Exit(this);
            Destroy(this.gameObject);
        }

        /// <summary>
        /// Gets the offset vector that determines where the entity should stand within a parent transform by default
        /// when at rest.
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetIdleZeroPointOffsetVector()
        {
            return ItemBehaviour.IdlePointOffsetVector;
        }
    }
}
