namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class Path : IEnumerable<KeyValuePair<Direction, RoomBehaviour>>, IComparable<Path>
    {
        private Stack<KeyValuePair<Direction, RoomBehaviour>> innerPath;

        private Stack<KeyValuePair<Direction, RoomBehaviour>> InnerPath
        {
            get
            {
                if (this.innerPath == null)
                {
                    this.innerPath = new Stack<KeyValuePair<Direction, RoomBehaviour>>();
                }

                return this.innerPath;
            }
        }

        public int Length { get => this.InnerPath.Count; }

        public KeyValuePair<Direction, RoomBehaviour> Peek()
        {
            return this.InnerPath.Peek();
        }

        public KeyValuePair<Direction, RoomBehaviour> PeekLast()
        {
            return this.innerPath.Last();
        }

        public KeyValuePair<Direction, RoomBehaviour> Pop()
        {
            return this.InnerPath.Pop();
        }

        public void Push(KeyValuePair<Direction, RoomBehaviour> step)
        {
            this.InnerPath.Push(step);
        }

        public IEnumerator<KeyValuePair<Direction, RoomBehaviour>> GetEnumerator()
        {
            return this.InnerPath.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.InnerPath.GetEnumerator();
        }

        public static explicit operator Path(Stack<KeyValuePair<Direction, RoomBehaviour>> stack)
        {
            var path = new Path();
            path.innerPath = stack;

            return path;
        }

        public int CompareTo(Path other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.Length - other.Length;
        }

        public static bool operator >(Path lhs, Path rhs)
        {
            if (((object)lhs) == null)
            {
                return false;
            }

            return lhs.CompareTo(rhs) > 0;
        }

        public static bool operator >=(Path lhs, Path rhs)
        {
            if (((object)lhs) == null)
            {
                return ((object)rhs) == null;
            }

            return lhs.CompareTo(rhs) > -1;
        }

        public static bool operator <(Path lhs, Path rhs)
        {
            if (((object)lhs) == null)
            {
                return ((object)rhs) != null;
            }

            return lhs.CompareTo(rhs) < 0;
        }

        public static bool operator <=(Path lhs, Path rhs)
        {
            if (((object)lhs) == null)
            {
                return true;
            }

            return lhs.CompareTo(rhs) < 1;
        }
    }
}
