/**************************************************
 *  InventoryBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours
{
    using System.Collections.Generic;
    using UnityEngine;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    using Menus = OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.GameplayManagement.GameplayMenuManagerBehaviour;

    public class InventoryBehaviour : MonoBehaviour
    {
        private const int poolSize = 5;

        private Dictionary<ArrowType, ObjectPool> arrowObjectPools = new Dictionary<ArrowType, ObjectPool>()
        {
            { ArrowType.Arrow, null },
            { ArrowType.FlashArrow, null },
            { ArrowType.NetArrow, null }
        };

        private Dictionary<ItemType, ObjectPool> itemObjectPools = new Dictionary<ItemType, ObjectPool>()
        {
            { ItemType.BearTrap, null }
        };

        private Dictionary<ArrowType, int> arrowCounts = new Dictionary<ArrowType, int>()
        {
            { ArrowType.Arrow, 0 },
            { ArrowType.FlashArrow, 0 },
            { ArrowType.NetArrow, 0 }
        };

        private Dictionary<ItemType, int> itemCounts = new Dictionary<ItemType, int>()
        {
            { ItemType.EauDuMuglump, 0 },
            { ItemType.BearTrap, 0 }
        };

        /// <summary>
        /// Gets the arrow pool.
        /// </summary>
        /// <value>
        /// The arrow pool.
        /// </value>
        public ObjectPool ArrowPool
        {
            get
            {
                if (this.arrowObjectPools[ArrowType.Arrow] == null)
                {
                    this.arrowObjectPools[ArrowType.Arrow] = new ObjectPool(GameManager.Instance.SessionId);

                    for (var i = 0; i < poolSize; i++)
                    {
                        var arrow = Instantiate(ArrowBehaviour.Prefab);
                        this.arrowObjectPools[ArrowType.Arrow].Add(arrow.GetComponent<ArrowBehaviour>());
                    }
                }

                return this.arrowObjectPools[ArrowType.Arrow];
            }
        }

        /// <summary>
        /// Gets the flash arrow pool.
        /// </summary>
        /// <value>
        /// The flash arrow pool.
        /// </value>
        public ObjectPool FlashArrowPool
        {
            get
            {
                if (this.arrowObjectPools[ArrowType.FlashArrow] == null)
                {
                    this.arrowObjectPools[ArrowType.FlashArrow] = new ObjectPool(GameManager.Instance.SessionId);

                    for (var i = 0; i < poolSize; i++)
                    {
                        var flashArrow = Instantiate(GameManager.Instance.flashArrowPrefab);
                        this.arrowObjectPools[ArrowType.FlashArrow].Add(flashArrow.GetComponent<FlashArrowBehaviour>());
                    }
                }

                return this.arrowObjectPools[ArrowType.FlashArrow];
            }
        }

        /// <summary>
        /// Gets the net arrow pool.
        /// </summary>
        /// <value>
        /// The net arrow pool.
        /// </value>
        public ObjectPool NetArrowPool
        {
            get
            {
                if (this.arrowObjectPools[ArrowType.NetArrow] == null)
                {
                    this.arrowObjectPools[ArrowType.NetArrow] = new ObjectPool(GameManager.Instance.SessionId);

                    for (var i = 0; i < poolSize; i++)
                    {
                        var netArrow = Instantiate(GameManager.Instance.netArrowPrefab);
                        this.arrowObjectPools[ArrowType.NetArrow].Add(netArrow.GetComponent<NetArrowBehaviour>());
                    }
                }

                return this.arrowObjectPools[ArrowType.NetArrow];
            }
        }

        /// <summary>
        /// Gets the bear trap pool.
        /// </summary>
        /// <value>
        /// The bear trap pool.
        /// </value>
        public ObjectPool BearTrapPool
        {
            get
            {
                if (this.itemObjectPools[ItemType.BearTrap] == null)
                {
                    this.itemObjectPools[ItemType.BearTrap] = new ObjectPool(GameManager.Instance.SessionId);

                    for (var i = 0; i < poolSize; i++)
                    {
                        var bearTrap = Instantiate(BearTrapBehaviour.Prefab);
                        this.itemObjectPools[ItemType.BearTrap].Add(bearTrap.GetComponent<BearTrapBehaviour>());
                    }
                }

                return this.itemObjectPools[ItemType.BearTrap];
            }
        }

        /// <summary>
        /// Gets the arrow count.
        /// </summary>
        /// <value>
        /// The arrow count.
        /// </value>
        public int ArrowCount
        {
            get => this.arrowCounts[ArrowType.Arrow];
            private set
            {
                this.arrowCounts[ArrowType.Arrow] = value;
                Menus.SetArrowCountText(this.arrowCounts[ArrowType.Arrow]);
            }
        }

        /// <summary>
        /// Gets the flash arrow count.
        /// </summary>
        /// <value>
        /// The flash arrow count.
        /// </value>
        public int FlashArrowCount
        {
            get => this.arrowCounts[ArrowType.FlashArrow];
            private set
            {
                this.arrowCounts[ArrowType.FlashArrow] = value;
                Menus.SetFlashArrowCountText(this.arrowCounts[ArrowType.FlashArrow]);
            }
        }

        /// <summary>
        /// Gets the net arrow count.
        /// </summary>
        /// <value>
        /// The net arrow count.
        /// </value>
        public int NetArrowCount
        {
            get => this.arrowCounts[ArrowType.NetArrow];
            private set
            {
                this.arrowCounts[ArrowType.NetArrow] = value;
                Menus.SetNetArrowCountText(this.arrowCounts[ArrowType.NetArrow]);
            }
        }

        /// <summary>
        /// Gets the cover scent count.
        /// </summary>
        /// <value>
        /// The cover scent count.
        /// </value>
        public int CoverScentCount
        {
            get => this.itemCounts[ItemType.EauDuMuglump];
            private set
            {
                this.itemCounts[ItemType.EauDuMuglump] = value;
                Menus.SetEauDuMuglumpCountText(this.itemCounts[ItemType.EauDuMuglump]);
            }
        }

        public int BearTrapCount
        {
            get => this.itemCounts[ItemType.BearTrap];
            private set
            {
                this.itemCounts[ItemType.BearTrap] = value;
                Menus.SetBearTrapCountText(this.itemCounts[ItemType.BearTrap]);
            }
        }

        public void Initialize()
        {
            PlayerBehaviour.Instance.Inventory.AddArrows(0);
            PlayerBehaviour.Instance.Inventory.AddFlashArrows(0);
            PlayerBehaviour.Instance.Inventory.AddNetArrows(0);
            PlayerBehaviour.Instance.Inventory.AddCoverScent(0);
            PlayerBehaviour.Instance.Inventory.AddBearTraps(0);
        }

        public void AddArrows(int count)
        {
            this.ArrowCount += count;
        }

        public void AddFlashArrows(int count)
        {
            this.FlashArrowCount += count;
        }

        public void AddNetArrows(int count)
        {
            this.NetArrowCount += count;
        }

        public void AddCoverScent(int count)
        {
            this.CoverScentCount += count;
        }

        public void AddBearTraps(int count)
        {
            this.BearTrapCount += count;
        }

        public void AddArrow(ArrowType arrowType, int count)
        {
            this.arrowCounts[arrowType] += count;

            switch (arrowType)
            {
                case ArrowType.Arrow:
                    Menus.SetArrowCountText(this.ArrowCount);
                    break;
                case ArrowType.FlashArrow:
                    Menus.SetFlashArrowCountText(this.FlashArrowCount);
                    break;
                case ArrowType.NetArrow:
                    Menus.SetNetArrowCountText(this.NetArrowCount);
                    break;
            }
        }

        public void AddItem(ItemType itemType, int count)
        {
            this.itemCounts[itemType] += count;

            switch (itemType)
            {
                case ItemType.EauDuMuglump:
                    Menus.SetEauDuMuglumpCountText(this.CoverScentCount);
                    break;
                case ItemType.BearTrap:
                    Menus.SetBearTrapCountText(this.BearTrapCount);
                    break;
            }
        }

        public ArrowBehaviour GetArrow()
        {
            if (this.ArrowCount > 0)
            {
                --this.ArrowCount;
                return this.ArrowPool.ActivateNext() as ArrowBehaviour;
            }

            return null;
        }

        public FlashArrowBehaviour GetFlashArrow()
        {
            if (this.FlashArrowCount > 0)
            {
                --this.FlashArrowCount;
                return this.FlashArrowPool.ActivateNext() as FlashArrowBehaviour;
            }

            return null;
        }

        /// <summary>
        /// Gets the net arrow.
        /// </summary>
        /// <returns></returns>
        public NetArrowBehaviour GetNetArrow()
        {
            if (this.NetArrowCount > 0)
            {
                --this.NetArrowCount;
                return this.NetArrowPool.ActivateNext() as NetArrowBehaviour;
            }

            return null;
        }

        /// <summary>
        /// Gets the cover scent.
        /// </summary>
        /// <returns></returns>
        public bool GetCoverScent()
        {
            if (this.CoverScentCount > 0)
            {
                --this.CoverScentCount;
                return true;
            }

            return false;
        }

        public BearTrapBehaviour GetBearTrap()
        {
            if (this.BearTrapCount > 0)
            {
                --this.BearTrapCount;
                return this.BearTrapPool.AddOrActivateNext(
                (prefab) =>
                {
                    return Instantiate(prefab.gameObject);
                },
                PoolablePrefabLibrary.Instance.BearTrapPrefab) as BearTrapBehaviour;
            }

            return null;
        }

        public ArrowBehaviour GetArrow(ArrowType arrowType)
        {
            switch (arrowType)
            {
                case ArrowType.Arrow:
                    return this.GetArrow();
                case ArrowType.FlashArrow:
                    return this.GetFlashArrow();
                case ArrowType.NetArrow:
                    return this.GetNetArrow();
            }

            return null;
        }

        public int GetArrowCount(ArrowType arrowType)
        {
            return this.arrowCounts[arrowType];
        }

        public int GetItemCount(ItemType itemType)
        {
            return this.itemCounts[itemType];
        }
    }
}
