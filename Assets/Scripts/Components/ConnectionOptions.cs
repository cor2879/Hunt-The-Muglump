/**************************************************
 *  ConnectionOptions.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    /// <summary>
    /// Defines options for setting up the ConnectionIndices data structure.
    /// </summary>
    public class ConnectionOptions
    {
        /// <summary>
        /// Gets or sets the existing connection.
        /// </summary>
        /// <value>
        /// The existing connection.
        /// </value>
        public Connection ExistingConnection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [connect north].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [connect north]; otherwise, <c>false</c>.
        /// </value>
        public bool ConnectNorth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [connect east].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [connect east]; otherwise, <c>false</c>.
        /// </value>
        public bool ConnectEast { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [connect south].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [connect south]; otherwise, <c>false</c>.
        /// </value>
        public bool ConnectSouth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [connect west].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [connect west]; otherwise, <c>false</c>.
        /// </value>
        public bool ConnectWest { get; set; }

        /// <summary>
        /// Disables all directions.
        /// </summary>
        public void DisableAllDirections()
        {
            this.ConnectNorth = false;
            this.ConnectEast = false;
            this.ConnectSouth = false;
            this.ConnectWest = false;
        }
    }
}
