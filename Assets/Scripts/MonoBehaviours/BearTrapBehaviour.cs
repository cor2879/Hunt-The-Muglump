/**************************************************
 *  BearTrapBehaviour.cs
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
    using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    /// <summary>
    /// Defines the behaviours for Pit objects.
    /// </summary>
    /// <seealso cref="OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.EntityBehaviour" />
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class BearTrapBehaviour : EntityBehaviour, IPoolable
    {
        private static ObjectPool bearTrapObjectPool;
        private static readonly Vector3 IdlePointOffsetVector = new Vector3(0.0f, -0.4f, 0.0f);

        [SerializeField, ReadOnly]
        private ObjectPool objectPool;

        public GameObject GameObject { get => this.gameObject; }

        public ObjectPool ObjectPool { get => this.objectPool; }

        void IPoolable.OnDestinationReached()
        {
            if (this.objectPool != null)
            {
                this.ExitRoom();
                this.objectPool.Deactivate(this);
            }
            else
            {
                this.gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// Executed when another <see cref="RigidBody2D" /> makes contact with this instance's <see cref="Collider2D" />
        /// </summary>
        /// <param name="collider">The collider</param>
        public void OnTriggerEnter2D(Collider2D collider)
        {
            var muglump = collider.gameObject.GetComponent<MuglumpBehaviour>();

            if (muglump == null || muglump.IsBeingCarried)
            {
                return;
            }

            this.TrapEntity(muglump);
        }

        public void Reset()
        {
            this.transform.rotation = Quaternion.identity;
        }

        public void SetObjectPool(ObjectPool objectPool)
        {
            if (BearTrapBehaviour.bearTrapObjectPool != null &&
                (objectPool != BearTrapBehaviour.bearTrapObjectPool && objectPool.SessionId == BearTrapBehaviour.bearTrapObjectPool.SessionId))
            {
                throw new InvalidOperationException("Attempting to create an object pool for an object type when one already exists is an invalid action.");
            }

            if (BearTrapBehaviour.bearTrapObjectPool == null)
            {
                BearTrapBehaviour.bearTrapObjectPool = objectPool;
            }

            this.objectPool = BearTrapBehaviour.bearTrapObjectPool;
        }

        private void TrapEntity(EntityBehaviour entity)
        {
            var waitAction = new WaitAction(
                () => entity.CurrentRoom != this.CurrentRoom,
                () =>
                {
                    GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.BearTrapTrigger);
                    var playerDestination = PlayerBehaviour.Instance.GetComponent<MovementBehaviour>().CurrentDestination;

                    if (playerDestination != null)
                    {
                        playerDestination.AddSelfDestructClue(StringContent.BearTrapTrigger);
                    }

                    entity.IsTrapped = true;
                    Statistic.MuglumpsTrapped.Value++;
                    ((IPoolable)this).OnDestinationReached();
                });

            StartCoroutine(nameof(WaitForPredicateToBeFalseThenDoAction), waitAction);
        }

        public void Place(RoomBehaviour room, Vector3 position)
        {
            this.transform.position = position;
            this.MoveToRoom(room);
            Statistic.BearTrapsSet.Value++;
            this.Enable();
            GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.BearTrapSet);
        }

        public static BearTrapBehaviour Prefab => (BearTrapBehaviour)PoolablePrefabLibrary.Instance.BearTrapPrefab;

        IPoolable IPoolable.GetPrefab()
        {
            return BearTrapBehaviour.Prefab;
        }

        public static Path GetPathToClosestBearTrap(RoomBehaviour startingPoint)
        {
            var surroundingRooms = startingPoint.GetSurroundingRooms();
            var bearTrap = surroundingRooms.Where(room => room.GetBearTrapBehaviour() != null).Select(room => room.GetBearTrapBehaviour()).FirstOrDefault();

            if (bearTrap == null)
            {
                return null;
            }

            return MovementBehaviour.GetShortestWalkingPathAstar(startingPoint, bearTrap.CurrentRoom);
        }

        public override Vector3 GetIdleZeroPointOffsetVector()
        {
            return BearTrapBehaviour.IdlePointOffsetVector;
        }
    }
}