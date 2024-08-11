#pragma warning disable CS0649
/**************************************************
 *  InventoryPanelBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI
{
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class InventoryPanelBehaviour : MonoBehaviour
    {
        private const int ItemTypeCount = 2;

        [SerializeField]
        private Button inventoryBackButton;

        [SerializeField]
        private Button inventoryForwardButton;

        [SerializeField]
        private List<Image> inventoryImages = new List<Image>();

        [SerializeField, ReadOnly]
        private ItemType currentItemType = ItemType.EauDuMuglump;

        private bool lockInput;

        public void Start()
        {
            inventoryForwardButton.onClick.AddListener(this.MoveItemInventorySelectorForward);
            inventoryBackButton.onClick.AddListener(this.MoveItemInventorySelectorBack);
        }

        public void FixedUpdate()
        {
            if (!lockInput  && !GameManager.Instance.PauseAction)
            {
                if (InputExtension.IsCycleItemsPressed())
                {
                    this.lockInput = true;
                    StartCoroutine(nameof(this.WaitForInventoryForwardReleaseThenMoveInventoryForward));
                }
            }
        }

        public IEnumerator WaitForInventoryBackReleaseThenMoveInventoryBack()
        {
            while (InputExtension.IsCyleArrowsPressed())
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            this.MoveItemInventorySelectorBack();
            this.lockInput = false;
        }

        public IEnumerator WaitForInventoryForwardReleaseThenMoveInventoryForward()
        {
            while (InputExtension.IsCycleItemsPressed())
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            this.MoveItemInventorySelectorForward();
            this.lockInput = false;
        }

        public void MoveItemInventorySelectorForward()
        {
            this.MoveItemInventorySelector(1);
        }

        public void MoveItemInventorySelectorBack()
        {
            this.MoveItemInventorySelector(-1);
        }

        public void MoveItemInventorySelector(int selector)
        {
            var currentItemIndex = (int)this.currentItemType;

            this.inventoryImages[currentItemIndex].gameObject.SetActive(false);

            currentItemIndex = (currentItemIndex == 0 && selector < 0) ? 
                this.inventoryImages.Count + selector : 
                (currentItemIndex + selector) % this.inventoryImages.Count;

            this.currentItemType = (ItemType)currentItemIndex;

            PlayerBehaviour.Instance.SelectedItemType = this.currentItemType;

            this.inventoryImages[(int)this.currentItemType].gameObject.SetActive(true);

            GameManager.Instance.SoundEffectManager.PlayAudioOnce(SoundClips.Click2);
        }
    }
}
