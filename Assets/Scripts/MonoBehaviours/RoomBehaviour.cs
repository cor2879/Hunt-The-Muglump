/**************************************************
 *  RoomBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.Tilemaps;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Extensions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the behaviours for a Room object.  Rooms are the most basic
    /// building blocks of a dungeon.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(TilemapRenderer))]
    public class RoomBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The row position
        /// </summary>
        [ReadOnly, SerializeField]
        private int rowPosition;

        /// <summary>
        /// The column position
        /// </summary>
        [ReadOnly, SerializeField]
        private int columnPosition;

        /// <summary>
        /// The is entrance
        /// </summary>
        [ReadOnly, SerializeField]
        private bool isEntrance;

        /// <summary>
        /// The is connection
        /// </summary>
        [ReadOnly, SerializeField]
        private bool isConnection;

        /// <summary>
        /// The visited
        /// </summary>
        [ReadOnly, SerializeField]
        private bool visited;

        /// <summary>
        /// Indicates whether this room is suitable for containing an entrance
        /// </summary>
        [SerializeField]
        private bool canHaveEntrance = true;

        /// <summary>
        /// Indicates whether this room is suitable for containing a Pit Trap
        /// </summary>
        [SerializeField]
        private bool canHavePitTrap = true;

        /// <summary>
        /// The darkness
        /// </summary>
        [ReadOnly, SerializeField]
        private DarknessBehaviour darkness;

        /// <summary>
        /// The box collider
        /// </summary>
        [ReadOnly, SerializeField]
        private BoxCollider2D boxCollider;

        /// <summary>
        /// The adjacent rooms
        /// </summary>
        [ReadOnly, SerializeField]
        private Dictionary<Direction, RoomBehaviour> adjacentRooms = new Dictionary<Direction, RoomBehaviour>()
        {
            { Direction.North, null },
            { Direction.East, null },
            { Direction.South, null },
            { Direction.West, null }
        };

        [SerializeField, ReadOnly]
        private TilemapRenderer tilemapRenderer;

        /// <summary>
        /// The torches
        /// </summary>
        [SerializeField]
        private List<TorchBehaviour> torches = new List<TorchBehaviour>();

        /// <summary>
        /// Gets the torches.
        /// </summary>
        /// <value>
        /// The torches.
        /// </value>
        public List<TorchBehaviour> Torches
        {
            get => this.torches;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance contains the entrance.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is entrance; otherwise, <c>false</c>.
        /// </value>
        public bool IsEntrance { get => this.isEntrance; set => this.isEntrance = value; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a connection to another dungeon section.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is connection; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnection { get => this.isConnection; set => this.isConnection = value; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="RoomBehaviour"/> has been visited by the player.
        /// </summary>
        /// <value>
        ///   <c>true</c> if visited; otherwise, <c>false</c>.
        /// </value>
        public bool Visited
        {
            get => this.visited;

            private set
            {
                this.visited = value;

                if (this.visited)
                {
                    this.Darkness.Hide();

                    this.TurnOnLights();
                }
                else
                {
                    this.Darkness.Show();
                    this.TurnOffLights();
                }
            }
        }

        public bool IsInView { get; set; }

        public TilemapRenderer TilemapRenderer
        {
            get
            {
                if (this.tilemapRenderer == null)
                {
                    this.tilemapRenderer = this.GetComponent<TilemapRenderer>();
                }

                return this.tilemapRenderer;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can have an entrance.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can have entrance; otherwise, <c>false</c>.
        /// </value>
        public bool CanHaveEntrance { get => this.canHaveEntrance; private set => this.canHaveEntrance = value; }

        /// <summary>
        /// Gets a value indicating whether this instance can have a pit trap.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can have pit trap; otherwise, <c>false</c>.
        /// </value>
        public bool CanHavePitTrap { get => this.canHavePitTrap; private set => this.canHavePitTrap = value; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector3 Position { get => this.gameObject.transform.position; }

        /// <summary>
        /// Gets or sets the row position.
        /// </summary>
        /// <value>
        /// The row position.
        /// </value>
        public int RowPosition { get => this.rowPosition; set => this.rowPosition = value; }

        /// <summary>
        /// Gets or sets the column position.
        /// </summary>
        /// <value>
        /// The column position.
        /// </value>
        public int ColumnPosition { get => this.columnPosition; set => this.columnPosition = value; }

        /// <summary>
        /// Gets the transform.
        /// </summary>
        /// <value>
        /// The transform.
        /// </value>
        public Transform Transform { get => this.gameObject.transform; }

        /// <summary>
        /// Gets the occupants.
        /// </summary>
        /// <value>
        /// The occupants.
        /// </value>
        public HashSet<EntityBehaviour> Occupants { get; } = new HashSet<EntityBehaviour>();

        public HashSet<IFindable> Findables { get; } = new HashSet<IFindable>();

        /// <summary>
        /// Gets or sets the dungeon.
        /// </summary>
        /// <value>
        /// The dungeon.
        /// </value>
        public DungeonBehaviour Dungeon { get; set; }

        /// <summary>
        /// Gets or sets the section of the dungeon that this room belongs to
        /// </summary>
        public Section Section { get; set; }

        /// <summary>
        /// Gets the darkness.
        /// </summary>
        /// <value>
        /// The darkness.
        /// </value>
        public DarknessBehaviour Darkness
        {
            get
            {
                if (this.darkness == null)
                {
                    this.darkness = Instantiate(GameManager.Instance.darknessPrefab, this.gameObject.transform)?.GetComponent<DarknessBehaviour>();
                    this.darkness.SetScale(Constants.RoomDarknessScale, Constants.RoomDarknessScale);
                }

                return this.darkness;
            }
        }

        /// <summary>
        /// Gets the box collider.
        /// </summary>
        /// <value>
        /// The box collider.
        /// </value>
        private BoxCollider2D BoxCollider
        {
            get
            {
                if (this.boxCollider == null)
                {
                    this.boxCollider = this.GetComponent<BoxCollider2D>();
                }

                return this.boxCollider;
            }
        }

        public void AddSelfDestructClue(string message)
        {
            var clueBehaviour = Instantiate(GameManager.Instance.cluePrefab).GetComponent<ClueBehaviour>();

            clueBehaviour.CurrentRoom = this;
            clueBehaviour.Message = message;
            clueBehaviour.SelfDestruct = true;

            this.Occupants.Add(clueBehaviour);
        }

        /// <summary>
        /// Gets the item behaviour.
        /// </summary>
        /// <returns></returns>
        public IFindable GetFindable()
        {
            return this.Findables.FirstOrDefault();
        }

        /// <summary>
        /// Gets the arrow item behaviour.
        /// </summary>
        /// <returns></returns>
        public ArrowItemBehaviour GetArrowItemBehaviour()
        {
            return this.Findables.OfType<ArrowItemBehaviour>().Where(arrow => arrow.ArrowType == ArrowType.Arrow).FirstOrDefault();
        }

        public FlashArrowItemBehaviour GetFlashArrowItemBehaviour()
        {
            return this.Findables.OfType<FlashArrowItemBehaviour>().FirstOrDefault();
        }

        public NetArrowItemBehaviour GetNetArrowItemBehaviour()
        {
            return this.Findables.OfType<NetArrowItemBehaviour>().FirstOrDefault();
        }

        public CoverScentItemBehaviour GetCoverScentItemBehaviour()
        {
            return this.Findables.OfType<CoverScentItemBehaviour>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the bats.
        /// </summary>
        /// <returns></returns>
        public FlockBehaviour GetBats()
        {
            return this.Occupants.OfType<FlockBehaviour>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the clue.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClueBehaviour> GetClues()
        {
            return this.Occupants.OfType<ClueBehaviour>().ToList();
        }

        /// <summary>
        /// Gets the muglump behaviour.
        /// </summary>
        /// <returns></returns>
        public MuglumpBehaviour GetMuglumpBehaviour()
        {
            return this.Occupants.OfType<MuglumpBehaviour>().FirstOrDefault();
        }

        public MuglumpBehaviour GetRedMuglumpBehaviour()
        {
            return this.Occupants.OfType<MuglumpBehaviour>().Where(muglump => muglump.MuglumpType == MuglumpType.RedMuglump).FirstOrDefault();
        }

        public BlackMuglumpBehaviour GetBlackMuglumpBehaviour()
        {
            return this.Occupants.OfType<BlackMuglumpBehaviour>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the blue muglump behaviour.
        /// </summary>
        /// <returns></returns>
        public BlueMuglumpBehaviour GetBlueMuglumpBehaviour()
        {
            return this.Occupants.OfType<BlueMuglumpBehaviour>().FirstOrDefault();
        }

        public GoldMuglumpBehaviour GetGoldMuglumpBehaviour()
        {
            return this.Occupants.OfType<GoldMuglumpBehaviour>().FirstOrDefault();
        }

        public SilverbackMuglumpBehaviour GetSilverbackMuglumpBehaviour()
        {
            return this.Occupants.OfType<SilverbackMuglumpBehaviour>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the pit behaviour.
        /// </summary>
        /// <returns></returns>
        public PitBehaviour GetPitBehaviour()
        {
            return this.Occupants.OfType<PitBehaviour>().FirstOrDefault();
        }

        public BearTrapItemBehaviour GetBearTrapItemBehaviour()
        {
            return this.Findables.OfType<BearTrapItemBehaviour>().FirstOrDefault();
        }

        public BearTrapBehaviour GetBearTrapBehaviour()
        {
            return this.Occupants.OfType<BearTrapBehaviour>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the crown behaviour.
        /// </summary>
        /// <returns></returns>
        public CrownBehaviour GetCrownBehaviour()
        {
            return this.Occupants.OfType<CrownBehaviour>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the entrance behaviour.
        /// </summary>
        /// <returns></returns>
        public EntranceBehaviour GetEntranceBehaviour()
        {
            return this.Occupants.OfType<EntranceBehaviour>().FirstOrDefault();
        }

        public bool ContainsHazard()
        {
            var pitBehaviour = this.GetPitBehaviour();

            return (pitBehaviour != null && pitBehaviour.Net == null) || this.GetMuglumpBehaviour() != null;
        }

        /// <summary>
        /// Adds the specified entity to this room.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Enter(EntityBehaviour entity)
        {
            if (entity is IFindable findable)
            {
                this.Findables.Add(findable);
                entity.transform.SetParent(this.Transform);
                entity.transform.SetPositionAndRotation(this.transform.position + entity.GetIdleZeroPointOffsetVector(), Quaternion.identity);
                this.IsInView = true;
                return;
            }
            if (entity is PlayerBehaviour player)
            {
                this.Enter(player);
            }

            this.Occupants.Add(entity);
            entity.transform.SetParent(this.Transform);
            entity.transform.SetPositionAndRotation(this.transform.position + entity.GetIdleZeroPointOffsetVector(), Quaternion.identity);
            this.IsInView = true;
        }

        /// <summary>
        /// Adds the specified player to this room.
        /// </summary>
        /// <param name="player">The player.</param>
        public void Enter(PlayerBehaviour player)
        {
            GameManager.Instance.ClearMainWindowText();

            var explored = !this.Visited;
            this.Visited = true;

            if (explored)
            {
                Statistic.RoomsExplored.Value++;
            }

            var flockOfBats = this.GetBats();

            if (flockOfBats != null)
            {
                if (flockOfBats.HandleEncounter(player))
                {
                    return;
                }
            }

            var muglump = this.GetMuglumpBehaviour();

            if (muglump != null)
            {
                muglump.HandleEncounter(player);
                return;
            }

            var pit = this.GetPitBehaviour();

            if (pit != null)
            {
                if (pit.HandleEncounter(player))
                {
                    return;
                }
            }

            var crown = this.GetCrownBehaviour();

            if (crown != null)
            {
                crown.HandleEncounter(player);
            }

            var item = this.GetFindable();

            if (item != null)
            {
                item.HandleEncounter(player);
            }

            var entrance = this.GetEntranceBehaviour();

            if (entrance != null)
            {
                entrance.HandleEncounter(player);
            }

            this.GetWarnings();

            foreach (var torch in this.Torches)
            {
                torch.FlameBehaviour.SetImportant();
            }
        }

        /// <summary>
        /// Updates the state of this instance on a fixed interval determined by the Unity Engine at runtime.
        /// </summary>
        public void FixedUpdate()
        {
            if (this.Occupants.Contains(PlayerBehaviour.Instance))
            {
                this.TurnOnLights();
            }
        }

        /// <summary>
        /// Removes the specified Entity from this room.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Exit(EntityBehaviour entity)
        {
            if (entity is IFindable findable)
            {
                this.Findables.Remove(findable);
            }

            this.Occupants.Remove(entity);

            if (entity is PlayerBehaviour)
            {
                foreach (var torch in this.Torches)
                {
                    torch.FlameBehaviour.SetUnimportant();
                }

                var clues = this.GetClues().Where(clue => clue.SelfDestruct);

                if (clues.Any())
                {
                    foreach (var clue in clues)
                    {
                        clue.Destroy();
                    }
                }
            }
        }

        public IEnumerable<string> DetectNearbyHunters()
        {
            var messages = new List<string>();
            var adjacentRooms = new List<RoomBehaviour>(this.GetAdjacentRooms().Values.Where(r => r != null));
            var nearbyRooms = new HashSet<RoomBehaviour>(adjacentRooms);

            foreach (var room in adjacentRooms)
            {
                nearbyRooms.AddRange(room.GetAdjacentRooms().Values.Where(r => r != null));
            }

            if (nearbyRooms.Any(room => room.GetBlueMuglumpBehaviour() != null))
            {
                messages.Add(StringContent.BlueMuglumpDireWarning);
            }

            if (nearbyRooms.Any(room =>
            {
                var muglump = room.GetSilverbackMuglumpBehaviour();
                return muglump != null;
            }))
            {
                messages.Add(StringContent.SilverbackMuglumpDireWarning);
                var bossMusic = GameManager.Instance.MusicManager.GetAudioClip(SoundClips.Ominous);

                if (GameManager.Instance.MusicManager.BackgroundMusic != bossMusic)
                {
                    GameManager.Instance.MusicManager.SetBackgroundMusic(SoundClips.Ominous);
                }
            }
            else
            {
                var backgroundMusic = GameManager.Instance.MusicManager.GetAudioClip(SoundClips.CurrentBGM);

                if (GameManager.Instance.MusicManager.BackgroundMusic != backgroundMusic)
                {
                    GameManager.Instance.MusicManager.SetBackgroundMusic(SoundClips.CurrentBGM);
                }
            }

            return messages;
        }

        /// <summary>
        /// Gets the warnings.
        /// </summary>
        public void GetWarnings()
        {
            var messages = this.GetAdjacentRoomWarningMessages();

            var clues = this.GetClues();

            if (!clues.IsNullOrEmpty())
            {
                foreach (var clue in clues)
                {
                    messages.Add(clue.GetMessage());
                }
            }
            else
            {
                messages.AddRange(DetectNearbyHunters());
            }

            foreach (var message in messages)
            {
                GameManager.Instance.AppendLineMainWindowText(message);
                FeedbackManager.Instance.QuickVibration(0.1f);
            }

            if (this.Dungeon.Dungeon.Count(room => room.GetMuglumpBehaviour() != null) == 0)
            {
                if (Settings.Difficulty.EnableBossMode && !GameManager.Instance.HasDefeatedBoss)
                {
                    GameManager.Instance.StartBossSequence();
                }
                else
                {
                    GameManager.Instance.AppendLineMainWindowText(StringContent.AllMuglumpsHunted);
                }
            }
        }

        /// <summary>
        /// Gets the adjacent room found at the specified direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        public RoomBehaviour GetAdjacentRoom(Direction direction)
        {
            return this.adjacentRooms[direction];
        }

        /// <summary>
        /// Sets the adjacent room.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="room">The room.</param>
        public void SetAdjacentRoom(Direction direction, RoomBehaviour room)
        {
            this.adjacentRooms[direction] = room;
        }

        /// <summary>
        /// Gets the adjacent rooms.
        /// </summary>
        /// <returns></returns>
        public Dictionary<Direction, RoomBehaviour> GetAdjacentRooms()
        {
            return this.adjacentRooms;
        }

        public IEnumerable<Section> GetLocalSections(sbyte depth)
        {
            var sections = new HashSet<Section>();
            sections.Add(this.Section);

            for (var i = 0; i < depth && i < sections.Count(); i++)
            {
                var newSections = sections.ElementAt(i).GetAdjacentSections().Select(kvp => kvp.Value).Where(section => !sections.Contains(section) && section != null);

                if (newSections.Any())
                {
                    sections.AddRange(newSections);
                }
            }

            return sections;
        }

        public Region GetSurroundingRooms()
        {
            var adjacentRooms = new HashSet<RoomBehaviour>();
            var nearSurroundingRooms = new HashSet<RoomBehaviour>();
            var outerSurroundingRooms = new HashSet<RoomBehaviour>();
            adjacentRooms.Add(this);

            foreach (var room in this.GetAdjacentRooms().Values.Where(value => value != null))
            {
                adjacentRooms.Add(room);
            }

            foreach (var adjacentRoom in adjacentRooms)
            {
                foreach (var nearRoom in 
                    adjacentRoom.GetAdjacentRooms().Values.Where(value => value != null && !adjacentRooms.Contains(value)))
                {
                    nearSurroundingRooms.Add(nearRoom);
                }
            }

            foreach (var nearRoom in nearSurroundingRooms.ToArray())
            {
                foreach (var outerNearSurroundingRoom in nearRoom.GetAdjacentRooms().Values.Where(value => value != null &&
                    !adjacentRooms.Contains(value) && !nearSurroundingRooms.Contains(value)))
                {
                    nearSurroundingRooms.Add(outerNearSurroundingRoom);
                }
            }

            foreach (var nearRoom in nearSurroundingRooms)
            {
                foreach (var outerSurroundingRoom in nearRoom.GetAdjacentRooms().Values.Where(value => value != null &&
                    !adjacentRooms.Contains(value) && !nearSurroundingRooms.Contains(value)))
                {
                    outerSurroundingRooms.Add(outerSurroundingRoom);
                }
            }

            return new Region(adjacentRooms, nearSurroundingRooms, outerSurroundingRooms);
        }

        private void OptimizeRendering()
        {
            var surroundingRooms = this.GetSurroundingRooms();

            foreach (var adjacentRoom in surroundingRooms.AdjacentRooms)
            {
                adjacentRoom.IsInView = true;
                adjacentRoom.ShowInView();
            }

            foreach (var nearSurroundingRoom in surroundingRooms.NearSurroundingRooms)
            {
                nearSurroundingRoom.IsInView = true;
                nearSurroundingRoom.ShowInView();
            }

            foreach (var outerSurroundingRoom in surroundingRooms.OuterSurroundingRooms)
            {
                outerSurroundingRoom.IsInView = false;
                outerSurroundingRoom.HideFromView();
            }
        }

        /// <summary>
        /// Determines whether the specified room is adjacent to this room.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <returns>
        ///   <c>true</c> if the specified room is adjacent; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAdjacent(RoomBehaviour room)
        {
            return this.adjacentRooms.Values.Contains(room);
        }

        /// <summary>
        /// Turns the lights off.
        /// </summary>
        public void TurnOffLights()
        {
            foreach (var torch in this.Torches)
            {
                torch.Extinguish();
            }
        }

        /// <summary>
        /// Turns the lights on.
        /// </summary>
        public void TurnOnLights()
        {
            foreach (var torch in this.Torches)
            {
                torch.Ignite();
            }
        }

        /// <summary>
        /// Gets the the warning messages from any adjacent rooms, shuffled.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAdjacentRoomWarningMessages()
        {
            var messages = new List<string>();

            foreach (var room in GetAdjacentRooms().Values.Where(room => room != null))
            {
                messages.AddRange(room.GetMessages());
            }

            if (messages.Count > 1)
            {
                messages.Shuffle();
            }

            return messages;
        }

        /// <summary>
        /// Gets the messages from any occupants in the room.
        /// </summary>
        /// <returns></returns>
        public List<string> GetMessages()
        {
            var messages = new List<string>();

            foreach (var occupant in this.Occupants.Where(entity => !(entity is ClueBehaviour)))
            {
                var message = occupant.GetMessage();
                
                if (!string.IsNullOrWhiteSpace(message))
                {
                    Debug.Log($"{occupant} : ({this.RowPosition}, {this.ColumnPosition}) : {message}");
                    messages.Add(message);
                }
            }

            return messages;
        }

        /// <summary>
        /// Executed during the Start event of the GameObject life cycle.
        /// </summary>
        public void Start()
        {
            if (this.Occupants.Contains(PlayerBehaviour.Instance))
            {
                this.TurnOnLights();
            }
            else if (!this.Visited)
            {
                this.ShowDarkness();
                this.TurnOffLights();
            }
        }

        /// <summary>
        /// Called when a <see cref="Rigidbody2D" /> enters this instance's <see cref="Collider2D" />
        /// </summary>
        /// <param name="collision">The collision.</param>
        public void OnTriggerEnter2D(Collider2D collision)
        {
            var playerBehaviour = collision.attachedRigidbody.GetComponent<PlayerBehaviour>();

            if (playerBehaviour != null)
            {
                this.HideDarkness();

                foreach (var torch in this.Torches)
                {
                    torch.FlameBehaviour.SetImportant();
                }

                if (playerBehaviour.IsWalking)
                {
                    StartCoroutine(nameof(this.DelayThenTurnOnLights), 0.25f);
                }
            }
            else
            {
                var arrowBehaviour = collision.attachedRigidbody.GetComponent<ArrowBehaviour>();

                if (arrowBehaviour != null)
                {
                    if (this.GetMuglumpBehaviour() != null || 
                        ((arrowBehaviour is NetArrowBehaviour || arrowBehaviour is NetBehaviour) && this.GetPitBehaviour() != null))
                    {
                        this.HideDarkness();
                        StartCoroutine(nameof(this.DelayThenTurnOnLights), 0.25f);
                    }
                }
            }
        }

        public void HideDarkness()
        {
            this.Darkness.Hide();
            this.OptimizeRendering();
        }

        public void ShowDarkness()
        {
                this.Darkness.Show();
        }

        public IEnumerator DelayThenTurnOnLights(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            this.TurnOnLights();
        }

        /// <summary>
        /// Called when a <see cref="Rigidbody2D" /> exits this instance's <see cref="Collider2D" />
        /// </summary>
        /// <param name="collision">The collision.</param>
        public void OnTriggerExit2D(Collider2D collision)
        {
            var playerBehaviour = collision.attachedRigidbody.GetComponent<PlayerBehaviour>();

            if (playerBehaviour != null)
            {
                foreach (var torch in this.Torches)
                {
                    torch.FlameBehaviour.SetUnimportant();
                }

                if (!this.Visited && !this.Torches.AreLit())
                {
                    this.Darkness.Show();
                }
            }
        }

        private IEnumerator RenderDungeon()
        {
            foreach (var room in this.Dungeon.Dungeon)
            {
                if (!GameManager.Instance.Minimap.IsEnabled && (Mathf.Abs(CameraManager.Instance.MainCamera.transform.position.x - room.gameObject.transform.position.x) > 25.0f ||
                    Mathf.Abs(CameraManager.Instance.MainCamera.transform.position.y - room.gameObject.transform.position.y) > 25.0f))
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

        public void HideFromView()
        {
            this.TilemapRenderer.enabled = false;

            foreach (var renderer in this.gameObject.GetComponentsInChildren<Renderer>())
            {
                if (renderer.GetComponent<PlayerBehaviour>() == null)
                {
                    renderer.enabled = false;
                }
            }
        }

        public void ShowInView()
        {
            foreach (var renderer in this.gameObject.GetComponentsInChildren<Renderer>())
            {
                this.TilemapRenderer.enabled = true;

                if (renderer.GetComponent<BlinkBehaviour>() == null)
                {
                    renderer.enabled = true;
                }
            }
        }
    }
}
