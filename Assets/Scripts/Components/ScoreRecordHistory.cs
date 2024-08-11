/**************************************************
 *  ScoreRecordHistory.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides a history of top 10 scores
    /// </summary>
    [DataContract]
    public class ScoreRecordHistory : ICollection<ScoreRecord>
    {
        public const int MaxRecordCount = 10;
        private static readonly Comparison<ScoreRecord> comparison = (Tx, Ty) => Ty.CompareTo(Tx);

        private OrderedCollection<ScoreRecord> innerCollection = new OrderedCollection<ScoreRecord>(comparison, MaxRecordCount);

        /// <summary>
        /// Creates a new instance of the <see cref="ScoreRecordHistory" /> class.
        /// </summary>
        public ScoreRecordHistory() { }

        /// <summary>
        /// The inner collection
        /// </summary>
        [DataMember]
        internal IEnumerable<ScoreRecord> InnerCollection 
        {
            get => this.innerCollection.ToArray();

            set
            {
                if (value == null)
                {
                    return;
                }

                this.innerCollection = new OrderedCollection<ScoreRecord>(comparison, value);
            }
        }

        public int Count
        {
            get { return this.innerCollection.Count; }
        }

        public bool IsReadOnly { get => false; }

        /// <summary>
        /// Adds a new score record.
        /// </summary>
        /// <param name="scoreRecord"></param>
        public void Add(ScoreRecord scoreRecord)
        {
            this.innerCollection.Add(scoreRecord);
            
            while (this.innerCollection.Count > MaxRecordCount)
            {
                this.innerCollection.RemoveAt(this.innerCollection.Count - 1);
            }
        }

        /// <summary>
        /// Gets the <see cref="ScoreRecord" /> instance found at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index</param>
        /// <returns>The <see cref="ScoreRecord" /></returns>
        public ScoreRecord this[int index] { get => this.innerCollection[index]; }

        public void Clear()
        {
            this.innerCollection.Clear();
        }

        public bool Contains(ScoreRecord item)
        {
            return this.innerCollection.Contains(item);
        }

        public void CopyTo(ScoreRecord[] array, int count)
        {
            this.innerCollection.CopyTo(array, count);
        }

        public bool Remove(ScoreRecord item)
        {
            return this.innerCollection.Remove(item);
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>An <see cref="IEnumerator" /></returns>
        public IEnumerator<ScoreRecord> GetEnumerator()
        {
            return this.InnerCollection.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>An <see cref="IEnumerator" /></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.InnerCollection.GetEnumerator();
        }
    }
}
