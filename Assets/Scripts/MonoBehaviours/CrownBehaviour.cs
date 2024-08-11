/**************************************************
 *  CrownBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the behaviours for the Crown object
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.EntityBehaviour" />
    public class CrownBehaviour : EntityBehaviour
    {
        private static readonly Vector3 IdlePointOffsetVector = new Vector3(0.0f, -0.4f, 0.0f);

        /// <summary>
        /// Handles encounters with the Player.  Encounters occur when the player enters the same
        /// room that contains this instance.
        /// </summary>
        /// <param name="player">The player.</param>
        public void HandleEncounter(PlayerBehaviour player)
        {
            player.GetCrown(this);
            GameManager.Instance.AppendLineMainWindowText(StringContent.FoundCrown);
            Statistic.CrownsFound.Value++;
            this.CurrentRoom.Exit(this);
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Gets the offset vector that determines where the entity should stand within a parent transform by default
        /// when at rest.
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetIdleZeroPointOffsetVector()
        {
            return CrownBehaviour.IdlePointOffsetVector;
        }
    }
}
