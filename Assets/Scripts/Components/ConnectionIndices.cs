/**************************************************
 *  ConnectionIndices.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    using static UnityEngine.Random;

    /// <summary>
    /// Defines a data structure which is used to help construct dungeon sections by storing
    /// the index of the rooms in the sections room array which hold the connections.
    /// </summary>
    public class ConnectionIndices
    {
        /// <summary>
        /// Gets the north connection.
        /// </summary>
        /// <value>
        /// The north connection.
        /// </value>
        public int NorthConnection { get; private set; } = -1;

        /// <summary>
        /// Gets the east connection.
        /// </summary>
        /// <value>
        /// The east connection.
        /// </value>
        public int EastConnection { get; private set; } = -1;

        /// <summary>
        /// Gets the south connection.
        /// </summary>
        /// <value>
        /// The south connection.
        /// </value>
        public int SouthConnection { get; private set; } = -1;

        /// <summary>
        /// Gets the west connection.
        /// </summary>
        /// <value>
        /// The west connection.
        /// </value>
        public int WestConnection { get; private set; } = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionIndices"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="rows">The rows.</param>
        public ConnectionIndices(ConnectionOptions options, int columns, int rows)
        {
            if (options != null && options.ConnectSouth)
            {
                this.SouthConnection = Range(1, columns);
            }

            if (options != null && options.ConnectNorth)
            {
                this.NorthConnection = Range(0, columns);
            }

            if (options != null && options.ConnectEast)
            {
                this.EastConnection = Range(1, rows - 1);
            }

            if (options != null && options.ConnectWest)
            {
                this.WestConnection = Range(1, rows - 1);
            }

        }
    }
}
