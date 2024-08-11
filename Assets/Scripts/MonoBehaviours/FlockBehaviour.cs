/**************************************************
 *  FlockBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the state and behaviours for a flock of bats
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.EntityBehaviour" />
    [RequireComponent(typeof(MovementBehaviour))]
    [RequireComponent(typeof(CarryBehaviour))]
    public class FlockBehaviour : EntityBehaviour
    {

        private static readonly Vector3 IdlePointOffsetVector = new Vector3(0.0f, -0.4f, 0.0f);

        [SerializeField, ReadOnly]
        private CarryBehaviour carryBehaviour;

        [SerializeField, ReadOnly]
        private MovementBehaviour movementBehaviour;

        /// <summary>
        /// The bats in flock
        /// </summary>
        public int batsInFlock;

        /// <summary>
        /// Gets the bats.
        /// </summary>
        /// <value>
        /// The bats.
        /// </value>
        public List<BatBehaviour> Bats { get; private set; } = new List<BatBehaviour>();

        public override bool IsNear(EntityBehaviour entity)
        {
            return this.CarryBehaviour.IsCarrying(entity) || base.IsNear(entity);
        }

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
                    this.movementBehaviour = MovementBehaviour.GetMovementBehaviour(this);
                }

                return this.movementBehaviour;
            }
        }

        public override IList<string> MovementSounds { get => SoundClips.BatFlapping; }

        public override IList<string> IdleSounds { get => SoundClips.IdleFlapping; }

        /// <summary>
        /// Gets the carry behaviour.
        /// </summary>
        /// <value>
        /// The carry behaviour.
        /// </value>
        public CarryBehaviour CarryBehaviour 
        { 
            get
            {
                if (this.carryBehaviour == null)
                {
                    this.carryBehaviour = CarryBehaviour.GetCarryBehaviour(this);
                }

                return this.carryBehaviour;
            }
        }

        /// <summary>
        /// Executes during the Start event of the GameObject life cycle.
        /// </summary>
        public void Start()
        {
            for (var i = 0; i < batsInFlock; i++)
            {
                var gameObject = Instantiate(GameManager.Instance.batPrefab, this.gameObject.transform);
                this.Bats.Add(gameObject.GetComponent<BatBehaviour>());
            }
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            foreach (var bat in this.Bats)
            {
                bat.Origin = this.transform.position;
            }
        }

        /// <summary>
        /// Handles encounters with the player.  Encounters occure when the player enters a room already occupied by
        /// this instance.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns></returns>
        public bool HandleEncounter(PlayerBehaviour player)
        {
            if (this.MovementBehaviour.IsMoving || this.CarryBehaviour.IsCarrying(player))
            {
                return false;
            }

            var destination = this.GetCarryDestination();
            this.CarryBehaviour.Carry(player);
            player.TimesCarried++;
            GameManager.Instance.SetMainWindowText(StringContent.BatCarry);
            this.MovementBehaviour.MoveToRoom(
                destination, 
                () =>
                {
                    GameManager.Instance.ClearMainWindowText();
                    this.CarryBehaviour.Drop();
                    var newRoom = this.CurrentRoom.Dungeon.GetRandomUnoccupiedRoom();
                    this.MovementBehaviour.MoveToRoom(newRoom);
                },
                null);

            return true;
        }

        public void Flee()
        {
            this.MovementBehaviour.MoveToRoom(GameManager.Instance.Dungeon.GetRandomUnoccupiedRoom());
        }

        /// <summary>
        /// Gets the message that should be displayed when the Player enters a room
        /// adjacent to the one this Entity is occupying.
        /// </summary>
        /// <returns></returns>
        public override string GetMessage()
        {
            return StringContent.BatWarning;
        }

        private RoomBehaviour GetCarryDestination()
        {
            const int traversalDepth = 2;

            var sections = this.CurrentRoom.GetLocalSections(traversalDepth);

            var rooms = new List<RoomBehaviour>();
            
            foreach (var section in sections)
            {
                rooms.AddRange(section.GetFlattenedRoomCollection());
            }

            return DungeonBehaviour.GetRandomRoom(rooms, (room) => room != this && room.GetBats() == null);
        }

        public override Vector3 GetIdleZeroPointOffsetVector()
        {
            return FlockBehaviour.IdlePointOffsetVector;
        }
    }
}
