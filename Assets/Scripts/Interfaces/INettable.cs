/**************************************************
 *  INettable.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Interfaces
{
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;

    /// <summary>
    /// Defines an interface for entities that may interact with a NetBehaviour.
    /// </summary>
    public interface INettable
    {
        /// <summary>
        /// Gets the net.
        /// </summary>
        /// <value>
        /// The net.
        /// </value>
        NetBehaviour Net { get; }

        /// <summary>
        /// Applies the net.
        /// </summary>
        /// <param name="net">The net.</param>
        void ApplyNet(NetBehaviour net);
    }
}