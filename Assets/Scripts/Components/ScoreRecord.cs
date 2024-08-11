/**************************************************
 *  ScoreRecord.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a single score record for a playthrough of Hunt the Muglump
    /// </summary>
    [DataContract]
    public class ScoreRecord : IComparable<ScoreRecord>
    { 
        /// <summary>
        /// Creates a new instance of the <see cref="ScoreRecord" /> class
        /// </summary>
        public ScoreRecord() { }

        /// <summary>
        /// Creates a new instance of teh <see cref="ScoreRecord" /> class
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="score">The score</param>
        /// <param name="gameOverSettings">The game over settings</param>
        /// <param name="timeStamp">The timestamp</param>
        public ScoreRecord(Player player, int score, GameOverSettings gameOverSettings, DateTime timeStamp)
        {
            this.Player = player;
            this.Score = score;
            this.GameOverSettings = gameOverSettings;
            this.TimeStamp = timeStamp;
        }

        /// <summary>
        /// Gets or sets the player
        /// </summary>
        [DataMember]
        public Player Player { get; internal set; }

        /// <summary>
        /// Gets or sets the score
        /// </summary>
        [DataMember]
        public int Score { get; internal set; }

        /// <summary>
        /// Gets or sets the Game Over Settings
        /// </summary>
        [DataMember]
        public GameOverSettings GameOverSettings { get; internal set; }

        /// <summary>
        /// Gets or sets the Timestamp
        /// </summary>
        [DataMember]
        public DateTime TimeStamp { get; internal set; }

        /// <summary>
        /// Compares this instance to another instance.  The comparison is based on the score value.
        /// If the scores are equivalent, then the record with the most recent timestamp is considered
        /// to be greater.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>The comparison result</returns>
        public int CompareTo(ScoreRecord other)
        {
            if (other == null)
            {
                return 1;
            }

            var result = this.Score - other.Score;

            if (result == 0)
            {
                result = this.TimeStamp.CompareTo(other.TimeStamp);
            }

            return result;
        }
    }
}
