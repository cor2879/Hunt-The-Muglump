/**************************************************
 *  SectionBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Components
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using static UnityEngine.Object;
    using static UnityEngine.Random;

    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines a data structure that is part of the building blocks of a dungeon.
    /// Sections are rectangular grids that contain six or more rooms.  Each section
    /// has zero to four connections which may be used to connect to another section.
    /// Multiple sections joined together are used to create a dungeon.
    /// </summary>
    public class Section
    {
        /// <summary>
        /// The identifier pool
        /// </summary>
        private static int idPool = 0;

        /// <summary>
        /// The rooms
        /// </summary>
        private RoomBehaviour[][] rooms;

        /// <summary>
        /// The rows
        /// </summary>
        private int rows;

        /// <summary>
        /// The columns
        /// </summary>
        private int columns;

        /// <summary>
        /// The room width in sprites
        /// </summary>
        private int roomWidthInSprites;

        /// <summary>
        /// The room height in sprites
        /// </summary>
        private int roomHeightInSprites;

        /// <summary>
        /// The starting position
        /// </summary>
        private Vector3 startingPosition;

        /// <summary>
        /// The parent
        /// </summary>
        private Transform parent;

        /// <summary>
        /// The dungeon
        /// </summary>
        private DungeonBehaviour dungeon;

        /// <summary>
        /// The room prefabs
        /// </summary>
        private RoomPrefabs roomPrefabs;

        /// <summary>
        /// The connections
        /// </summary>
        private Dictionary<Direction, Connection> connections = new Dictionary<Direction, Connection>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="width">The width.</param>
        /// <param name="roomPrefabs">The room prefabs.</param>
        /// <param name="roomWidthInSprites">The room width in sprites.</param>
        /// <param name="roomHeightInSprites">The room height in sprites.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="dungeon">The dungeon.</param>
        /// <param name="options">The options.</param>
        public Section(int height, int width, RoomPrefabs roomPrefabs,
                int roomWidthInSprites, int roomHeightInSprites, Transform parent, DungeonBehaviour dungeon,
                ConnectionOptions options)
        {
            this.Id = idPool++;
            this.startingPosition = Vector3.zero;
            this.rows = height;
            this.columns = width;
            this.parent = parent;
            this.dungeon = dungeon;
            this.roomPrefabs = roomPrefabs;
            this.roomWidthInSprites = roomWidthInSprites;
            this.roomHeightInSprites = roomHeightInSprites;

            if (options?.ExistingConnection != null)
            {
                var opposingDirection = Direction.GetOpposingDirection(options.ExistingConnection.Direction);

                switch (opposingDirection.Value)
                {
                    case Direction.DirectionValue.North:
                        options.ConnectNorth = true;
                        break;
                    case Direction.DirectionValue.East:
                        options.ConnectEast = true;
                        break;
                    case Direction.DirectionValue.South:
                        options.ConnectSouth = true;
                        break;
                    case Direction.DirectionValue.West:
                        options.ConnectWest = true;
                        break;
                }
            }

            this.Rooms = BuildSection(options);

            this.Connections = BuildConnections(options);
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the rooms.
        /// </summary>
        /// <value>
        /// The rooms.
        /// </value>
        public RoomBehaviour[][] Rooms
        {
            get
            {
                return this.rooms;
            }

            private set
            {
                this.rooms = value;
            }
        }

        /// <summary>
        /// Gets the connections.
        /// </summary>
        /// <value>
        /// The connections.
        /// </value>
        public Dictionary<Direction, Connection> Connections
        {
            get { return this.connections; }
            private set { this.connections = value; }
        }

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <returns></returns>
        public Vector2[] GetBounds()
        {
            return new Vector2[4]
{                new Vector2(
                    this.Rooms.Last().Last().Position.x - (this.roomWidthInSprites / 2),
                    this.Rooms.Last().Last().Position.y + (this.roomHeightInSprites / 2)
                    ),
                new Vector2(
                    this.Rooms.Last().First().Position.x + (this.roomWidthInSprites / 2),
                    this.Rooms.Last().First().Position.y + (this.roomHeightInSprites / 2)
                    ),
                new Vector2(
                    this.Rooms.First().First().Position.x + (this.roomWidthInSprites / 2),
                    this.Rooms.First().First().Position.y - (this.roomHeightInSprites / 2)
                    ),
                new Vector2(
                    this.Rooms.First().Last().Position.x - (this.roomWidthInSprites / 2),
                    this.Rooms.First().Last().Position.y - (this.roomHeightInSprites / 2)
                    )};
        }

        public IEnumerable<RoomBehaviour> GetFlattenedRoomCollection()
        {
            return this.Rooms.SelectMany(room => room).ToArray();
        }

        public Dictionary<Direction, Section> GetAdjacentSections()
        {
            var sections = new Dictionary<Direction, Section>();

            foreach (var connection in this.Connections.Values)
            {
                sections.Add(connection.Direction, connection.Room.GetAdjacentRoom(connection.Direction)?.Section);
            }

            return sections;
        }

        /// <summary>
        /// Determines whether [is south edge] [the specified row index].
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns>
        ///   <c>true</c> if [is south edge] [the specified row index]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsSouthEdge(int rowIndex)
        {
            return rowIndex == 0;
        }

        /// <summary>
        /// Determines whether [is east edge] [the specified column index].
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>
        ///   <c>true</c> if [is east edge] [the specified column index]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsEastEdge(int columnIndex)
        {
            return columnIndex == 0;
        }

        /// <summary>
        /// Determines whether [is north edge] [the specified row index].
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns>
        ///   <c>true</c> if [is north edge] [the specified row index]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsNorthEdge(int rowIndex)
        {
            return rowIndex == this.rows - 1;
        }

        /// <summary>
        /// Determines whether [is west edge] [the specified column index].
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>
        ///   <c>true</c> if [is west edge] [the specified column index]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsWestEdge(int columnIndex)
        {
            return columnIndex == this.columns - 1;
        }

        /// <summary>
        /// Selects the prefab collection.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        /// <param name="connectionIndices">The connection indices.</param>
        /// <param name="isConnection">if set to <c>true</c> [is connection].</param>
        /// <returns></returns>
        private List<GameObject> SelectPrefabCollection(int row, int column, ConnectionIndices connectionIndices, out bool isConnection)
        {
            if (IsSouthEdge(row))
            {
                if (IsEastEdge(column))
                {
                    return (isConnection = connectionIndices.SouthConnection == column) ?
                        this.roomPrefabs.EdgeEastPrefabs :
                        this.roomPrefabs.CornerSouthEastPrefabs;
                }
                else if (IsWestEdge(column))
                {
                    return (isConnection = connectionIndices.SouthConnection == column) ?
                        this.roomPrefabs.EdgeWestPrefabs :
                        this.roomPrefabs.CornerSouthWestPrefabs;
                }
                else
                {
                    return (isConnection = connectionIndices.SouthConnection == column) ?
                        this.roomPrefabs.InnerRoomPrefabs :
                        this.roomPrefabs.EdgeSouthPrefabs;
                }
            }
            else if (IsNorthEdge(row))
            {
                if (IsEastEdge(column))
                {
                    return (isConnection = connectionIndices.NorthConnection == column) ?
                        this.roomPrefabs.EdgeEastPrefabs :
                        this.roomPrefabs.CornerNorthEastPrefabs;
                }
                else if (IsWestEdge(column))
                {
                    return (isConnection = connectionIndices.NorthConnection == column) ?
                        this.roomPrefabs.EdgeWestPrefabs :
                        this.roomPrefabs.CornerNorthWestPrefabs;
                }
                else
                {
                    return (isConnection = connectionIndices.NorthConnection == column) ?
                        this.roomPrefabs.InnerRoomPrefabs :
                        this.roomPrefabs.EdgeNorthPrefabs;
                }
            }
            if (this.IsEastEdge(column))
            {
                return (isConnection = connectionIndices.EastConnection == row) ?
                    this.roomPrefabs.InnerRoomPrefabs :
                    this.roomPrefabs.EdgeEastPrefabs;
            }
            else if (this.IsWestEdge(column))
            {
                return (isConnection = connectionIndices.WestConnection == row) ?
                    this.roomPrefabs.InnerRoomPrefabs :
                    this.roomPrefabs.EdgeWestPrefabs;
            }
            else
            {
                isConnection = false;
                return this.roomPrefabs.InnerRoomPrefabs;
            }
        }

        /// <summary>
        /// Builds the section.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        private RoomBehaviour[][] BuildSection(ConnectionOptions options)
        {
            var roomPosition = this.startingPosition;
            var rowOffsetVector = new Vector3(roomWidthInSprites, 0);
            var columnOffsetVector = new Vector3(0, roomHeightInSprites);
            var rowPosition = roomPosition;
            var section = new RoomBehaviour[rows][];
            var connectionIndices = new ConnectionIndices(options, columns, rows);

            for (var row = 0; row < rows; row++)
            {
                section[row] = new RoomBehaviour[columns];

                for (var column = 0; column < columns; column++)
                {
                    int prefabSelector = -1;
                    GameObject roomPrefab = null;
                    var isConnection = false;
                    var prefabCollection = this.SelectPrefabCollection(row, column, connectionIndices, out isConnection);

                    prefabSelector = Range(0, prefabCollection.Count);
                    roomPrefab = prefabCollection[prefabSelector];

                    var room = Instantiate(roomPrefab, roomPosition, Quaternion.identity);

                    room.transform.parent = parent;

                    roomPosition -= rowOffsetVector;

                    section[row][column] = room.gameObject.GetComponent<RoomBehaviour>();
                    section[row][column].IsConnection = isConnection;
                    section[row][column].RowPosition = row;
                    section[row][column].ColumnPosition = column;
                    section[row][column].Section = this;
                    section[row][column].Dungeon = dungeon;

                    if (column > 0)
                    {
                        section[row][column].SetAdjacentRoom(Direction.East, section[row][column - 1]);
                        section[row][column - 1].SetAdjacentRoom(Direction.West, section[row][column]);
                    }

                    if (row > 0)
                    {
                        section[row][column].SetAdjacentRoom(Direction.South, section[row - 1][column]);
                        section[row - 1][column].SetAdjacentRoom(Direction.North, section[row][column]);
                    }
                }

                rowPosition += columnOffsetVector;
                roomPosition = rowPosition;
            }

            return section;
        }

        /// <summary>
        /// Builds the connections.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        private Dictionary<Direction, Connection> BuildConnections(ConnectionOptions options)
        {
            var connections = new Dictionary<Direction, Connection>();
            Direction existingConnectionOpposingDirection = null;

            if (options?.ExistingConnection != null)
            {
                existingConnectionOpposingDirection = Direction.GetOpposingDirection(options?.ExistingConnection.Direction);
            }

            var northConnectionRoom = this.Rooms.Last().Where(room => room.IsConnection).FirstOrDefault();

            if (northConnectionRoom != null)
            {
                connections.Add(
                    Direction.North,
                    new Connection(
                        northConnectionRoom,
                        Direction.North));
            }

            var southConnectionRoom = this.Rooms.First().Where(room => room.IsConnection).FirstOrDefault();

            if (southConnectionRoom != null)
            {
                connections.Add(
                    Direction.South,
                    new Connection(
                        southConnectionRoom,
                        Direction.South));
            }

            var eastConnectionRoom = this.Rooms.Where(array => !array[0].IsEntrance && array[0].IsConnection).FirstOrDefault()?[0];

            if (eastConnectionRoom != null)
            {
                connections.Add(
                    Direction.East,
                    new Connection(
                        eastConnectionRoom,
                        Direction.East));
            }

            var westConnectionRoom = this.Rooms.Where(array => array.Last().IsConnection).FirstOrDefault()?.Last();

            if (westConnectionRoom != null)
            {
                connections.Add(
                    Direction.West,
                    new Connection(
                        westConnectionRoom,
                        Direction.West));
            }

            if (options?.ExistingConnection != null)
            {
                connections[existingConnectionOpposingDirection]
                    .Room.SetAdjacentRoom(existingConnectionOpposingDirection, options.ExistingConnection.Room);

                options.ExistingConnection.Room.SetAdjacentRoom(
                    options.ExistingConnection.Direction, 
                    connections[existingConnectionOpposingDirection].Room);

                this.OffsetPositionsFromConnection(options.ExistingConnection);
            }

            return connections;
        }

        /// <summary>
        /// Offsets the positions of each room in the section using the connection from a previous section
        /// as the starting point for determining the relative positions.
        /// </summary>
        /// <param name="existingConnection">The existing connection.</param>
        private void OffsetPositionsFromConnection(Connection existingConnection)
        {
            var visited = new HashSet<RoomBehaviour>
            {
                existingConnection.Room
            };

            var room = existingConnection.Room.GetAdjacentRoom(existingConnection.Direction);

            this.OffsetRoom(room, existingConnection.Room.Position, existingConnection.Direction, visited);
        }

        /// <summary>
        /// Gets the offset vector.
        /// </summary>
        /// <param name="startingPosition">The starting position.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        private Vector3 GetOffsetVector(Vector3 startingPosition, Direction direction)
        {
            return startingPosition + new Vector3(
                this.roomWidthInSprites * direction.XValue, 
                this.roomHeightInSprites * direction.YValue);
        }

        /// <summary>
        /// Offsets the room.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <param name="previousVector">The previous vector.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="visited">The visited.</param>
        private void OffsetRoom(RoomBehaviour room, Vector3 previousVector, Direction direction, HashSet<RoomBehaviour> visited)
        {
            Vector3 offsetVector = GetOffsetVector(previousVector, direction);

            room.transform.position = offsetVector;

            visited.Add(room);

            var adjacentRooms = room.GetAdjacentRooms().Where(kvp => kvp.Value != null);

            foreach (var pair in adjacentRooms)
            {
                if (pair.Value != null && !visited.Contains(pair.Value))
                {
                    OffsetRoom(pair.Value, offsetVector, pair.Key, visited);
                }
            }
        }
    }
}
