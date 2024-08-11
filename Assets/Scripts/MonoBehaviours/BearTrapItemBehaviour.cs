/**************************************************
 *  BearTrapItemBehaviour.cs
 *  
 *  copyright (c) 2021 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the behaviour for arrows which may be picked up and
    /// added to the player inventory.
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.EntityBehaviour" />
    [RequireComponent(typeof(SpriteRenderer))]
    public class BearTrapItemBehaviour : ItemBehaviour
    {
        /// <summary>
        /// Gets the type of the arrow.
        /// </summary>
        /// <value>
        /// The type of the arrow.
        /// </value>
        public override ItemType ItemType
        {
            get { return ItemType.BearTrap; }
        }

        /// <summary>
        /// Handles the encounter.
        /// </summary>
        /// <param name="player">The player.</param>
        public override void HandleEncounter(PlayerBehaviour player)
        {
            GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.BearTrapGet);
            base.HandleEncounter(player);
        }
    }
}

