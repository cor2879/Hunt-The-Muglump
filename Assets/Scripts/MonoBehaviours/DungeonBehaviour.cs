/**************************************************
 *  DungeonBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the behaviours for the Dungeon.  This is a critical script in
    /// Hunt the Muglump since it owns all of the code responsible for building
    /// and maintaining the dungeon during gameplay.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(RoomPrefabs))]
    public class DungeonBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The rectangle that is defined by the four extreme bounds of this dungeon.
        /// The positions of the rectangle edges are determined after the dungeon has
        /// been constructed.
        /// </summary>
        [SerializeField, ReadOnly]
        private Rectangle rectangle;

        /// <summary>
        /// The collection of rooms that comprise the dungeon
        /// </summary>
        private List<RoomBehaviour> dungeon;

        /// <summary>
        /// The sections
        /// </summary>
        private List<Section> sections = new List<Section>();

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        public PlayerBehaviour Player { get; private set; }

        /// <summary>
        /// Gets the collection of rooms that comprise the dungeon.
        /// </summary>
        /// <value>
        /// The dungeon.
        /// </value>
        public List<RoomBehaviour> Dungeon
        {
            get => this.dungeon;
        }

        /// <summary>
        /// Gets the rectangle that is defined by the four extreme bounds of this dungeon.
        /// The positions of the rectangle edges are determined after the dungeon has
        /// been constructed.
        /// </summary>
        /// <value>
        /// The rectangle.
        /// </value>
        public Rectangle Rectangle
        {
            get => this.rectangle;
            private set => this.rectangle = value;
        }

        /// <summary>
        /// Gets the collider.
        /// </summary>
        /// <value>
        /// The collider.
        /// </value>
        public Collider2D Collider
        {
            get => this.GetComponent<PolygonCollider2D>();
        }

        /// <summary>
        /// Gets the entrance.
        /// </summary>
        /// <returns></returns>
        public RoomBehaviour GetEntrance()
        {
            return this.dungeon.Where(room => room.IsEntrance).FirstOrDefault();
        }

        /// <summary>
        /// Executed during the Start event of the GameObject life cycle.
        /// </summary>
        void Start()
        {
            GameManager.Instance.Dungeon = this;

            this.BuildDungeon();
            this.AddOccupants();
            this.AddEntrance();
            this.AddPlayer();
            StartCoroutine(nameof(this.RenderDungeon));
        }

        /// <summary>
        /// Builds the dungeon using parameters defined in the Difficulty Settings
        /// </summary>
        private void BuildDungeon()
        {
            var collider = this.GetComponent<PolygonCollider2D>();

            dungeon = new List<RoomBehaviour>(Settings.Difficulty.MinimumRoomCount * 2);

            var directionRoll = Random.Range(0, 4);
            var direction = Direction.Idle;

            switch (directionRoll)
            {
                case 0:
                    direction = Direction.North;
                    break;
                case 1:
                    direction = Direction.East;
                    break;
                case 2:
                    direction = Direction.South;
                    break;
                case 3:
                    direction = Direction.West;
                    break;
            }

            this.BuildSections(direction);

            var bounds = this.GetBounds().ToArray();
            collider.points = bounds;
            this.Rectangle = Rectangle.GetRectangle(bounds);
        }

        /// <summary>
        /// Builds the sections.
        /// </summary>
        /// <param name="direction">The direction.</param>
        private void BuildSections(Direction direction)
        {
            const int minSectionSize = 3;
            const int maxSectionSize = 9;

            var connectionOptions = new ConnectionOptions();

            switch (direction?.Value)
            {
                case Direction.DirectionValue.North:
                    connectionOptions.ConnectNorth = true;
                    break;
                case Direction.DirectionValue.East:
                    connectionOptions.ConnectEast = true;
                    break;
                case Direction.DirectionValue.South:
                    connectionOptions.ConnectSouth = true;
                    break;
                case Direction.DirectionValue.West:
                    connectionOptions.ConnectWest = true;
                    break;
            }

            Section currentSection = null;

            while (this.dungeon.Count < Settings.Difficulty.MinimumRoomCount)
            {
                currentSection = new Section(
                    Random.Range(minSectionSize, maxSectionSize),
                    Random.Range(minSectionSize, maxSectionSize),
                    RoomPrefabs.Instance,
                    Constants.RoomHeight,
                    Constants.RoomWidth,
                    this.gameObject.transform,
                    this,
                    connectionOptions);

                foreach (var row in currentSection.Rooms)
                {
                    foreach (var roomBehaviour in row)
                    {
                        this.Dungeon.Add(roomBehaviour);
                    }
                }

                this.sections.Add(currentSection);
                connectionOptions.ExistingConnection = currentSection.Connections[direction];
            }

            connectionOptions.DisableAllDirections();

            currentSection = new Section(
                minSectionSize,
                minSectionSize,
                RoomPrefabs.Instance,
                Constants.RoomHeight,
                Constants.RoomWidth,
                this.gameObject.transform,
                this,
                connectionOptions);

            foreach (var row in currentSection.Rooms)
            {
                foreach (var roomBehaviour in row)
                {
                    this.Dungeon.Add(roomBehaviour);
                }
            }

            this.sections.Add(currentSection);
        }

        /// <summary>
        /// Adds the occupants.
        /// </summary>
        private void AddOccupants()
        {
            for (var batCount = 0; batCount < Settings.Difficulty.BatCount; batCount++)
            {
                var room = GetRandomUnoccupiedRoom();

                var flock = Instantiate(GameManager.Instance.flockOfBatsPrefab, room.Position, Quaternion.identity);
                var flockBehaviour = flock.GetComponent<FlockBehaviour>();
                flockBehaviour.MoveToRoom(room);
            }

            for (var muglumpCount = 0; muglumpCount < Settings.Difficulty.MuglumpCount; muglumpCount++)
            {
                var room = GetRandomUnoccupiedRoom();

                var goldMuglumpChance = Random.Range(0, Settings.GoldMuglumpDiceRoll) % Settings.GoldMuglumpDiceRoll;

                var muglumpPrefab = default(GameObject);

                if (goldMuglumpChance <= Settings.GoldMuglumpCriticalRange)
                {
                    muglumpPrefab = GameManager.Instance.goldMuglumpPrefab;
                    GameManager.Instance.GoldMuglumpCount++;
                }
                else
                {
                    muglumpPrefab = GameManager.Instance.muglumpPrefab;
                    GameManager.Instance.RedMuglumpCount++;
                }

                this.AddEntity<MuglumpBehaviour>(muglumpPrefab, room);

                //var muglump = Instantiate(muglumpPrefab, room.Position, Quaternion.identity);

                //var muglumpBehaviour = muglump.GetComponent<MuglumpBehaviour>();
                //muglumpBehaviour.MoveToRoom(room);
            }

            for (var blueMuglumpCount = 0; blueMuglumpCount < Settings.Difficulty.BlueMuglumpCount; blueMuglumpCount++)
            {
                var room = GetRandomUnoccupiedRoom();

                var blueMuglump = Instantiate(GameManager.Instance.blueMuglumpPrefab, room.Position, Quaternion.identity);
                var blueMuglumpBehaviour = blueMuglump.GetComponent<BlueMuglumpBehaviour>();
                blueMuglumpBehaviour.MoveToRoom(room);
            }

            for (var blackMuglumpCount = 0; blackMuglumpCount < Settings.Difficulty.BlackMuglumpCount; blackMuglumpCount++)
            {
                var room = GetRandomUnoccupiedRoom();

                var blackMuglump = Instantiate(GameManager.Instance.blackMuglumpPrefab, room.Position, Quaternion.identity);
                var blackMuglumpBehaviour = blackMuglump.GetComponent<BlackMuglumpBehaviour>();
                blackMuglumpBehaviour.MoveToRoom(room);
            }

            for (var silverbackMuglumpCount = 0; silverbackMuglumpCount < Settings.Difficulty.SilverbackMuglumpCount; silverbackMuglumpCount++)
            {
                var room = GetRandomUnoccupiedRoom();

                this.AddEntity<SilverbackMuglumpBehaviour>(GameManager.Instance.silverbackMuglumpPrefab, room);

                //var silverbackMuglump = Instantiate(GameManager.Instance.silverbackMuglumpPrefab, room.Position, Quaternion.identity);
                //var silverbackMuglumpBehaviour = silverbackMuglump.GetComponent<SilverbackMuglumpBehaviour>();
                //silverbackMuglumpBehaviour.MoveToRoom(room);
            }

            for (var pitCount = 0; pitCount < Settings.Difficulty.PitCount; pitCount++)
            {
                var room = GetRandomRoom(r => r.CanHavePitTrap && !r.IsConnection && !r.IsEntrance && !r.Occupants.Any());

                if (room == null)
                {
                    continue;
                }
                var pit = Instantiate(GameManager.Instance.pitPrefab, room.Position, Quaternion.identity);
                var pitBehaviour = pit.GetComponent<PitBehaviour>();
                pitBehaviour.MoveToRoom(room);
            }

            var crownRoom = this.GetRandomUnoccupiedRoom();
            var crown = Instantiate(GameManager.Instance.crownPrefab, crownRoom.Position, Quaternion.identity);
            var crownBehaviour = crown.GetComponent<CrownBehaviour>();
            crownBehaviour.MoveToRoom(crownRoom);
        }

        public TEntity AddEntity<TEntity>(GameObject prefab, RoomBehaviour room) where TEntity : EntityBehaviour
        {
            try
            {
                var gameObject = Instantiate(prefab, room.Position, Quaternion.identity);

                var behaviour = gameObject.GetComponent<TEntity>();
                behaviour.MoveToRoom(room);


                return behaviour;
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex);
            }

            return null;
        }

        /// <summary>
        /// Adds the entrance.
        /// </summary>
        public void AddEntrance()
        {
            var room = GetRandomRoom(r => !r.Occupants.Any() && r.CanHaveEntrance);
            var entrance = Instantiate(GameManager.Instance.entrancePrefab, room.Position, Quaternion.identity);
            var entranceBehaviour = entrance.GetComponent<EntranceBehaviour>();
            entranceBehaviour.MoveToRoom(room);
            room.IsEntrance = true;
        }

        /// <summary>
        /// Adds the player.
        /// </summary>
        public void AddPlayer()
        {
            var entrance = this.GetEntrance();
            var player = Instantiate(GameManager.Instance.playerPrefab, entrance.Transform);
            this.Player = player.GetComponent<PlayerBehaviour>();
            GameManager.Instance.Player = this.Player;
            this.Player.MoveToRoom(entrance);
            this.Player.Inventory.Initialize();

            if (!Settings.SurvivalMode)
            {
                this.Player.Inventory.AddArrows(Settings.Difficulty.ArrowCount);
            }
            else
            {
                this.AddArrows();
            }

            this.AddFlashArrows();
            this.AddNetArrows();
            this.AddCoverScent();
            this.AddBearTraps();

            foreach (var enabledBadge in Badge.GetStaticBadges().Where(badge => badge.Enabled))
            {
                enabledBadge.Apply();
            }

            CameraManager.InitializeCamera();
        }

        /// <summary>
        /// Adds the arrows.
        /// </summary>
        public void AddArrows()
        {
            for (var i = 0; i < Settings.Difficulty.ArrowCount; i++)
            {
                var room = this.GetRandomItemlessRoom();

                if (room == null)
                {
                    return;
                }

                var arrowItem = Instantiate(GameManager.Instance.arrowItemPrefab, room.Position, Quaternion.identity);
                var arrowItemBehaviour = arrowItem.GetComponent<ArrowItemBehaviour>();
                arrowItemBehaviour.MoveToRoom(room);
            }
        }

        public void AddFlashArrows()
        {
            for (var i = 0; i < Settings.Difficulty.FlashArrowCount; i++)
            {
                var room = this.GetRandomItemlessRoom();

                if (room == null)
                {
                    return;
                }

                var arrowItem = Instantiate(GameManager.Instance.flashArrowItemPrefab, room.Position, Quaternion.identity);
                var arrowItemBehaviour = arrowItem.GetComponent<FlashArrowItemBehaviour>();
                arrowItemBehaviour.MoveToRoom(room);
            }
        }

        public void AddNetArrows()
        {
            for (var i = 0; i < Settings.Difficulty.NetArrowCount; i++)
            {
                var room = this.GetRandomItemlessRoom();

                if (room == null)
                {
                    return;
                }

                var arrowItem = Instantiate(GameManager.Instance.netArrowItemPrefab, room.Position, Quaternion.identity);
                var arrowItemBehaviour = arrowItem.GetComponent<NetArrowItemBehaviour>();
                arrowItemBehaviour.MoveToRoom(room);
            }
        }

        public void AddCoverScent()
        {
            for (var i = 0; i < Settings.Difficulty.CoverScentCount; i++)
            {
                var room = this.GetRandomItemlessRoom();

                if (room == null)
                {
                    return;
                }

                var coverScentItem = Instantiate(GameManager.Instance.coverScentItemPrefab, room.Position, Quaternion.identity);
                var coverScentItemBehaviour = coverScentItem.GetComponent<CoverScentItemBehaviour>();
                coverScentItemBehaviour.MoveToRoom(room);
            }
        }

        public void AddBearTraps()
        {
            for (var i = 0; i < Settings.Difficulty.BearTrapCount; i++)
            {
                var room = this.GetRandomItemlessRoom();

                if (room == null)
                {
                    return;
                }

                var bearTrapItem = Instantiate(GameManager.Instance.bearTrapItemPrefab, room.Position, Quaternion.identity);
                var bearTrapItemBehaviour = bearTrapItem.GetComponent<BearTrapItemBehaviour>();
                bearTrapItemBehaviour.MoveToRoom(room);
            }
        }

        /// <summary>
        /// Gets a random unoccupied room.
        /// </summary>
        /// <returns></returns>
        public RoomBehaviour GetRandomUnoccupiedRoom()
        {
            var unoccupiedRooms = this.dungeon.Where(room => !room.IsEntrance && !room.Occupants.Any());
            var randomRoomNumber = Random.Range(0, unoccupiedRooms.Count());

            try
            {
                return unoccupiedRooms.ElementAt(randomRoomNumber);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex);
            }

            return null;
        }

        public RoomBehaviour GetRandomItemlessRoom()
        {
            var rooms = this.dungeon.Where(room => !room.IsEntrance && !room.Findables.Any() && room.GetPitBehaviour() == null);
            var randomRoomNumber = Random.Range(0, rooms.Count());

            return rooms.ElementAtOrDefault(randomRoomNumber);
        }

        /// <summary>
        /// Gets a random room that satisfies the criteria defined in the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public RoomBehaviour GetRandomRoom(System.Predicate<RoomBehaviour> filter)
        {
            var rooms = this.dungeon.Where(room => filter(room));
            var randomNumber = Random.Range(0, rooms.Count());
            
            return rooms.ElementAtOrDefault(randomNumber);
        }

        public static RoomBehaviour GetRandomRoom(IEnumerable<RoomBehaviour> rooms, System.Predicate<RoomBehaviour> filter = null)
        {
            if (filter != null)
            {
                var filteredRooms = rooms.Where(room => filter(room)).ToArray();
                rooms = filteredRooms;
            }

            var randomNumber = Random.Range(0, rooms.Count());

            return rooms.ElementAtOrDefault(randomNumber);
        }

        /// <summary>
        /// Gets the bounds of all sections in the dungeon.
        /// </summary>
        /// <returns></returns>
        private List<Vector2> GetBounds()
        {
            var bounds = new List<Vector2>();

            foreach (var section in this.sections)
            {
                bounds.AddRange(section.GetBounds());
            }

            return bounds;
        }

        public void OptimizeRendering()
        {
            if (this.Dungeon == null)
            {
                return;
            }

            foreach (var room in this.Dungeon)
            {
                if (room.IsInView)
                {
                    room.ShowInView();
                }
                else
                {
                    room.HideFromView();
                }
            }
        }

        public void RenderAll()
        {
            foreach (var room in this.Dungeon)
            {
                room.ShowInView();
            }
        }

        public void RenderDungeonExternal()
        {
            StartCoroutine(nameof(this.RenderDungeon));
        }

        public IEnumerator RenderDungeon()
        {
            foreach (var room in this.Dungeon)
            {
                if (!GameManager.Instance.Minimap.IsEnabled && (Mathf.Abs(CameraManager.Instance.MainCamera.transform.position.x - room.gameObject.transform.position.x) > Constants.HearingRange ||
                    Mathf.Abs(CameraManager.Instance.MainCamera.transform.position.y - room.gameObject.transform.position.y) > Constants.HearingRange))
                {
                    room.IsInView = false;
                    room.HideFromView();
                }
                else
                {
                    room.IsInView = true;
                    room.ShowInView();
                }
            }

            yield return null;
        }
    }
}