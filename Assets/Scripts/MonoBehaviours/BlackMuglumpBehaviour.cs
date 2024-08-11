/**************************************************
 *  BlackMuglumpBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines behaviours and state that are specific to Black Muglumps
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.MuglumpBehaviour" />
    [RequireComponent(typeof(MovementBehaviour))]
    [RequireComponent(typeof(BleedBehaviour))]
    public class BlackMuglumpBehaviour : MuglumpBehaviour
    {
        /// <summary>
        /// The starting hit points
        /// </summary>
        [SerializeField, ReadOnly]
        private int startingHitPoints = 2;

        /// <summary>
        /// The current hit points
        /// </summary>
        [SerializeField, ReadOnly]
        private int hitPoints;

        /// <summary>
        /// The movement behaviour
        /// </summary>
        [SerializeField, ReadOnly]
        private MovementBehaviour movementBehaviour;

        /// <summary>
        /// The bleed behaviour
        /// </summary>
        [SerializeField, ReadOnly]
        private BleedBehaviour bleedBehaviour;

        /// <summary>
        /// The clue behaviour
        /// </summary>
        [SerializeField, ReadOnly]
        private ClueBehaviour clueBehaviour;

        public override MuglumpType MuglumpType { get => MuglumpType.BlackMuglump; }

        /// <summary>
        /// Gets the movement behaviour.
        /// </summary>
        /// <value>
        /// The movement behaviour.
        /// </value>
        public MovementBehaviour MovementBehaviour
        {
            get
            {
                if (this.movementBehaviour == null)
                {
                    this.movementBehaviour = this.GetComponent<MovementBehaviour>();
                }

                return this.movementBehaviour;
            }
        }

        public override Statistic<int> KillCountStatistic => Statistic.BlackMuglumpsKilled;

        public override IList<string> IdleSounds { get => SoundClips.LargeBreathing; }

        /// <summary>
        /// Gets the bleed behaviour.
        /// </summary>
        /// <value>
        /// The bleed behaviour.
        /// </value>
        public BleedBehaviour BleedBehaviour
        {
            get
            {
                if (this.bleedBehaviour == null)
                {
                    this.bleedBehaviour = this.GetComponent<BleedBehaviour>();
                }

                return this.bleedBehaviour;
            }
        }

        /// <summary>
        /// Gets the clue behaviour.
        /// </summary>
        /// <value>
        /// The clue behaviour.
        /// </value>
        public ClueBehaviour ClueBehaviour
        {
            get { return this.clueBehaviour; }
            private set { this.clueBehaviour = value; }
        }

        /// <summary>
        /// Gets the hit points.
        /// </summary>
        /// <value>
        /// The hit points.
        /// </value>
        public int HitPoints
        {
            get { return this.hitPoints; }
            private set { this.hitPoints = value; }
        }

        public override bool IsTrapped
        {
            get => this.MovementBehaviour.IsTrapped;
            set => this.MovementBehaviour.IsTrapped = value;
        }

        public bool IsMoving
        {
            get => this.MovementBehaviour.IsMoving;
        }

        public override IList<string> MovementSounds { get => SoundClips.MediumMonsterFootsteps; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            this.HitPoints = this.startingHitPoints;
        }

        /// <summary>
        /// Attempts to Kill this instance.  Whether or not the kill is successful may be determined
        /// by the implementation code of this method.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void Kill(KillOptions options)
        {
            if (--this.HitPoints == 0)
            {
                if (this.ClueBehaviour != null)
                {
                    this.ClueBehaviour.Destroy();
                }

                if (this.BleedBehaviour != null)
                {
                    this.BleedBehaviour.CleanUpBlood();
                }

                base.Kill(options);
            }
            else
            {
                CameraManager.TransitionTo(PlayerBehaviour.Instance);

                options?.OnKillUnsuccessful?.Invoke();

                if (this.Net == null)
                {
                    this.RunAway();
                }
            }
        }

        /// <summary>
        /// Gets the message that should be displayed when the Player enters a room
        /// adjacent to the one this Entity is occupying.
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
            if (!this.IsMoving)
            {
                return base.GetMessage();
            }

            return null;
        }

        /// <summary>
        /// Causes the Black Muglump to flee to another random room in the dungeon.
        /// Black Muglumps will find a room that is empty and that is also not adjacent
        /// to any occopied rooms.  They love their solitude, especially after being
        /// injured ;-)
        /// </summary>
        private void RunAway()
        {
            var destination = GameManager.Instance.Dungeon.GetRandomRoom(room => !room.Occupants.Any()); // && !room.GetAdjacentRooms().Any(kvp => kvp.Value != null && kvp.Value.Occupants.Any()));

            if (destination != null)
            {
                this.BleedBehaviour.StartTheBleeding();
            }

            this.MovementBehaviour.MoveToRoom(
                destination, 
                null,
                () =>
                {
                    var clue = Instantiate(GameManager.Instance.cluePrefab).GetComponent<ClueBehaviour>();
                    clue.Message = StringContent.BlackMuglumpClue;
                    clue.MoveToRoom(this.CurrentRoom);
                    this.ClueBehaviour = clue;
                    this.BleedBehaviour.StopTheBleeding();
                });
            GameManager.Instance.SetMainWindowText(StringContent.BlackMuglumpHit);

            foreach (var message in PlayerBehaviour.Instance.CurrentRoom.GetAdjacentRoomWarningMessages())
            {
                GameManager.Instance.AppendLineMainWindowText(message);
            }
        }
    }
}
