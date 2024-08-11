/**************************************************
 *  CoverScentBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.ItemBehaviour" />
    public class CoverScentItemBehaviour : ItemBehaviour
    {
        /// <summary>
        /// Gets the type of the item.
        /// </summary>
        /// <value>
        /// The type of the item.
        /// </value>
        public override ItemType ItemType { get => ItemType.EauDuMuglump; }

        /// <summary>
        /// Handles the encounter.
        /// </summary>
        /// <param name="player">The player.</param>
        public override void HandleEncounter(PlayerBehaviour player)
        {
            GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.GlassClink2);
            base.HandleEncounter(player);
        }
    }
}
