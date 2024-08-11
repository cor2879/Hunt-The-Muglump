/**************************************************
 *  HunterBehaviour.cs
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

    [RequireComponent(typeof(MuglumpBehaviour))]
    [RequireComponent(typeof(MovementBehaviour))]
    [RequireComponent(typeof(Collider2D))]
    public class HunterBehaviour : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private int playerMovesBetweenAdvance = 3;

        [SerializeField, ReadOnly]
        private int lastPlayerMove = 1;

        [SerializeField, ReadOnly]
        private MuglumpBehaviour muglumpBehaviour;

        [SerializeField, ReadOnly]
        private MovementBehaviour movementBehaviour;

        [SerializeField, ReadOnly]
        private bool isMoving;

        private static HashSet<RoomBehaviour> ClaimedDestinations { get; } = new HashSet<RoomBehaviour>();

        public RoomBehaviour CurrentRoom { get => this.MuglumpBehaviour.CurrentRoom; }

        public MuglumpBehaviour MuglumpBehaviour
        {
            get
            {
                if (this.muglumpBehaviour == null)
                {
                    this.muglumpBehaviour = this.GetComponent<MuglumpBehaviour>();
                }

                return this.muglumpBehaviour;
            }
        }

        public MovementBehaviour MovementBehaviour
        {
            get
            {
                if (this.movementBehaviour == null)
                {
                    this.movementBehaviour = this.GetComponent<MovementBehaviour>();
                }

                return this.movementBehaviour;
            }
        }

        public bool IsMoving
        {
            get => this.isMoving;
            private set => this.isMoving = value;
        }

        public int PlayerMovesBetweenAdvance
        {
            get => this.playerMovesBetweenAdvance;
        }

        public int LastPlayerMove
        {
            get => this.lastPlayerMove;
            private set => this.lastPlayerMove = value;
        }

        public void FixedUpdate()
        {
            if (PlayerBehaviour.Instance.MoveCount != this.LastPlayerMove && this.MuglumpBehaviour.Net == null)
            {
                this.LastPlayerMove = PlayerBehaviour.Instance.MoveCount;

                if (this.LastPlayerMove % this.PlayerMovesBetweenAdvance == 0)
                {
                    this.HuntThePlayer();
                }
            }
        }

        /// <summary>
        /// Hunts the player.
        /// </summary>
        public void HuntThePlayer()
        {
            Path path = null;

            if (this.CanSmellPlayer())
            {
                var destinationRoom = PlayerBehaviour.Instance.CurrentRoom;
                path = MovementBehaviour.GetShortestWalkingPathAstar(this.MuglumpBehaviour.CurrentRoom, destinationRoom);
            }
            else if (Settings.Difficulty.TrapsAttractHunters)
            {
                path = this.GetPathToClosestTarget();
            }

            if (!path.IsNullOrEmpty())
            {
                var step = path.Pop();

                if (!ClaimedDestinations.Contains(step.Value))
                {
                    ClaimedDestinations.Add(step.Value);
                    this.IsMoving = true;
                    this.MovementBehaviour.MoveToRoom(step, () =>
                    {
                        this.isMoving = false;

                        ClaimedDestinations.Remove(step.Value);
                    });
                }
            }
            else
            {
                Debug.Log("No path to target");
            }
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            var playerBehaviour = collision.attachedRigidbody?.GetComponent<PlayerBehaviour>();

            if (playerBehaviour != null &&!playerBehaviour.IsBeingCarried && this.IsMoving)
            {
                StartCoroutine(nameof(this.WaitForPlayerPositionThenKill), playerBehaviour);
            }
        }

        private IEnumerator WaitForPlayerPositionThenKill(PlayerBehaviour playerBehaviour)
        {
            while (Mathf.Abs(playerBehaviour.Position.x - this.MuglumpBehaviour.Position.x) > 0.5f || Mathf.Abs(playerBehaviour.Position.y - this.MuglumpBehaviour.Position.y) > 0.5f)
            {
                yield return new WaitForFixedUpdate();
            }

            playerBehaviour.StopWalking();
            this.MovementBehaviour.StopMotion();
            this.MuglumpBehaviour.HandleEncounter(playerBehaviour);
        }

        public bool CanSmellPlayer()
        {
            return this.MuglumpBehaviour.CanSmellPlayer();
        }

        private Path GetPathToClosestTarget()
        {
            return BearTrapBehaviour.GetPathToClosestBearTrap(this.CurrentRoom);
        }
    }
}
