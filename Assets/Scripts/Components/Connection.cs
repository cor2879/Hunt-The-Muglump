/**************************************************
 *  Connection.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines connection point that allows one dungeon section to connect to another.
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// Gets the direction that the connection faces.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public Direction Direction { get; private set; }

        /// <summary>
        /// Gets the room that serves as the connecting point.
        /// </summary>
        /// <value>
        /// The room.
        /// </value>
        public RoomBehaviour Room { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <param name="direction">The direction that the room connection faces.</param>
        public Connection(RoomBehaviour room, Direction direction)
        {
            this.Direction = direction;
            this.Room = room;
            this.Room.IsConnection = true;
        }

        /// <summary>
        /// Gets a value indicating whether this connection connects to another dungeon section.
        /// </summary>
        /// <value>
        ///   <c>false</c> if this instance is connected to another section; otherwise, <c>true</c>.
        /// </value>
        public bool IsOpen
        {
            get
            {
                return this.Room?.GetAdjacentRoom(this.Direction) == null;
            }
        }
    }
}
