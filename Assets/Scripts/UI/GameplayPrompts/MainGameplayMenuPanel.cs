#pragma warning disable CS0649
/**************************************************
 *  MainGameplayMenuBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.UI.GameplayPrompts
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    using BeautifulInterface = Interface.Elements.Scripts;

    using OldSchoolGames.HuntTheMuglump.Scripts.Components;
    using OldSchoolGames.HuntTheMuglump.Scripts.Exceptions;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours;
    using OldSchoolGames.HuntTheMuglump.Scripts.MonoBehaviours.GameplayManagement;
    using OldSchoolGames.HuntTheMuglump.Scripts.UI;
    using OldSchoolGames.HuntTheMuglump.Scripts.Utilities;

    public class MainGameplayMenuPanel : PanelBehaviour
    {
        public static GameplayMenuManagerBehaviour GameplayMenuManager { get => GameplayMenuManagerBehaviour.Instance; }

        #region private child references

        [SerializeField]
        private MovementPromptBehaviour movementPrompt;

        [SerializeField]
        private GameplayPromptBehaviour shootPrompt;

        [SerializeField]
        private GameplayPromptBehaviour useItemPrompt;

        [SerializeField]
        private GameplayPromptBehaviour lookPrompt;

        [SerializeField]
        private GameplayPromptBehaviour mapPrompt;

        [SerializeField]
        private GameplayPromptBehaviour cycleArrowsPrompt;

        [SerializeField]
        private GameplayPromptBehaviour cycleItemsPrompt;

        [SerializeField]
        private GameplayPromptBehaviour pausePrompt;

        [SerializeField]
        private GameplayPromptBehaviour stairsPrompt;

        #endregion

        #region public child accessors

        public virtual MovementPromptBehaviour MovementPrompt
        {
            get
            {
                this.ValidateUnityEditorParameter(this.movementPrompt, nameof(this.movementPrompt));

                return this.movementPrompt;
            }
        }

        public GameplayPromptBehaviour ShootPrompt
        {
            get
            {
                this.ValidateUnityEditorParameter(this.shootPrompt, nameof(this.shootPrompt));

                return this.shootPrompt;
            }
        }

        public GameplayPromptBehaviour UseItemPrompt
        {
            get
            {
                this.ValidateUnityEditorParameter(this.useItemPrompt, nameof(this.useItemPrompt));

                return this.useItemPrompt;
            }
        }

        public GameplayPromptBehaviour LookPrompt
        {
            get
            {
                this.ValidateUnityEditorParameter(this.lookPrompt, nameof(this.lookPrompt));

                return this.lookPrompt;
            }
        }

        public GameplayPromptBehaviour MapPrompt
        {
            get
            {
                this.ValidateUnityEditorParameter(this.mapPrompt, nameof(this.mapPrompt));

                return this.mapPrompt;
            }
        }

        public GameplayPromptBehaviour CycleArrowsPrompt
        {
            get
            {
                this.ValidateUnityEditorParameter(this.cycleArrowsPrompt, nameof(this.cycleArrowsPrompt));

                return this.cycleArrowsPrompt;
            }
        }

        public GameplayPromptBehaviour CycleItemsPrompt
        {
            get
            {
                this.ValidateUnityEditorParameter(this.cycleItemsPrompt, nameof(this.cycleItemsPrompt));

                return this.cycleItemsPrompt;
            }
        }

        public GameplayPromptBehaviour PausePrompt
        {
            get
            {
                this.ValidateUnityEditorParameter(this.pausePrompt, nameof(this.pausePrompt));

                return this.pausePrompt;
            }
        }

        public GameplayPromptBehaviour StairsPrompt
        {
            get
            {
                this.ValidateUnityEditorParameter(this.stairsPrompt, nameof(this.stairsPrompt));

                return this.stairsPrompt;
            }
        }

        #endregion

        public bool Active { get; private set; }

        public void SetActive(bool active)
        {
            this.Active = active;
            this.gameObject.SetActive(active);
        }

        public PlayerBehaviour Player { get => PlayerBehaviour.Instance; }

        protected virtual void Start()
        {
        }

        protected override void Update()
        {
            if (this.Player != null && this.Player.CurrentRoom != null && !this.Player.IsWalking && this.Player.CurrentRoom.IsEntrance)
            {
                this.StairsPrompt.Show();
            }
            else
            {
                this.StairsPrompt.Hide();
            }

            base.Update();
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

        public void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(NewGameMenuBehaviour));
        }
    }
}
