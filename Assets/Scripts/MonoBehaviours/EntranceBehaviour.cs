/**************************************************
 *  EntranceBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines behaviours for the Entrance object
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.EntityBehaviour" />
    [RequireComponent(typeof(SpriteRenderer))]
    public class EntranceBehaviour : EntityBehaviour
    {
        private static readonly Vector3 IdlePointOffsetVector = new Vector3(0.0f, -0.4f, 0.0f);

        /// <summary>
        /// Handles encounters with the player object.  Encounters occur when the player enters the same
        /// room that is occupied by this instance.
        /// </summary>
        /// <param name="player">The player.</param>
        public void HandleEncounter(PlayerBehaviour player)
        {
            //    if (player.MoveCount > 0)
            //    {
            //        GameManager.Instance.MessageBoxBehaviour.Show(
            //            StringContent.ExitDungeonConfirmation,
            //            () =>
            //            {
            //                GameManager.Instance.GameOver(GameOverCondition.Victory);
            //                GameManager.Instance.MessageBoxBehaviour.Hide(true);
            //            },
            //            null);
            //    }
        }

        public override Vector3 GetIdleZeroPointOffsetVector()
        {
            return EntranceBehaviour.IdlePointOffsetVector;
        }
    }
}
