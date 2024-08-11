namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    using System.Collections;
    using System.Collections.Generic;

    using OldSchoolGames.HuntTheMuglump.Scripts.Extensions;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;

    public class Region : IEnumerable<RoomBehaviour>
    {
        private HashSet<RoomBehaviour> adjacentRooms;

        private HashSet<RoomBehaviour> nearSurroundingRooms;

        private HashSet<RoomBehaviour> outerSurroundingRooms;

        private HashSet<RoomBehaviour> allRooms = new HashSet<RoomBehaviour>();

        public Region(HashSet<RoomBehaviour> adjacentRooms, HashSet<RoomBehaviour> nearSurroundingRooms, HashSet<RoomBehaviour> outerSurroundingRooms)
        {
            this.adjacentRooms = adjacentRooms;
            this.nearSurroundingRooms = nearSurroundingRooms;
            this.outerSurroundingRooms = outerSurroundingRooms;
            this.allRooms.AddRange(this.adjacentRooms);
            this.allRooms.AddRange(this.nearSurroundingRooms);
            this.allRooms.AddRange(this.outerSurroundingRooms);
        }

        public HashSet<RoomBehaviour> AdjacentRooms { get => this.adjacentRooms; }

        public HashSet<RoomBehaviour> NearSurroundingRooms { get => this.nearSurroundingRooms; }

        public HashSet<RoomBehaviour> OuterSurroundingRooms { get => this.outerSurroundingRooms; }

        public IEnumerator<RoomBehaviour> GetEnumerator()
        {
            return this.allRooms.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.allRooms.GetEnumerator();
        }
    }
}
