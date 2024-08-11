/**************************************************
 *  ClueBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines a behaviour that can be used to leave a specific clue in a room.
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.EntityBehaviour" />
    public class ClueBehaviour : EntityBehaviour
    {
        /// <summary>
        /// The message
        /// </summary>
        [SerializeField, ReadOnly]
        private string message;

        [SerializeField, ReadOnly]
        private bool selfDestruct;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get
            {
                this.DestroyInNextFrame = true;
                return this.message;
            }

            set => this.message = value;
        }

        public bool SelfDestruct
        {
            get => this.selfDestruct;
            set => this.selfDestruct = value;
        }

        private bool DestroyInNextFrame { get; set; } = false;

        /// <summary>
        /// Gets the message that should be displayed when the Player enters a room
        /// that contains this clue.
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
            var message = this.Message;

            return message;
        }

        /// <summary>
        /// Handles encounters with the Player.  Encounters occur when the player
        /// enters a room already occupied by this instance.
        /// </summary>
        /// <param name="player">The player.</param>
        public void HandleEncounter(PlayerBehaviour player)
        {
            GameManager.Instance.AppendLineMainWindowText(this.GetMessage());
        }

        public void Update()
        {
            if (this.DestroyInNextFrame)
            {
                this.Destroy();
            }
        }

        /// <summary>
        /// Destroys this instance.
        /// </summary>
        public void Destroy()
        {
            if (this.CurrentRoom != null)
            {
                this.CurrentRoom.Exit(this);
            }

            Destroy(this);
        }
    }
}
