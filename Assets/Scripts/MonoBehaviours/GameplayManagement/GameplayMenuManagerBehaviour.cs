#pragma warning disable CS0649
/**************************************************
 *  GameplayMenuManagerBehavour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.GameplayManagement
{
    using System;
    using System.Collections;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;


    using BeautifulInterface = Interface.Elements.Scripts;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.Interfaces;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.GameplayManagement.ChildReferenceManagers;
    using OldSchoolGames.HuntTheMuglump.Scripts.Platform;
    using OldSchoolGames.HuntTheMuglump.Scripts.Rules;
    using OldSchoolGames.HuntTheMuglump.Scripts.UI;
    using OldSchoolGames.HuntTheMuglump.Scripts.UI.GameplayPrompts;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    [RequireComponent(typeof(ArrowCountTextManagerBehaviour))]
    [RequireComponent(typeof(ItemCountTextManagerBehaviour))]
    public class GameplayMenuManagerBehaviour : MonoBehaviour
    {
        private static GameplayMenuManagerBehaviour instance;
        private static bool[] validDirections = new bool[] { false, false, false, false };

        [SerializeField, ReadOnly]
        private GameplayMenuStateBase menuState;

        [SerializeField, ReadOnly]
        private GameplayMenuStateBase previousState;

        [SerializeField, ReadOnly]
        private ControlStateBase currentControlState;

        [SerializeField, ReadOnly]
        private ControlStateBase previousControlState;

        [SerializeField, ReadOnly]
        private bool isInputLocked;

        #region private reference fields

        [SerializeField]
        private MainGameplayMenuPanel keyboardMainGameplayMenu;

        [SerializeField]
        private DirectionalActionSubMenuBehaviour keyboardDirectionalActionSubMenu;

        [SerializeField]
        private LookingAtRoomMenuBehaviour keyboardLookingAtRoomMenu;

        [SerializeField]
        private MainGameplayMenuPanel gamepadMainGameplayMenu;

        [SerializeField]
        private DirectionalActionSubMenuBehaviour gamepadDirectionalActionSubMenu;

        [SerializeField]
        private LookingAtRoomMenuBehaviour gamepadLookingAtRoomMenu;

        [SerializeField]
        private MainGameplayMenuPanel mobileTouchMainGameplayMenu;

        [SerializeField]
        private DirectionalActionSubMenuBehaviour mobileTouchDirectionalActionSubMenu;

        [SerializeField]
        private LookingAtRoomMenuBehaviour mobileTouchLookingAtRoomMenu;

        [SerializeField]
        private ArrowPanelBehaviour arrowInventoryPanel;

        [SerializeField]
        private InventoryPanelBehaviour itemInventoryPanel;

        [SerializeField]
        private MainTextPanelBehaviour mainTextPanel;

        #endregion

        #region public reference accessors

        public MainGameplayMenuPanel KeyboardMainGameplayMenu
        {
            get
            {
                this.ValidateUnityEditorParameter(this.keyboardMainGameplayMenu, nameof(this.keyboardMainGameplayMenu));

                return this.keyboardMainGameplayMenu;
            }
        }

        public DirectionalActionSubMenuBehaviour KeyboardDirectionalActionMenu
        {
            get
            {
                this.ValidateUnityEditorParameter(this.keyboardDirectionalActionSubMenu, nameof(this.keyboardDirectionalActionSubMenu));

                return this.keyboardDirectionalActionSubMenu;
            }
        }

        public LookingAtRoomMenuBehaviour KeyboardLookingAtRoomMenu
        {
            get
            {
                this.ValidateUnityEditorParameter(this.keyboardLookingAtRoomMenu, nameof(this.keyboardLookingAtRoomMenu));

                return this.keyboardLookingAtRoomMenu;
            }
        }

        public MainGameplayMenuPanel GamepadMainGameplayMenu
        {
            get
            {
                this.ValidateUnityEditorParameter(this.gamepadMainGameplayMenu, nameof(this.gamepadMainGameplayMenu));

                return this.gamepadMainGameplayMenu;
            }
        }

        public DirectionalActionSubMenuBehaviour GamepadDirectionalActionMenu
        { 
            get
            {
                this.ValidateUnityEditorParameter(this.gamepadDirectionalActionSubMenu, nameof(this.gamepadDirectionalActionSubMenu));

                return this.gamepadDirectionalActionSubMenu;
            }
        }

        public LookingAtRoomMenuBehaviour GamepadLookingAtRoomMenu
        {
            get
            {
                this.ValidateUnityEditorParameter(this.gamepadLookingAtRoomMenu, nameof(this.gamepadLookingAtRoomMenu));

                return this.gamepadLookingAtRoomMenu;
            }
        }

        public MainGameplayMenuPanel MobileTouchMainGameplayMenu
        {
            get
            {
                this.ValidateUnityEditorParameter(this.mobileTouchMainGameplayMenu, nameof(this.mobileTouchMainGameplayMenu));

                return this.mobileTouchMainGameplayMenu;
            }
        }

        public DirectionalActionSubMenuBehaviour MobileTouchDirectionalActionMenu
        {
            get
            {
                this.ValidateUnityEditorParameter(this.mobileTouchDirectionalActionSubMenu, nameof(this.mobileTouchDirectionalActionSubMenu));

                return this.mobileTouchDirectionalActionSubMenu;
            }
        }

        public LookingAtRoomMenuBehaviour MobileTouchLookingAtRoomMenu
        {
            get
            {
                this.ValidateUnityEditorParameter(this.mobileTouchLookingAtRoomMenu, nameof(this.mobileTouchLookingAtRoomMenu));

                return this.mobileTouchLookingAtRoomMenu;
            }
        }

        public ArrowPanelBehaviour ArrowInventoryPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.arrowInventoryPanel, nameof(this.arrowInventoryPanel));

                return this.arrowInventoryPanel;
            }
        }

        public InventoryPanelBehaviour ItemInventoryPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.itemInventoryPanel, nameof(this.itemInventoryPanel));

                return this.itemInventoryPanel;
            }
        }

        public MainTextPanelBehaviour MainTextPanel
        {
            get
            {
                this.ValidateUnityEditorParameter(this.mainTextPanel, nameof(this.mainTextPanel));

                return this.mainTextPanel;
            }
        }

        #endregion

        #region public Component accessors

        public ArrowCountTextManagerBehaviour ArrowCountTexts { get => ArrowCountTextManagerBehaviour.Instance; }

        public ItemCountTextManagerBehaviour ItemCountTexts { get => ItemCountTextManagerBehaviour.Instance; }

        #endregion

        private static PlayerBehaviour Player { get => PlayerBehaviour.Instance; }

        public GameplayMenuStateBase MenuState { get => this.menuState; set => this.menuState = value; }

        public GameplayMenuStateBase PreviousState { get => this.previousState; set => this.previousState = value; }

        public ControlStateBase CurrentControlState { get => this.currentControlState; set => this.currentControlState = value; }

        public ControlStateBase PreviousControlState { get => this.previousControlState; set => this.previousControlState = value; }

        public static GameplayMenuManagerBehaviour Instance { get => instance; }

        private static GameManager GameManager { get => GameManager.Instance; }

        /// <summary>
        /// Sets the arrow count text.
        /// </summary>
        /// <param name="count">The count.</param>
        public static void SetArrowCountText(int count)
        {
            foreach (var t in Instance.ArrowCountTexts.ArrowCountText)
            {
                t.text = count.ToString();
            }
        }

        public static void SetFlashArrowCountText(int count)
        {
            foreach (var t in Instance.ArrowCountTexts.FlashArrowCountText)
            {
                t.text = count.ToString();
            }
        }

        public static void SetNetArrowCountText(int count)
        {
            foreach (var t in Instance.ArrowCountTexts.NetArrowCountText)
            {
                t.text = count.ToString();
            }
        }

        public static void SetEauDuMuglumpCountText(int count)
        {
            foreach (var t in Instance.ItemCountTexts.EauDuMuglumpCountText)
            {
                t.text = count.ToString();
            };
        }

        public static void SetBearTrapCountText(int count)
        {
            foreach (var t in Instance.ItemCountTexts.BearTrapCountText)
            {
                t.text = count.ToString();
            };
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void Update()
        {
            SetMenuValidDirections(GetValidDirections());
            this.MenuState.Update();
            this.CurrentControlState.Update();
            this.isInputLocked = this.MenuState.LockInput;
        }

        private void Start()
        {
            this.MenuState = GameplayMenuStateBase.Instance;
            this.CurrentControlState = ControlStateBase.Instance;
            this.MenuState.Start();
        }

        public static void SetMainGameplayMenuActive(bool activeState)
        {
            Instance.CurrentControlState.MainGameplayMenu.SetActive(activeState);
        }

        public static void SetDirectionalActionSubMenuActiveState(bool activeState)
        {
            Instance.CurrentControlState.DirectionalActionMenu.SetActive(activeState);
        }

        public static void SetLookingAtRoomMenuActiveState(bool activeState)
        {
            Instance.CurrentControlState.LookingAtRoomMenu.SetActive(activeState);
        }

        public static void OpenMinimap()
        {
            GameManager.Minimap.Enable();
        }

        public static void SetMainTextPanelText(string text)
        {
            Instance.MainTextPanel.Text = text;
            Instance.MainTextPanel.StartAnimation();
        }

        public static void AppendLineMainWindowText(string text)
        {
            if (!string.IsNullOrEmpty(Instance.MainTextPanel.Text))
            {
                Instance.MainTextPanel.Text += $"{Environment.NewLine}{Environment.NewLine}{text}";
            }
            else
            {
                SetMainTextPanelText(text);
            }
        }

        /// <summary>
        /// Clears the main window text.
        /// </summary>
        public static void ClearMainWindowText()
        {
            Instance.MainTextPanel.Text = string.Empty;
        }

        public static void SetMainTextWindowActive(bool active)
        {
            Instance.MainTextPanel.SetActive(!string.IsNullOrEmpty(Instance.MainTextPanel.Text) && active);
        }

        private static bool[] GetValidDirections()
        {
            var adjacentRooms = Player?.CurrentRoom?.GetAdjacentRooms();
            bool left = false, up = false, right = false, down = false;

            if (adjacentRooms != null)
            {
                left = adjacentRooms[Direction.West] != null;
                up = adjacentRooms[Direction.North] != null;
                right = adjacentRooms[Direction.East] != null;
                down = adjacentRooms[Direction.South] != null;
            }

            validDirections[0] = left;
            validDirections[1] = up;
            validDirections[2] = right;
            validDirections[3] = down;

            return validDirections;
        }

        private static void SetMenuValidDirections(bool[] validDirections)
        {
            Instance.CurrentControlState.DirectionalActionMenu.SetValidDirections(validDirections);
        }

        public IEnumerator WaitForDurationThenDoAction(WaitDuration waitDuration)
        {
            while (waitDuration.Duration >= float.Epsilon)
            {
                waitDuration.Duration -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            waitDuration.DoAction.Invoke();
        }

        /// <summary>
        /// Waits for predicate to be false then does the action.
        /// </summary>
        /// <param name="waitAction">The wait action.</param>
        /// <returns></returns>
        public IEnumerator WaitForPredicateToBeFalseThenDoAction(WaitAction waitAction)
        {
            while (waitAction.Predicate())
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            waitAction.DoAction.Invoke();
        }

        private void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(GameplayMenuManagerBehaviour));
        }
    }
}
