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
    using UnityEngine.InputSystem;
    using UnityEngine.UI;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.GameplayManagement;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class ArrowPanelBehaviour : PanelBehaviour
    {
        private const int ArrowTypeCount = 3;

        [SerializeField]
        private Button inventoryBackButton;

        [SerializeField]
        private Button inventoryForwardButton;

        [SerializeField]
        private List<Image> arrowImages = new List<Image>();

        [SerializeField, ReadOnly]
        private ArrowType currentArrowType = ArrowType.Arrow;

        private bool lockInput;

        public ArrowType CurrentArrowType { get => this.currentArrowType; private set => this.currentArrowType = value; }

        public void Start()
        {
            inventoryForwardButton.onClick.AddListener(this.MoveArrowInventorySelectorForward);
            inventoryBackButton.onClick.AddListener(this.MoveArrowInventorySelectorBack);
        }

        public void FixedUpdate()
        {
            if (!lockInput && !GameManager.Instance.PauseAction)
            {
                if (InputExtension.IsCyleArrowsPressed())
                {
                    this.lockInput = true;

                    StartCoroutine(nameof(this.WaitForInventoryBackReleaseThenMoveInventoryForward));
                }
            }
        }

        public IEnumerator WaitForInventoryBackReleaseThenMoveInventoryForward()
        {
            while (InputExtension.IsCyleArrowsPressed())
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            this.MoveArrowInventorySelectorForward();
            this.lockInput = false;
        }

        public IEnumerator WaitForInventoryForwardReleaseThenMoveInventoryForward()
        {
            while (InputExtension.IsCycleItemsPressed())
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            this.MoveArrowInventorySelectorForward();
            this.lockInput = false;
        }

        public void MoveArrowInventorySelectorForward()
        {
            this.MoveArrowInventorySelector(1);
        }

        public void MoveArrowInventorySelectorBack()
        {
            this.MoveArrowInventorySelector(-1);
        }

        public void MoveArrowInventorySelector(int selector)
        {
            var currentItemIndex = (int)this.CurrentArrowType;

            this.arrowImages[currentItemIndex].gameObject.SetActive(false);

            currentItemIndex = (currentItemIndex == 0 && selector < 0) ?
                this.arrowImages.Count + selector :
                (currentItemIndex + selector) % this.arrowImages.Count;

            this.currentArrowType = (ArrowType)currentItemIndex;

            PlayerBehaviour.Instance.SelectArrows(this.CurrentArrowType);

            this.arrowImages[(int)this.CurrentArrowType].gameObject.SetActive(true);
        }

        private void OnCycleArrowsActionUp()
        {
            this.MoveArrowInventorySelectorForward();
        }
    }
}

