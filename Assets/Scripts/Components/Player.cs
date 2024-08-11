/**************************************************
 *  Player.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a player
    /// </summary>
    [DataContract]
    public class Player : IEquatable<Player>
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        public override int GetHashCode()
        {
            return (this.Name ?? string.Empty).GetHashCode();
        }

        /// <summary>
        /// Tests whether or not this instance is equivalent to another object.
        /// </summary>
        /// <param name="other">The other object</param>
        /// <returns><c>true</c> if the other object is equivalent to this player, otherwise <c>false</c>.</returns>
        public override bool Equals(Object other)
        {
            return this.Equals(other as Player);
        }

        /// <summary>
        /// Tests whether or not this instance is equivalent to another <see cref="Player" /> instance
        /// </summary>
        /// <param name="other">The other player</param>
        /// <returns><c>true</c> if the other player is equivalent to this player, otherwise <c>false</c>.</returns>
        public bool Equals(Player other)
        {
            if (((object)other) == null)
            {
                return false;
            }

            if (this.Name == null)
            {
                return other.Name == null;
            }

            return this.Name.Equals(other.Name);
        }

        /// <summary>
        /// Tests whether or not a <see cref="Player" /> instance is equivalent to another instance.
        /// </summary>
        /// <param name="lhs">The left hand operand</param>
        /// <param name="rhs">The right hand operand</param>
        /// <returns><c>true</c> if the two operands are equivalent, otherwise <c>false</c>.</returns>
        public static bool operator ==(Player lhs, object rhs)
        {
            if (lhs == null)
            {
                return rhs == null;
            }

            return lhs.Equals(rhs as Player);
        }

        /// <summary>
        /// Tests whether or not a <see cref="Player" /> instance is not equivalent to another instance.
        /// </summary>
        /// <param name="lhs">The left hand operand</param>
        /// <param name="rhs">The right hand operand</param>
        /// <returns><c>true</c> if the two operands are equivalent, otherwise <c>false</c>.</returns>
        public static bool operator !=(Player lhs, object rhs)
        {
            if (lhs == null)
            {
                return rhs != null;
            }

            return !(lhs.Equals(rhs as Player));
        }
    }
}
