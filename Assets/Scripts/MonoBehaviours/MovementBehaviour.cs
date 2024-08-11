﻿/**************************************************
 *  MovementBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Defines the behaviours for game objects that have self-directed movement
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class MovementBehaviour : MonoBehaviour
    {
        private Dictionary<Direction.DirectionValue, Func<MovementBehaviour, Vector3>> DirectionVectors = new Dictionary<Direction.DirectionValue, Func<MovementBehaviour, Vector3>>()
        {
            { Direction.DirectionValue.North, (MovementBehaviour m) => new Vector3(0, Mathf.Min(m.movementSpeed, (m.currentDestination.Position.y + m.Entity.GetIdleZeroPointOffsetVector().y) - m.Entity.Position.y), 0) },
            { Direction.DirectionValue.South, (MovementBehaviour m) => new Vector3(0, Mathf.Max(m.movementSpeed* -1, (m.currentDestination.Position.y + m.Entity.GetIdleZeroPointOffsetVector().y) - m.Entity.Position.y), 0) },
            { Direction.DirectionValue.East, (MovementBehaviour m) => new Vector3(Mathf.Min(m.movementSpeed, (m.currentDestination.Position.x + m.Entity.GetIdleZeroPointOffsetVector().x) - m.Entity.Position.x), 0, 0) },
            { Direction.DirectionValue.West, (MovementBehaviour m) => new Vector3(Mathf.Max(m.movementSpeed* -1, (m.currentDestination.Position.x + m.Entity.GetIdleZeroPointOffsetVector().x) - m.Entity.Position.x), 0, 0) },
        };

        [SerializeField]
        private bool isTrapped;

        /// <summary>
        /// The origin
        /// </summary>
        protected RoomBehaviour origin;

        /// <summary>
        /// The current destination
        /// </summary>
        protected RoomBehaviour currentDestination;

        /// <summary>
        /// The direction
        /// </summary>
        protected Direction direction = Direction.Idle;

        /// <summary>
        /// The animator
        /// </summary>
        protected Animator animator;

        /// <summary>
        /// The path
        /// </summary>
        protected Path path;


        /// <summary>
        /// The use animator
        /// </summary>
        public bool useAnimator;

        /// <summary>
        /// The movement speed
        /// </summary>
        public float movementSpeed;

        /// <summary>
        /// The entity
        /// </summary>
        protected EntityBehaviour entity;

        /// <summary>
        /// Gets or sets the <see cref="Action" /> to execute once a destination is reached.
        /// </summary>
        /// <value>
        /// The on destination reached.
        /// </value>
        protected Action OnDestinationReached { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Action" /> to execute when this instance reaches the room
        /// just prior to the final destination.
        /// </summary>
        /// <value>
        /// The on before destination reached.
        /// </value>
        protected Action OnBeforeDestinationReached { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is moving.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is moving; otherwise, <c>false</c>.
        /// </value>
        public bool IsMoving { get; private set; }

        /// <summary>
        /// Gets or sets a Boolean value indicating whether or not this instance is trapped.
        /// </summary>
        public bool IsTrapped { get => this.isTrapped; set => this.isTrapped = value; }

        public bool UseAnimator { get => this.useAnimator; }

        /// <summary>
        /// Gets the animator.
        /// </summary>
        /// <value>
        /// The animator.
        /// </value>
        public Animator Animator
        {
            get
            {
                if (this.animator == null)
                {
                    this.animator = this.GetComponent<Animator>();
                }

                return this.animator;
            }
        }

        /// <summary>
        /// Gets the current destination.
        /// </summary>
        /// <value>
        /// The current destination.
        /// </value>
        public RoomBehaviour CurrentDestination
        {
            get => this.currentDestination;
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        public EntityBehaviour Entity
        {
            get
            {
                if (this.entity == null)
                {
                    this.entity = this.GetComponent<EntityBehaviour>();
                }

                return this.entity;
            }
        }

        public IList<string> MovementSounds { get => this.Entity.MovementSounds; }

        /// <summary>
        /// Executes during the Start event of the GameObject lifecycle
        /// </summary>
        public void Start()
        {
            this.IsMoving = false;
        }

        /// <summary>
        /// Updates this instance at a fixed interval which is determined by the Unity Engine at runtime.
        /// </summary>
        public void FixedUpdate()
        {
            if (GameManager.Instance.PauseAction)
            {
                return;
            }

            if (this.CanMove())
            {
                this.Move();
            }

            this.UpdateState();
        }

        public void Update()
        {
            if (this.IsTrapped)
            {
                this.IsMoving = false;
            }
        }

        /// <summary>
        /// Moves this instance to the specified room.
        /// </summary>
        /// <param name="room">The room.</param>
        public void MoveToRoom(RoomBehaviour room)
        {
            this.MoveToRoom(room, null, null);
        }

        public void MoveToRoom(KeyValuePair<Direction, RoomBehaviour> adjacentRoom, Action onDestinationReached)
        {
            this.OnDestinationReached = onDestinationReached;
            this.origin = this.Entity.CurrentRoom;
            this.currentDestination = adjacentRoom.Value;
            this.direction = adjacentRoom.Key;
            this.IsMoving = true;
        }

        /// <summary>
        /// Moves this instance to the specified room.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <param name="onDestinationReached">The on destination reached.</param>
        /// <param name="onBeforeDestinationReached">The on before destination reached.</param>
        public void MoveToRoom(RoomBehaviour room, Action onDestinationReached, Action onBeforeDestinationReached)
        {
            this.OnDestinationReached = onDestinationReached;
            this.OnBeforeDestinationReached = onBeforeDestinationReached;
            this.origin = this.Entity.CurrentRoom;
            this.path = GetShortestPath(this.origin, room);
            
            if (path.Any())
            {
                var pair = path.Pop();

                this.currentDestination = pair.Value;
                this.direction = pair.Key;

                this.IsMoving = true;

                if (this.UseAnimator)
                {
                    this.Animator.SetFloat(Constants.XDirection, this.direction.XValue);
                    this.Animator.SetFloat(Constants.YDirection, this.direction.YValue);
                }
            }
            else
            {
                this.currentDestination = null;
            }
        }

        /// <summary>
        /// Stops the motion.
        /// </summary>
        public void StopMotion()
        {
            this.IsMoving = false;
        }

        /// <summary>
        /// Updates the state.
        /// </summary>
        protected virtual void UpdateState()
        {
            if (this.useAnimator)
            {
                if ((path.IsNullOrEmpty() && 
                        (this.currentDestination == null || this.currentDestination == this.Entity.CurrentRoom) || 
                        !this.IsMoving))
                {
                    this.Animator.SetBool(Constants.IsWalking, false);
                }
                else
                {
                    this.Animator.SetBool(Constants.IsWalking, true);
                }

                this.Animator.SetFloat(Constants.XDirection, this.direction.XValue);
                this.Animator.SetFloat(Constants.YDirection, this.direction.YValue);
            }
        }

        public bool CanMove()
        {
            return !this.IsTrapped &&
                this.currentDestination != null && 
                this.entity.Position != (this.currentDestination.Position + this.Entity.GetIdleZeroPointOffsetVector()) && 
                this.IsMoving;
        }

        /// <summary>
        /// Moves this instance.
        /// </summary>
        protected virtual void Move()
        {
            var movementVector = this.GetMovementVector();

            var moveTo = movementVector == Vector3.zero ?
                this.currentDestination.Position + this.Entity.GetIdleZeroPointOffsetVector() :
                entity.Position + movementVector;

            if (this.UseAnimator)
            {
                this.Animator.SetFloat(Constants.XDirection, this.direction.XValue);
                this.Animator.SetFloat(Constants.YDirection, this.direction.YValue);
            }

            this.entity.transform.SetPositionAndRotation(moveTo, Quaternion.identity);

            var distanceVector = (this.CurrentDestination.Position + this.Entity.GetIdleZeroPointOffsetVector()) - this.Entity.Position;

            if (Math.Abs(distanceVector.x) <= Constants.DistanceTolerance && Math.Abs(distanceVector.y) <= Constants.DistanceTolerance)
            {
                this.entity.MoveToRoom(currentDestination);

                if (this.path != null && this.path.Any())
                {
                    var pair = this.path.Pop();

                    this.currentDestination = pair.Value;
                    this.direction = pair.Key;

                    if (this.path.Length == 0)
                    {
                        this.OnBeforeDestinationReached?.Invoke();
                    }
                }
                else
                {
                    this.currentDestination = null;
                    this.IsMoving = false;

                    this.OnDestinationReached?.Invoke();
                }
            }
        }

        /// <summary>
        /// Gets the movement vector.
        /// </summary>
        /// <returns></returns>
        protected virtual Vector3 GetMovementVector()
        {
            return DirectionVectors.TryGetValue(this.direction.Value, out var movementVectorFunc) ? movementVectorFunc(this) : Vector3.zero;
        }

        /// <summary>
        /// Gets the movement behaviour.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static MovementBehaviour GetMovementBehaviour(EntityBehaviour entity)
        {
            var movementBehaviour = entity.GetComponent<MovementBehaviour>();
            movementBehaviour.entity = entity;

            return movementBehaviour;
        }

        /// <summary>
        /// Gets the shortest path between any two rooms in the dungeon.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        public static Path GetShortestWalkingPath(RoomBehaviour origin, RoomBehaviour destination)
        {
            var edges = new Dictionary<RoomBehaviour, RoomBehaviour>();
            var vertices = new Queue<RoomBehaviour>();
            vertices.Enqueue(origin);

            while (vertices.Count > 0)
            {
                var vertex = vertices.Dequeue();

                foreach (var pair in vertex.GetAdjacentRooms().Where(kvp => kvp.Value != null && kvp.Value.GetPitBehaviour() == null && kvp.Value.GetMuglumpBehaviour() == null))
                {
                    if (edges.ContainsKey(pair.Value))
                    {
                        continue;
                    }

                    edges[pair.Value] = vertex;
                    vertices.Enqueue(pair.Value);
                }
            }

            var path = new Path();

            var current = destination;

            while (!(current == origin) && edges.ContainsKey(current))
            {
                path.Push(edges[current].GetAdjacentRooms().Where(kvp => kvp.Value == current).First());
                current = edges[current];
            }

            return path;
        }

        public static Path GetShortestWalkingPathAstar(RoomBehaviour origin, RoomBehaviour destination)
        {
            var openSet = new PriorityQueue<RoomBehaviour>();
            var cameFrom = new Dictionary<RoomBehaviour, RoomBehaviour>();
            var gScore = new Dictionary<RoomBehaviour, float>();
            var fScore = new Dictionary<RoomBehaviour, float>();

            if (origin.IsAdjacent(destination))
            {
                var shortPath = new Path();
                shortPath.Push(origin.GetAdjacentRooms().Where(kvp => kvp.Value == destination).First());

                return shortPath;
            }

            gScore[origin] = 0;
            fScore[origin] = HeuristicCostEstimate(origin, destination);
            openSet.Enqueue(origin, fScore[origin]);

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();

                if (current == destination)
                {
                    break;
                }

                foreach (var pair in current.GetAdjacentRooms().Where(kvp => kvp.Value != null && kvp.Value.GetPitBehaviour() == null && kvp.Value.GetMuglumpBehaviour() == null))
                {
                    var neighbor = pair.Value;
                    var tentativeGScore = gScore[current] + DistanceBetween(current, neighbor);

                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, destination);

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Enqueue(neighbor, fScore[neighbor]);
                        }
                    }
                }
            }

            var path = new Path();
            var currentRoom = destination;

            while (currentRoom != origin && cameFrom.ContainsKey(currentRoom) && cameFrom[currentRoom] != origin)
            {
                path.Push(currentRoom.GetAdjacentRooms().Where(kvp => kvp.Value == cameFrom[currentRoom]).First());
                currentRoom = cameFrom[currentRoom];
            }

            return path;
        }

        private static float DistanceBetween(RoomBehaviour room1, RoomBehaviour room2)
        {
            // Return the Euclidean distance between the two rooms (or some other distance metric)
            return Mathf.Sqrt(Mathf.Pow(room1.transform.position.x - room2.transform.position.x, 2) + Mathf.Pow(room1.transform.position.y - room2.transform.position.y, 2));
        }

        private static float HeuristicCostEstimate(RoomBehaviour current, RoomBehaviour destination)
        {
            // Return the heuristic cost estimate between the current room and the destination room (e.g., the Euclidean distance)
            return DistanceBetween(current, destination);
        }

        /// <summary>
        /// Gets the shortest path between any two rooms in the dungeon.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        public static Path  GetShortestPath(RoomBehaviour origin, RoomBehaviour destination)
        {
            var edges = new Dictionary<RoomBehaviour, RoomBehaviour>();
            var vertices = new Queue<RoomBehaviour>();
            vertices.Enqueue(origin);

            while (vertices.Count > 0)
            {
                var vertex = vertices.Dequeue();

                foreach (var pair in vertex.GetAdjacentRooms().Where(kvp => kvp.Value != null))
                {
                    if (edges.ContainsKey(pair.Value))
                    {
                        continue;
                    }

                    edges[pair.Value] = vertex;
                    vertices.Enqueue(pair.Value);
                }
            }

            var path = new Path();

            var current = destination;

            while (!(current == origin) && !(current == null))
            {
                path.Push(edges[current].GetAdjacentRooms().Where(kvp => kvp.Value == current).First());
                current = edges[current];
            }

            return path;
        }

        public static Path GetShortestPathAstar(RoomBehaviour origin, RoomBehaviour destination)
        {
            var openList = new List<RoomBehaviour> { origin };
            var closedList = new HashSet<RoomBehaviour>();
            var gScores = new Dictionary<RoomBehaviour, float> { [origin] = 0 };
            var fScores = new Dictionary<RoomBehaviour, float> { [origin] = HeuristicCostEstimate(origin, destination) };

            var cameFrom = new Dictionary<RoomBehaviour, RoomBehaviour>();

            while (openList.Count > 0)
            {
                var currentRoom = openList.OrderBy(n => fScores[n]).First();

                if (currentRoom == destination)
                {
                    var path = new Path();
                    path.Push(currentRoom.GetAdjacentRooms().Where(kvp => kvp.Value == cameFrom[currentRoom]).First());

                    while (cameFrom.ContainsKey(currentRoom) && cameFrom[currentRoom] != origin)
                    {
                        currentRoom = cameFrom[currentRoom];
                        path.Push(currentRoom.GetAdjacentRooms().Where(kvp => kvp.Value == cameFrom[currentRoom]).First());
                    }

                    return path;
                }

                openList.Remove(currentRoom);
                closedList.Add(currentRoom);

                foreach (var pair in currentRoom.GetAdjacentRooms().Where(kvp => kvp.Value != null))
                {
                    var neighbor = pair.Value;
                    if (closedList.Contains(neighbor))
                    {
                        continue;
                    }

                    var tentativeGScore = gScores[currentRoom] + 1;
                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                    else if (tentativeGScore >= gScores[neighbor])
                    {
                        continue;
                    }

                    cameFrom[neighbor] = currentRoom;
                    gScores[neighbor] = tentativeGScore;
                    fScores[neighbor] = tentativeGScore + HeuristicCostEstimate(neighbor, destination);
                }
            }

            return null;
        }
    }
}
